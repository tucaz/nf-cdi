namespace NF.NotaFiscal.DataContract
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public partial class tcIdentificacaoRps
    {
        private string numeroField;

        private string serieField;

        private sbyte tipoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string Numero
        {
            get { return this.numeroField; }
            set { this.numeroField = value; }
        }

        /// <remarks/>
        public string Serie
        {
            get { return this.serieField; }
            set { this.serieField = value; }
        }

        /// <remarks/>
        public sbyte Tipo
        {
            get { return this.tipoField; }
            set { this.tipoField = value; }
        }
    }
}