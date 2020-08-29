using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "EnviarLoteRpsEnvio",
        Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_envio_v03.xsd")]
    public class EnviarLoteRpsEnvio
    {
        [XmlElement(ElementName = "LoteRps",
            Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_envio_v03.xsd")]
        public LoteRps LoteRps { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}