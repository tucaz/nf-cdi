namespace NF.NotaFiscal.DataContract
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public partial class tcMensagemRetorno
    {
        private string codigoField;

        private string mensagemField;

        private string correcaoField;

        /// <remarks/>
        public string Codigo
        {
            get { return this.codigoField; }
            set { this.codigoField = value; }
        }

        /// <remarks/>
        public string Mensagem
        {
            get { return this.mensagemField; }
            set { this.mensagemField = value; }
        }

        /// <remarks/>
        public string Correcao
        {
            get { return this.correcaoField; }
            set { this.correcaoField = value; }
        }
    }
}