using System.Xml.Serialization;

namespace NF.NotaFiscal.DataContract
{
    [XmlRoot(ElementName = "Validacao")]
    public class Validacao
    {
        [XmlElement(ElementName = "cStat")] public string CStat { get; set; }
        [XmlElement(ElementName = "xMotivo")] public string XMotivo { get; set; }
    }
}