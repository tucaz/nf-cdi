using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Nfse", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Nfse
    {
        [XmlElement(ElementName = "InfNfse", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public InfNfse InfNfse { get; set; }
    }
}