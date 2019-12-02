using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace NF.NotaFiscal
{
    [XmlRoot(ElementName = "EnviarLoteRpsResposta",
        Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_resposta_v03.xsd")]
    public class EnviarLoteRpsResposta
    {
        [XmlElement(ElementName = "NumeroLote",
            Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_resposta_v03.xsd")]
        public string NumeroLote { get; set; }

        [XmlElement(ElementName = "DataRecebimento",
            Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_resposta_v03.xsd")]
        public string DataRecebimento { get; set; }

        [XmlElement(ElementName = "Protocolo",
            Namespace = "http://www.ginfes.com.br/servico_enviar_lote_rps_resposta_v03.xsd")]
        public string Protocolo { get; set; }

        [XmlAttribute(AttributeName = "ns2", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ns2 { get; set; }

        [XmlAttribute(AttributeName = "ns3", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ns3 { get; set; }
    }
}