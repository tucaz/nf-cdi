using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "IdentificacaoRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class IdentificacaoRps
    {
        [XmlElement(ElementName = "Numero", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Numero { get; set; }

        [XmlElement(ElementName = "Serie", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Serie { get; set; }

        [XmlElement(ElementName = "Tipo", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Tipo { get; set; }
    }
}