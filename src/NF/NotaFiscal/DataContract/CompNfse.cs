using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "CompNfse",
        Namespace = "http://www.ginfes.com.br/servico_consultar_nfse_rps_resposta_v03.xsd")]
    public class CompNfse
    {
        [XmlElement(ElementName = "Nfse", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Nfse Nfse { get; set; }
    }
}