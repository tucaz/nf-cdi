using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "CpfCnpj", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class CpfCnpj
    {
        [XmlElement(ElementName = "Cpf", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Cpf { get; set; }
        [XmlElement(ElementName = "Cnpj", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Cnpj { get; set; }
    }
}