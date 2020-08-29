using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "LoteRps", Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_envio_v03.xsd")]
    public class LoteRps
    {
        [XmlElement(ElementName = "NumeroLote", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string NumeroLote { get; set; }

        [XmlElement(ElementName = "Cnpj", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Cnpj { get; set; }

        [XmlElement(ElementName = "InscricaoMunicipal", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string InscricaoMunicipal { get; set; }

        [XmlElement(ElementName = "QuantidadeRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string QuantidadeRps { get; set; }

        [XmlElement(ElementName = "ListaRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public ListaRps ListaRps { get; set; }

        [XmlAttribute(AttributeName = "Id")] public string Id { get; set; }

        [XmlAttribute(AttributeName = "tipos", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Tipos { get; set; }
    }
}