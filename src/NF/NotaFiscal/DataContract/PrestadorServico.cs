using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "PrestadorServico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class PrestadorServico
    {
        [XmlElement(ElementName = "IdentificacaoPrestador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public IdentificacaoPrestador IdentificacaoPrestador { get; set; }

        [XmlElement(ElementName = "RazaoSocial", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string RazaoSocial { get; set; }

        [XmlElement(ElementName = "NomeFantasia", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string NomeFantasia { get; set; }

        [XmlElement(ElementName = "Endereco", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public EnderecoNF Endereco { get; set; }

        [XmlElement(ElementName = "Contato", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Contato Contato { get; set; }
    }
}