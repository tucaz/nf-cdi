using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using NF.DataAccess;
using NF.Hotmart;
using NF.Hotmart.DataContract;

namespace NF.NotaFiscal
{
    public static class NotasFiscaisDb
    {
        /// <summary>
        /// Reads the XML file with return data from Prefeitura
        /// </summary>
        public static T ReadReturnResultsFromFolder<T>(string folder, string file) where T : class
        {
            var path = Path.Combine(folder, file);

            if (!File.Exists(path)) throw new ArgumentOutOfRangeException();

            var content = File.ReadAllText(path);
            return NotaFiscal.Deserialize<T>(content);
        }


        /// <summary>
        /// Loads all Notas Fiscais from the database
        /// </summary>
        /// <returns></returns>
        public static async Task<List<NotaFiscal>> LoadAllNotasFiscais()
        {
            const string sql =
                @"SELECT 
                    id as Id, 
                    member as HotmartMember, 
                    purchase as HotmartTransaction, 
                    rps_number as NumeroRPS, 
                    nf_number as Numero, 
                    nf_request as NotaFiscalRequest, 
                    nf_request_xml as NotaFiscalRequestXML, 
                    valid as Valid, 
                    validation as Validation,
                    validation_xml as ValidationXML,               
                    sent as Sent, 
                    sucessfully_transmitted as SuccessfullyTransmitted,             
                    nf_response as NotaFiscalResponse, 
                    nf_response_xml as NotaFiscalResponseXML,                     
                    details_requested as DetailsRequested,
                    has_nf as HasNotaFiscal,
                    nf_data as NotaFiscalData,
                    nf_data_xml as NotaFiscalDataXML,
                    is_foreigner as IsForeigner, 
                    invalid_address as InvalidAddress, 
                    us_dolar as IsUSDolar, 
                    nf_data as NotaFiscalData, 
                    nf_data_xml as NotaFiscalDataXML,                    
                    created as Created 
                 FROM 
                      nota_fiscal WHERE rps_number NOT IN (212,222)
                 ORDER BY 
                    rps_number ASC";
            var results = await SqLiteBaseRepository.DbConnection().Query<NotaFiscal>(sql, null, true);
            return results.ToList();
        }

        /// <summary>
        /// Insert a new Nota Fiscal into the database
        /// </summary>
        public static void InsertNotaFiscal(NotaFiscal nf, SQLiteConnection conn)
        {
            conn.Execute(
                @"INSERT INTO nota_fiscal (member, purchase, rps_number, nf_request, nf_request_xml, sent, details_requested, has_nf, is_foreigner, invalid_address, us_dolar, created) 
VALUES (json(@member), json(@purchase), @rps_number, @nf_request, @nf_request_xml, @sent, @details_requested, @has_nf, @is_foreigner, @invalid_address, @us_dolar, @created)",
                new
                {
                    member = JsonConvert.SerializeObject(nf.HotmartMember),
                    purchase = JsonConvert.SerializeObject(nf.HotmartTransaction),
                    rps_number = nf.NumeroRPS,
                    nf_request = JsonConvert.SerializeObject(nf.NotaFiscalRequest),
                    nf_request_xml = nf.NotaFiscalRequestXML,
                    sent = 0,
                    details_requested = nf.DetailsRequested,
                    has_nf = nf.HasNotaFiscal,
                    is_foreigner = nf.IsForeigner,
                    invalid_address = nf.InvalidAddress,
                    us_dolar = nf.IsUSDolar,
                    created = DateTime.UtcNow.ToString("o")
                });
        }


        public static async Task<NotaFiscal> LoadNotaFiscalByTransaction(string transactionCode)
        {
            const string sql =
                @"SELECT 
                    id as Id, 
                    member as HotmartMember, 
                    purchase as HotmartTransaction, 
                    rps_number as NumeroRPS, 
                    nf_number as Numero, 
                    nf_request as NotaFiscalRequest, 
                    nf_request_xml as NotaFiscalRequestXML, 
                    valid as Valid, 
                    validation as Validation,
                    validation_xml as ValidationXML,               
                    sent as Sent, 
                    sucessfully_transmitted as SuccessfullyTransmitted,             
                    nf_response as NotaFiscalResponse, 
                    nf_response_xml as NotaFiscalResponseXML,                     
                    details_requested as DetailsRequested,
                    has_nf as HasNotaFiscal,
                    nf_data as NotaFiscalData,
                    nf_data_xml as NotaFiscalDataXML,
                    is_foreigner as IsForeigner, 
                    invalid_address as InvalidAddress, 
                    us_dolar as IsUSDolar, 
                    nf_data as NotaFiscalData, 
                    nf_data_xml as NotaFiscalDataXML,                    
                    created as Created 
                 FROM 
                      nota_fiscal 
                WHERE 
                    json_extract(purchase, '$.Purchase.Transaction') = @transactionCode
                 ORDER BY 
                    rps_number ASC";
            var results = await SqLiteBaseRepository.DbConnection().Query<NotaFiscal>(sql, new { transactionCode }, true);
            return results.FirstOrDefault();
        }

