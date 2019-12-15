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

namespace NF.NotaFiscal
{
    public static class NotasFiscaisDb
    {

        /// <summary>
        /// Reads the XML file with results from validation
        /// </summary>
        public static EnviarLoteRpsResposta ReadTransmissionResultFromFolder(string folder, string file)
        {
            var path = Path.Combine(folder, file);
            
            if(!File.Exists(path)) throw new ArgumentOutOfRangeException();

            var content = File.ReadAllText(path);
            return NotaFiscal.Deserialize<EnviarLoteRpsResposta>(content);
        }
        
        /// <summary>
        /// Reads the XML file with results from validation
        /// </summary>
        public static Validacao ReadRetornoValidacaoFromFolder(string folder, string file)
        {
            var path = Path.Combine(folder, file);
            
            if(!File.Exists(path)) throw new ArgumentOutOfRangeException();

            var content = File.ReadAllText(path);
            return NotaFiscal.Deserialize<Validacao>(content);
        }

        /// <summary>
        /// Loads all Notas Fiscais from the database
        /// </summary>
        /// <returns></returns>
        public static async Task<List<NotaFiscal>> LoadAllNotasFiscais()
        {
            const string sql =
                "SELECT id as Id, member as HotmartMember, purchase as HotmartTransaction , nf_number as Numero, nf_request as NotaFiscalRequest, nf_request_xml as NotaFiscalRequestXML, nf_response as NotaFiscalResponse, nf_response_xml as NotaFiscaResponseXML, sent as Sent, sucessfully_transmitted as SuccessfullyTransmitted, is_foreigner as IsForeigner, invalid_address as InvalidAddress, us_dolar as IsUSDolar, valid as Valid, validation as Validation, validation_xml as ValidationXML, created as Created FROM nota_fiscal";
            var results = await SqLiteBaseRepository.DbConnection().Query<NotaFiscal>(sql);
            return results.Where(nf => nf.Numero != 212 && nf.Numero != 222).ToList();
        }

        /// <summary>
        /// Insert a new Nota Fiscal into the database
        /// </summary>
        public static void InsertNotaFiscal(NotaFiscal nf, SQLiteConnection conn)
        {
            conn.Execute(
                @"INSERT INTO nota_fiscal (member, purchase, nf_number, nf_request, nf_request_xml, sent, is_foreigner, invalid_address, us_dolar, created) 
VALUES (json(@member), json(@purchase), @nf_number, @nf_request, @nf_request_xml, @sent, @is_foreigner, @invalid_address, @us_dolar, @created)",
                new
                {
                    member = JsonConvert.SerializeObject(nf.HotmartMember),
                    purchase = JsonConvert.SerializeObject(nf.HotmartTransaction),
                    nf_number = nf.Numero,
                    nf_request = JsonConvert.SerializeObject(nf.NotaFiscalRequest),
                    nf_request_xml = nf.NotaFiscalRequestXML,
                    sent = 0,
                    is_foreigner = nf.IsForeigner,
                    invalid_address = nf.InvalidAddress,
                    us_dolar = nf.IsUSDolar,
                    created = DateTime.UtcNow.ToString("o")
                });
        }

    
        /// <summary>
        /// Creates a <see cref="NotaFiscal"/> given the transaction from Hotmart and member who did it
        /// It generates the required XML to be transmitted to Receita Federal
        /// </summary>
        /// <param name="numeroNotaFiscal">Number that will be used for the Nota Fiscal</param>
        /// <returns></returns>
        public static NotaFiscal CreateNotaFiscal(Transaction transaction, List<Member> members, int numeroNotaFiscal)
        {
            var member = members.First(m => m.Id == transaction.Buyer.Id);
            var loteRps = NotaFiscal.GenerateLoteRPS(numeroNotaFiscal, numeroNotaFiscal, transaction,
                out bool isForeigner, out bool invalidAddress, out bool isUSDolar);
            var loteRpsXML = NotaFiscal.Serialize(loteRps);

            var nf = new NotaFiscal
            {
                Numero = numeroNotaFiscal,
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
        /// Update all fields for the given Nota Fiscal
        /// </summary>
        private static void UpdateNotaFiscal(NotaFiscal nf, SQLiteConnection conn)
        {
            conn.Execute(
                @"UPDATE  nota_fiscal SET 
                            member=json(@member), purchase=json(@purchase), 
                            nf_number=@nf_number, nf_request=json(@nf_request), 
                            nf_request_xml=@nf_request_xml, sent=@sent, 
                            is_foreigner=@is_foreigner, invalid_address=@invalid_address, 
                            valid=@valid, validation=json(@validation), validation_xml=@validation_xml,
                            sucessfully_transmitted=@sucessfully_transmitted, 
                            nf_response = json(@nf_response), nf_response_xml = @nf_response_xml  
                        WHERE id=@id",
                new
                {
                    member = JsonConvert.SerializeObject(nf.HotmartMember),
                    purchase = JsonConvert.SerializeObject(nf.HotmartTransaction),
                    nf_number = nf.Numero,
                    nf_request = JsonConvert.SerializeObject(nf.NotaFiscalRequest),
                    nf_request_xml = nf.NotaFiscalRequestXML,

                    valid = nf.Valid,
                    validation = JsonConvert.SerializeObject(nf.Validation),
                    validation_xml = nf.ValidationXML,

                    sent = nf.Sent,
                    sucessfully_transmitted = nf.SuccessfullyTransmitted,
                    nf_response = nf.NotaFiscalResponse,
                    nf_response_xml = nf.NotaFiscalResponseXML,

                    is_foreigner = nf.IsForeigner,
                    invalid_address = nf.InvalidAddress,
                    id = nf.Id
                });
        }
    }
}