using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Tomador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Tomador
    {
        [XmlElement(ElementName = "IdentificacaoTomador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public IdentificacaoTomador IdentificacaoTomador { get; set; }

        [XmlElement(ElementName = "RazaoSocial", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string RazaoSocial { get; set; }

        [XmlElement(ElementName = "Endereco", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public EnderecoRps Endereco { get; set; }
        
        [XmlElement(ElementName = "Contato", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Contato Contato { get; set; }
    }
}