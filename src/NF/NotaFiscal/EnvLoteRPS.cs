using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace NF.NotaFiscal
{
    [XmlRoot(ElementName = "IdentificacaoRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class IdentificacaoRps
    {
        [XmlElement(ElementName = "Numero", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Numero { get; set; }

        [XmlElement(ElementName = "Serie", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Serie { get; set; }

        [XmlElement(ElementName = "Tipo", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Tipo { get; set; }
    }

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

    [XmlRoot(ElementName = "Servico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Servico
    {
        [XmlElement(ElementName = "Valores", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Valores Valores { get; set; }

        [XmlElement(ElementName = "ItemListaServico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string ItemListaServico { get; set; }

        [XmlElement(ElementName = "CodigoTributacaoMunicipio", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string CodigoTributacaoMunicipio { get; set; }

        [XmlElement(ElementName = "Discriminacao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Discriminacao { get; set; }

        [XmlElement(ElementName = "CodigoMunicipio", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string CodigoMunicipio { get; set; }
    }

    [XmlRoot(ElementName = "Prestador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Prestador
    {
        [XmlElement(ElementName = "Cnpj", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Cnpj { get; set; }

        [XmlElement(ElementName = "InscricaoMunicipal", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string InscricaoMunicipal { get; set; }
    }

    [XmlRoot(ElementName = "CpfCnpj", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class CpfCnpj
    {
        [XmlElement(ElementName = "Cpf", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Cpf { get; set; }
    }

    [XmlRoot(ElementName = "IdentificacaoTomador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class IdentificacaoTomador
    {
        [XmlElement(ElementName = "CpfCnpj", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public CpfCnpj CpfCnpj { get; set; }
    }
    
    [XmlRoot(ElementName = "Contato", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Contato
    {
        [XmlElement(ElementName = "Email", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Email { get; set; }
        [XmlElement(ElementName = "Telefone", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Telefone { get; set; }
    }

    [XmlRoot(ElementName = "Endereco", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class EnderecoRps
    {
        [XmlElement(ElementName = "Endereco", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Endereco { get; set; }

        [XmlElement(ElementName = "Numero", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Numero { get; set; }

        [XmlElement(ElementName = "Bairro", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Bairro { get; set; }

        [XmlElement(ElementName = "CodigoMunicipio", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string CodigoMunicipio { get; set; }

        [XmlElement(ElementName = "Uf", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Uf { get; set; }

        [XmlElement(ElementName = "Cep", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Cep { get; set; }
    }

    [XmlRoot(ElementName = "Tomador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Tomador
    {
        [XmlElement(ElementName = "IdentificacaoTomador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public IdentificacaoTomador IdentificacaoTomador { get; set; }

        [XmlElement(ElementName = "RazaoSocial", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string RazaoSocial { get; set; }

        [XmlElement(ElementName = "Endereco", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public EnderecoRps Endereco { get; set; }
        
        [XmlElement(ElementName = "Contato", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Contato Contato { get; set; }
    }

    [XmlRoot(ElementName = "InfRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class InfRps
    {
        [XmlElement(ElementName = "IdentificacaoRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public IdentificacaoRps IdentificacaoRps { get; set; }

        [XmlElement(ElementName = "DataEmissao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public DateTime DataEmissao { get; set; }

        [XmlElement(ElementName = "NaturezaOperacao", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string NaturezaOperacao { get; set; }


        [XmlElement(ElementName = "OptanteSimplesNacional", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string OptanteSimplesNacional { get; set; }

        [XmlElement(ElementName = "IncentivadorCultural", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string IncentivadorCultural { get; set; }

        [XmlElement(ElementName = "Status", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public string Status { get; set; }

        [XmlElement(ElementName = "Servico", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Servico Servico { get; set; }

        [XmlElement(ElementName = "Prestador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Prestador Prestador { get; set; }

        [XmlElement(ElementName = "Tomador", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Tomador Tomador { get; set; }
    }

    [XmlRoot(ElementName = "Rps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class Rps
    {
        [XmlElement(ElementName = "InfRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public InfRps InfRps { get; set; }
    }

    [XmlRoot(ElementName = "ListaRps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
    public class ListaRps
    {
        [XmlElement(ElementName = "Rps", Namespace = "http://www.ginfes.com.br/tipos_v03.xsd")]
        public Rps Rps { get; set; }
    }

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