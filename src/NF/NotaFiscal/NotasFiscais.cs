using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NF.DataAccess;
using NF.Hotmart;

namespace NF.NotaFiscal
{
    public class NotasFiscais
    {
        private readonly ILogger<NotasFiscais> _logger;

        public NotasFiscais(ILogger<NotasFiscais> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Creates a XML file on the folder used for Validation for each given Nota Fiscal 
        /// </summary>
        public async Task SendNotasFiscaisToValidation(IEnumerable<NotaFiscal> nfs, string folder)
        {
            foreach (var notaFiscal in nfs)
            {
                await File.WriteAllTextAsync(Path.Combine(folder, notaFiscal.EnvioLoteRPS),
                    notaFiscal.NotaFiscalRequestXML);
            }
        }

        /// <summary>
        /// Loads all transactions from Hotmart and create a Nota Fiscal based on it inserting them into the database
        /// </summary>
        public async Task GenerateNotasFiscais(int startingNumber)
        {
            
            _logger.LogInformation($"Generating Notas Fiscais starting with NF number {startingNumber}");
            
            var transactions = await GetTransactionsFromHotmart.GetAlltransactions();
            _logger.LogDebug("Downloaded all transactions from HotMart");
            
            
            var members = await Api.GetAllMembers();
            _logger.LogDebug("Downloaded all members from HotMart");

            
            var nfs = new List<NotaFiscal>();

            var numeroNF = startingNumber;
            foreach (var transaction in transactions)
            {
                var nf = NotasFiscaisDb.CreateNotaFiscal(transaction, members, numeroNF);
                nfs.Add(nf);
                numeroNF++;
            }
            
            _logger.LogInformation($"{transactions.Count} notas fiscais created. From NF {startingNumber} to {numeroNF}.");

            await SqLiteBaseRepository.DbConnection().Execute(conn =>
            {
                foreach (var nf in nfs)
                {
                    NotasFiscaisDb.InsertNotaFiscal(nf, conn);
                }
            }, true);
            
            _logger.LogInformation("All notas fiscais added to the database");
        }

        /// <summary>
        /// Creates a XML file on the folder used to send Notas Fiscais to Receita Federal
        /// </summary>
        public async Task SendNotasFiscaisToReceita(List<NotaFiscal> nfs, string folder)
        {
            foreach (var notaFiscal in nfs.Where(nf => nf.Valid.HasValue && nf.Valid.Value && !nf.Sent))
            {
                await File.WriteAllTextAsync(Path.Combine(folder, notaFiscal.EnvioLoteRPS),
                    notaFiscal.NotaFiscalRequestXML);
                notaFiscal.Sent = true;
            }

            await NotasFiscaisDb.UpdateNotasFiscais(nfs);
        }


        /// <summary>
        /// For each given Nota Fiscal it reads transmission results from given folder and process errors updating the database at the end
        /// </summary>
        public async Task ProcessResultsFromTransmission(List<NotaFiscal> nfs, string returnFolder,
            string errorFolder)
        {

            foreach (var nf in nfs)
            {
                var file = NotasFiscaisDb.ReadTransmissionResultFromFolder(returnFolder, nf.RetornoLoteRPS);

                nf.NotaFiscalResponseXML = NotaFiscal.Serialize(file);
                nf.NotaFiscalResponse = file;
                nf.SuccessfullyTransmitted = true;
            }

            await NotasFiscaisDb.UpdateNotasFiscais(nfs);
        }

        /// <summary>
        /// For each given Nota Fiscal it reads validation results from given folder and process errors updating the database at the end
        /// </summary>
        public async Task ProcessResultsFromValidation(List<NotaFiscal> nfs, string validationFolder)
        {
            foreach (var nf in nfs)
            {
                var file = NotasFiscaisDb.ReadRetornoValidacaoFromFolder(validationFolder, nf.RetornoValidacao);

                if (file == null) continue;

                nf.Valid = file.CStat == "1";
                nf.Validation = file;
                nf.ValidationXML = NotaFiscal.Serialize(file);
            }

            await NotasFiscaisDb.UpdateNotasFiscais(nfs);
        }
    }
}