using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NF.DataAccess;
using NF.Hotmart;
using NF.Hotmart.DataContract;
using NF.NotaFiscal.DataContract;

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
        public async Task GenerateNotasFiscais(int startingNumber, string[] transactionListFilter = null)
        {
            _logger.LogInformation($"Generating Notas Fiscais starting with NF number {startingNumber}");

            var transactions = await GetTransactionsFromHotmart.GetAlltransactions();
            _logger.LogDebug("Downloaded all transactions from HotMart");

            var members = await Api.GetAllMembers();
            _logger.LogDebug("Downloaded all members from HotMart");

            transactions = FilterTransactionsIfNeeded(transactionListFilter, transactions);

            var nfs = new List<NotaFiscal>();

            var numeroRPF = startingNumber;
            foreach (var transaction in transactions)
            {
                var nf = NotasFiscaisDb.CreateNotaFiscal(transaction, members, numeroRPF);
                var existing = await NotasFiscaisDb.LoadNotaFiscalByTransaction(nf.HotmartTransaction.Purchase.Transaction);
                if (existing != null) continue;

                nfs.Add(nf);
                numeroRPF++;
            }

            _logger.LogInformation(
                $"{transactions.Count} notas fiscais created. From NF {startingNumber} to {numeroRPF}.");

            await SqLiteBaseRepository.DbConnection().Execute(async conn =>
            {
                foreach (var nf in nfs)
                {
                    NotasFiscaisDb.InsertNotaFiscal(nf, conn);
                }
            }, true);

            _logger.LogInformation("All notas fiscais added to the database");
        }

        private List<Transaction> FilterTransactionsIfNeeded(string[] transactionListFilter,
            List<Transaction> transactions)
        {
            if (transactionListFilter == null || transactionListFilter.Length < 1) return transactions;

            _logger.LogDebug("Filtering transactions");
            var filteredTransactions = new List<Transaction>();

            foreach (var transaction in transactions)
            {
                if (transactionListFilter.Contains(transaction.Purchase.Transaction))
                    filteredTransactions.Add(transaction);
            }

            _logger.LogDebug(
                $"{filteredTransactions.Count} transactions found and {transactions.Count - filteredTransactions.Count} ignored");
            if (filteredTransactions.Count != transactionListFilter.Length)
                _logger.LogWarning(
                    $"{transactionListFilter.Length - filteredTransactions.Count} transactions from the filter were not found");

            return filteredTransactions;
        }

        /// <summary>
        /// Creates a XML file on the folder used to send Notas Fiscais to Receita Federal
        /// </summary>
        public async Task SendNotasFiscaisToReceita(List<NotaFiscal> nfs, string folder,
            Func<NotaFiscal, string> filename = null, Func<NotaFiscal, string> propertyToBeUsed = null)
        {
            if (propertyToBeUsed == null)
                propertyToBeUsed = nf => nf.NotaFiscalRequestXML;

            if (filename == null)
                filename = nf => nf.EnvioLoteRPS;

            foreach (var notaFiscal in nfs)
            {
                notaFiscal.DetailsRequested = true;
                await File.WriteAllTextAsync(Path.Combine(folder, filename(notaFiscal)), propertyToBeUsed(notaFiscal));
            }
        }


        /// <summary>
        /// For each given Nota Fiscal it reads transmission results from given folder and process errors updating the database at the end
        /// </summary>
        public async Task ProcessResultsFromTransmission(List<NotaFiscal> nfs, string returnFolder,
            string errorFolder)
        {
            foreach (var nf in nfs)
            {
                var file = NotasFiscaisDb.ReadReturnResultsFromFolder<EnviarLoteRpsResposta>(returnFolder,
                    nf.RetornoLoteRPS);

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
                var file = NotasFiscaisDb.ReadReturnResultsFromFolder<Validacao>(validationFolder, nf.RetornoValidacao);

                if (file == null) continue;

                nf.Valid = file.CStat == "1";
                nf.Validation = file;
                nf.ValidationXML = NotaFiscal.Serialize(file);
            }

            await NotasFiscaisDb.UpdateNotasFiscais(nfs);
        }

        /// <summary>
        /// For each given Nota Fiscal it reads the XML data that contains details about the Nota Fiscal
        /// </summary>
        public async Task ProcessResultsFromNotaFiscalDetailsRequest(NotaFiscal nf, string folder)
        {
            var path = Path.Combine(folder, nf.RetornoConsultaNFPorLoteRPS);
            var exists = await Wait.UntilFileExists(path);

            if (!exists)
            {
                _logger.LogWarning($"Could not find details for nota fiscal with RPS number \"{nf.NumeroRPS}\" in path \"{path}\"");
                return;
            }

            var file = NotasFiscaisDb.ReadReturnResultsFromFolder<ConsultarNfseRpsResposta>(folder,
                nf.RetornoConsultaNFPorLoteRPS);

            nf.Numero = int.Parse(file.CompNfse.Nfse.InfNfse.Numero);
            nf.NotaFiscalData = file;
            nf.NotaFiscalDataXML = NotaFiscal.Serialize(file);
            nf.HasNotaFiscal = true;

            await NotasFiscaisDb.UpdateNotaFiscal(nf);
        }
    }
}