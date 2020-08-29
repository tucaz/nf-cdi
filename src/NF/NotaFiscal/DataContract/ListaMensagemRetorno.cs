namespace NF.NotaFiscal.DataContract
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,
        Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.ginfes.com.br/tipos_v03.xsd",
        IsNullable = false)]
    public partial class ListaMensagemRetorno
    {
        private tcMensagemRetorno[] mensagemRetornoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MensagemRetorno")]
        public tcMensagemRetorno[] MensagemRetorno
        {
            get { return this.mensagemRetornoField; }
            set { this.mensagemRetornoField = value; }
        }
    }
}