using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Prestador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Prestador
    {
        [XmlElement(ElementName = "Cnpj", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Cnpj { get; set; }

        [XmlElement(ElementName = "InscricaoMunicipal", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string InscricaoMunicipal { get; set; }
    }
}