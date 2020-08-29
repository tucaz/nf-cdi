using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NF.NotaFiscal
{
    public class ProcessNotasFiscais
    {
        private const int TAKE = 600;
        public NotasFiscais NFs { get; }
        private readonly ILogger<ProcessNotasFiscais> _logger;

        public ProcessNotasFiscais(ILogger<ProcessNotasFiscais> logger, NotasFiscais NFs)
        {
            this.NFs = NFs;
            _logger = logger;
        }

        public async Task ProcessTransmissionResults()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.ReadyForTransmissionResult)
                .ToList();

            await NFs.ProcessResultsFromTransmission(nfs,
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Retorno\",
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Erro\");
        }

        public async Task TransmitNotasFiscais()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.ReadyForTransmission)
                .Take(TAKE)
                .ToList();

            await NFs.SendNotasFiscaisToReceita(nfs,
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Envio\");
            
            nfs.ForEach(nf => nf.Sent = true);
            await NotasFiscaisDb.UpdateNotasFiscais(nfs);
        }

        public async Task ProcessValidationResults()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.ReadyForValidation)
                .Take(TAKE)
                .ToList();
            
            _logger.LogInformation($"{nfs.Count} notas fiscais loaded so their validation results can be processed.");

            var files = NFs.ProcessResultsFromValidation(nfs,
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Retorno\");
            
            _logger.LogInformation($"{nfs.Count} notas fiscais had validation results processed and updated on the database.");
        }

        public async Task SendNotasFiscaisToValidation()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.ReadyForValidation)
                .Take(TAKE)
                .ToList();

            _logger.LogInformation($"{nfs.Count} notas fiscais loaded to be sent to validation.");

            var validationFolder = @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Validar\";
            await NFs.SendNotasFiscaisToValidation(nfs, validationFolder);

            _logger.LogInformation($"{nfs.Count} sent to validation on folder {validationFolder}");
        }

        public async Task CreateNotasFiscaisFromHotmartTransactions(int startingNumber, string[] transactionListFilter=null)
        {
            await NFs.GenerateNotasFiscais(startingNumber, transactionListFilter);
        }
        
        public async Task UpdateNotasFiscaisData()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.NotaFiscalReady)
                .Take(TAKE)
                .ToList();
        }

        public async Task ProcessNotaFiscalDetails()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.NotaFiscalReady)
                .Take(TAKE)
                .ToList();

            foreach (var notaFiscal in nfs)
            {
                await NFs.SendNotasFiscaisToReceita(new List<NotaFiscal>() { notaFiscal }, 
                    @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Envio\",
                    nf => nf.ConsultaNFPorLoteRPS, 
                    nf => NotaFiscal.Serialize(NotaFiscal.GenerateConsultaNFPorLoteRPS(nf.NumeroRPS))
                );

                await NFs.ProcessResultsFromNotaFiscalDetailsRequest(notaFiscal,
                    @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Retorno\");

                await Task.Delay(5000);
            }
        }

        public async Task SendRequestForNotaFiscalDetails()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.NotaFiscalReady)
                .Take(TAKE)
                .ToList();

            await NFs.SendNotasFiscaisToReceita(nfs,
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Envio\", nf => nf.ConsultaNFPorLoteRPS, nf => NotaFiscal.Serialize(NotaFiscal.GenerateConsultaNFPorLoteRPS(nf.NumeroRPS)));
        }
        
        // public async Task ProcessNotaFiscalDetailsRequest()
        // {
        //     var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
        //         .Where(nf => nf.ReadyForValidation)
        //         .Take(TAKE)
        //         .ToList();
        //     
        //     _logger.LogInformation($"{nfs.Count} notas fiscais loaded so their details can be loaded.");
        //
        //     var files = NFs.ProcessResultsFromNotaFiscalDetailsRequest(nfs,
        //         @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Retorno");
        //     
        //     _logger.LogInformation($"{nfs.Count} notas fiscais had details loaded and updated on the database.");
        // }
    }
}