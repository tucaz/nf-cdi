using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Rps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Rps
    {
        [XmlElement(ElementName = "InfRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public InfRps InfRps { get; set; }
    }
}