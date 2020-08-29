using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Valores", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Valores
    {
        [XmlElement(ElementName = "ValorServicos", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorServicos { get; set; }

        [XmlElement(ElementName = "ValorDeducoes", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorDeducoes { get; set; }

        [XmlElement(ElementName = "ValorPis", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorPis { get; set; }

        [XmlElement(ElementName = "ValorCofins", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorCofins { get; set; }

        [XmlElement(ElementName = "ValorInss", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorInss { get; set; }

        [XmlElement(ElementName = "ValorIr", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorIr { get; set; }

        [XmlElement(ElementName = "ValorCsll", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorCsll { get; set; }

        [XmlElement(ElementName = "IssRetido", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string IssRetido { get; set; }

        [XmlElement(ElementName = "ValorIss", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorIss { get; set; }

        [XmlElement(ElementName = "ValorIssRetido", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorIssRetido { get; set; }

        [XmlElement(ElementName = "OutrasRetencoes", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string OutrasRetencoes { get; set; }

        [XmlElement(ElementName = "BaseCalculo", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string BaseCalculo { get; set; }

        [XmlElement(ElementName = "Aliquota", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Aliquota { get; set; }

        [XmlElement(ElementName = "ValorLiquidoNfse", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ValorLiquidoNfse { get; set; }
    }
}