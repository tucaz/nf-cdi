using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "ListaRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class ListaRps
    {
        [XmlElement(ElementName = "Rps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Rps Rps { get; set; }
    }
}