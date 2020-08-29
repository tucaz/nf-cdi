using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Contato", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Contato
    {
        [XmlElement(ElementName = "Telefone", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Telefone { get; set; }
        [XmlElement(ElementName = "Email", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Email { get; set; }
    }
}