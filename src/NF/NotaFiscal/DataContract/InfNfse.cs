using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "InfNfse", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class InfNfse
    {
        [XmlElement(ElementName = "Numero", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Numero { get; set; }

        [XmlElement(ElementName = "CodigoVerificacao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string CodigoVerificacao { get; set; }

        [XmlElement(ElementName = "DataEmissao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string DataEmissao { get; set; }

        [XmlElement(ElementName = "IdentificacaoRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public IdentificacaoRps IdentificacaoRps { get; set; }

        [XmlElement(ElementName = "DataEmissaoRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string DataEmissaoRps { get; set; }

        [XmlElement(ElementName = "NaturezaOperacao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string NaturezaOperacao { get; set; }

        [XmlElement(ElementName = "RegimeEspecialTributacao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string RegimeEspecialTributacao { get; set; }

        [XmlElement(ElementName = "OptanteSimplesNacional", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string OptanteSimplesNacional { get; set; }

        [XmlElement(ElementName = "IncentivadorCultural", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string IncentivadorCultural { get; set; }

        [XmlElement(ElementName = "Competencia", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Competencia { get; set; }

        [XmlElement(ElementName = "Servico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Servico Servico { get; set; }

        [XmlElement(ElementName = "ValorCredito", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorCredito { get; set; }

        [XmlElement(ElementName = "PrestadorServico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public PrestadorServico PrestadorServico { get; set; }

        [XmlElement(ElementName = "TomadorServico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public TomadorServico TomadorServico { get; set; }

        [XmlElement(ElementName = "OrgaoGerador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public OrgaoGerador OrgaoGerador { get; set; }
    }
}