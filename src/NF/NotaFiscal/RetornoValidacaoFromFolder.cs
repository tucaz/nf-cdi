using NF.NotaFiscal.DataContract;

namespace NF.NotaFiscal
{
    public class RetornoValidacaoFromFolder
    {
        public int Lote { get; set; }
        public Validacao Content { get; set; }
    }
}