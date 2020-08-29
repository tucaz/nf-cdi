using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "ConsultarNfseRpsResposta",
        Namespace = "http://www.ginfes.com.br/servico_consultar_nfse_rps_resposta_v03.xsd")]
    public class ConsultarNfseRpsResposta
    {
        [XmlElement(ElementName = "CompNfse",
            Namespace = "http://www.ginfes.com.br/servico_consultar_nfse_rps_resposta_v03.xsd")]
        public CompNfse CompNfse { get; set; }

        [XmlAttribute(AttributeName = "ns2", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ns2 { get; set; }

        [XmlAttribute(AttributeName = "ns4", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ns4 { get; set; }

        [XmlAttribute(AttributeName = "ns3", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ns3 { get; set; }
    }
}