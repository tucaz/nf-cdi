using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "IdentificacaoTomador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class IdentificacaoTomador
    {
        [XmlElement(ElementName = "CpfCnpj", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public CpfCnpj CpfCnpj { get; set; }
    }
}