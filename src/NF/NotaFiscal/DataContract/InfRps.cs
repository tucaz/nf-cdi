using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "InfRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class InfRps
    {
        [XmlElement(ElementName = "IdentificacaoRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public IdentificacaoRps IdentificacaoRps { get; set; }

        [XmlElement(ElementName = "DataEmissao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string DataEmissao { get; set; }

        [XmlElement(ElementName = "NaturezaOperacao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string NaturezaOperacao { get; set; }
        
        [XmlElement(ElementName = "RegimeEspecialTributacao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string RegimeEspecialTributacao { get; set; }

        [XmlElement(ElementName = "OptanteSimplesNacional", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string OptanteSimplesNacional { get; set; }

        [XmlElement(ElementName = "IncentivadorCultural", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string IncentivadorCultural { get; set; }

        [XmlElement(ElementName = "Status", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Servico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Servico Servico { get; set; }

        [XmlElement(ElementName = "Prestador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Prestador Prestador { get; set; }

        [XmlElement(ElementName = "Tomador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Tomador Tomador { get; set; }
    }
}