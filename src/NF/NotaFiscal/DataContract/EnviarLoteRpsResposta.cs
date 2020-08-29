using System;
using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "EnviarLoteRpsResposta", Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_resposta_v03.xsd")]
    public class EnviarLoteRpsResposta
    {
        [XmlElement(ElementName = "NumeroLote",
            Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_resposta_v03.xsd")]
        public string NumeroLote { get; set; }

        [XmlElement(ElementName = "DataRecebimento",
            Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_resposta_v03.xsd")]
        public DateTime DataRecebimento { get; set; }

        [XmlElement(ElementName = "Protocolo",
            Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_resposta_v03.xsd")]
        public string Protocolo { get; set; }

    }
}