        /// <summary>
        /// Creates a <see cref="NotaFiscal"/> given the transaction from Hotmart and member who did it
        /// It generates the required XML to be transmitted to Receita Federal
        /// </summary>
        /// <param name="numeroRPS">Number that will be used for the Nota Fiscal</param>
        /// <returns></returns>
        public static NotaFiscal CreateNotaFiscal(Transaction transaction, List<Member> members, int numeroRPS)
        {
            var member = members.First(m => m.Id == transaction.Buyer.Id);
            var loteRps = NotaFiscal.GenerateLoteRPS(numeroRPS, numeroRPS, transaction,
                out bool isForeigner, out bool invalidAddress, out bool isUSDolar);
            var loteRpsXML = NotaFiscal.Serialize(loteRps);

            var nf = new NotaFiscal
            {
                NumeroRPS = numeroRPS,
                Created = DateTime.Now,
                HotmartTransaction = transaction,
                HotmartMember = member,
                NotaFiscalRequest = loteRps,
                NotaFiscalRequestXML = loteRpsXML,
                IsForeigner = isForeigner,
                InvalidAddress = invalidAddress,
                IsUSDolar = isUSDolar
            };

            return nf;
        }

        /// <summary>
        /// Updates all fields for the given Notas Fiscais
        /// </summary>
        public static async Task UpdateNotasFiscais(List<NotaFiscal> notasFiscais)
        {
            await SqLiteBaseRepository.DbConnection().Execute(conn =>
            {
                foreach (var nf in notasFiscais)
                {
                    UpdateNotaFiscal(nf, conn);
                }
            }, true);
        }

        /// <summary>
        /// Updates all fields for the given Notas Fiscal
        /// </summary>
        public static async Task UpdateNotaFiscal(NotaFiscal notasFiscal)
        {
            await SqLiteBaseRepository.DbConnection().Execute(conn => { UpdateNotaFiscal(notasFiscal, conn); }, true);
        }


        /// <summary>
        /// Update all fields for the given Nota Fiscal
        /// </summary>
        private static void UpdateNotaFiscal(NotaFiscal nf, SQLiteConnection conn)
        {
            conn.Execute(
                @"UPDATE nota_fiscal SET 
                            member=json(@member), 
                            purchase=json(@purchase),
                            rps_number = @rps_number, 
                            nf_number=@nf_number, 
                            nf_request=json(@nf_request), 
                            nf_request_xml=@nf_request_xml, 
                            valid=@valid, 
                            validation=json(@validation), 
                            validation_xml=@validation_xml,
                            sent=@sent, 
                            sucessfully_transmitted=@sucessfully_transmitted, 
                            nf_response = json(@nf_response), 
                            nf_response_xml = @nf_response_xml,
                            details_requested = @details_requested,
                            has_nf = @has_nf,
                            nf_data = json(@nf_data), 
                            nf_data_xml = @nf_data_xml,  
                            is_foreigner=@is_foreigner, 
                            invalid_address=@invalid_address 
                        WHERE id=@id",
                new
                {
                    member = JsonConvert.SerializeObject(nf.HotmartMember),
                    purchase = JsonConvert.SerializeObject(nf.HotmartTransaction),
                    rps_number = nf.NumeroRPS,
                    nf_number = nf.Numero == 0 ? null : (int?) nf.Numero,
                    nf_request = JsonConvert.SerializeObject(nf.NotaFiscalRequest),
                    nf_request_xml = nf.NotaFiscalRequestXML,

                    valid = nf.Valid,
                    validation = JsonConvert.SerializeObject(nf.Validation),
                    validation_xml = nf.ValidationXML,

                    sent = nf.Sent,
                    sucessfully_transmitted = nf.SuccessfullyTransmitted,
                    nf_response = nf.NotaFiscalResponse,
                    nf_response_xml = nf.NotaFiscalResponseXML,

                    details_requested = nf.DetailsRequested,
                    has_nf = nf.HasNotaFiscal,
                    nf_data = JsonConvert.SerializeObject(nf.NotaFiscalData),
                    nf_data_xml = nf.NotaFiscalDataXML,

                    is_foreigner = nf.IsForeigner,
                    invalid_address = nf.InvalidAddress,
                    id = nf.Id
                });
        }
    }
}