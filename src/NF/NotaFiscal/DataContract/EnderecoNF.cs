using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Endereco", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class EnderecoNF
    {
        [XmlElement(ElementName = "Endereco", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Endereco { get; set; }

        [XmlElement(ElementName = "Numero", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Numero { get; set; }

        [XmlElement(ElementName = "Bairro", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Bairro { get; set; }

        [XmlElement(ElementName = "CodigoMunicipio", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string CodigoMunicipio { get; set; }

        [XmlElement(ElementName = "Uf", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Uf { get; set; }

        [XmlElement(ElementName = "Cep", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Cep { get; set; }
    }
}