using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Servico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Servico
    {
        [XmlElement(ElementName = "Valores", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Valores Valores { get; set; }

        [XmlElement(ElementName = "ItemListaServico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ItemListaServico { get; set; }

        [XmlElement(ElementName = "CodigoTributacaoMunicipio", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string CodigoTributacaoMunicipio { get; set; }

        [XmlElement(ElementName = "Discriminacao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Discriminacao { get; set; }

        [XmlElement(ElementName = "CodigoMunicipio", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string CodigoMunicipio { get; set; }
    }
}