using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "OrgaoGerador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class OrgaoGerador
    {
        [XmlElement(ElementName = "CodigoMunicipio", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string CodigoMunicipio { get; set; }

        [XmlElement(ElementName = "Uf", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Uf { get; set; }
    }
}