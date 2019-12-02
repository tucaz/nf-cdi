using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using NF.Hotmart;

namespace NF.NotaFiscal
{
    public static class GenerateNotaFiscal
    {
        public static EnviarLoteRpsEnvio Generate(int numeroLote, int numeroNF, Transaction transaction, out bool isForeigner, out bool invalidAddress)
        {
            isForeigner = transaction.Buyer.Address.Country != "Brasil";
            
            const string CNPJ = "34231972000109";
            const string InscricaoMunicipal = "269905";
            const string SerieNF = "NFSE";
            const string TipoNF = "1";
            const string NaturezaOperacaoNF = "1";
            const string OptanteSimplesNacionalNF = "1"; //1-Sim, 2-Não
            const string IncentivadorCulturalNF = "2"; //1-Sim, 2-Não
            const string StatusNF = "1";
            const string DescricaoServicoNF = "Contratacao do Curso de Ingles NOVO Ingles - Modulo 01";
            const string CodigoTributacaoMunicipioNF = "1.05 / 631940002";
            const string CodigoMunicipioPrestacaoServicoNF = "3547809";
            const string ItemListaServicoNF = "105"; //1.05
            const string ISSRetido = "2"; //1-Sim, 2-Não

            var member = transaction.Buyer;
            var address = member.Address;
            var valor = transaction.Purchase.Price.Value.ToString("F");
            var zero = "0.00";
            var aliquota = "0.0217";
            EnderecoRps endereco;

            if (isForeigner)
            {
                endereco = new EnderecoRps
                {
                    Bairro = "Vila Pires",
                    Cep = "09121420",
                    Numero = "350",
                    Uf = "SP",
                    Endereco = "Rua Vera Cruz",
                    CodigoMunicipio = CodigoMunicipioPrestacaoServicoNF
                };

                invalidAddress = false;
            }
            else
            {
                var municipio = MunicipioIBGE.Find(address.State, address.City);

                invalidAddress = municipio == null;
                
                if(invalidAddress) municipio = MunicipioIBGE.Find(null, address.City);
                
                endereco = new EnderecoRps
                {
                    Bairro = address.Neighborhood.RemoveDiacritics(),
                    Cep = address.ZipCode,
                    Numero = address.Number,
                    Uf = address.State,
                    Endereco = address.Address.RemoveDiacritics(),
                    CodigoMunicipio = municipio.CodigoMunicipio
                };
            }

            var lote = new EnviarLoteRpsEnvio
            {
                Xmlns = "http://www.ginfes.com.br/servico_enviar_lote_rps_envio_v03.xsd",
                LoteRps = new LoteRps
                {
                    NumeroLote = numeroLote.ToString(),
                    InscricaoMunicipal = InscricaoMunicipal,
                    Cnpj = CNPJ,
                    Tipos = "http://www.ginfes.com.br/tipos_v03.xsd",
                    QuantidadeRps = "1",
                    Id = $"LOTE{numeroLote.ToString()}",
                    ListaRps = new ListaRps
                    {
                        Rps = new Rps
                        {
                            InfRps = new InfRps()
                            {
                                DataEmissao = DateTime.Today,
                                NaturezaOperacao = NaturezaOperacaoNF,
                                OptanteSimplesNacional = OptanteSimplesNacionalNF,
                                IncentivadorCultural = IncentivadorCulturalNF,
                                Status = StatusNF,
                                IdentificacaoRps = new IdentificacaoRps
                                {
                                    Numero = numeroNF.ToString(),
                                    Serie = SerieNF,
                                    Tipo = TipoNF
                                },
                                Prestador = new Prestador
                                {
                                    Cnpj = CNPJ,
                                    InscricaoMunicipal = InscricaoMunicipal
                                },
                                Tomador = new Tomador
                                {
                                    RazaoSocial = member.Name.RemoveDiacritics(),
                                    Endereco = endereco,
                                    IdentificacaoTomador = new IdentificacaoTomador
                                    {
                                        CpfCnpj = new CpfCnpj
                                        {
                                            Cpf = member.Documents.First().Value
                                        }
                                    },
                                    Contato = new Contato
                                    {
                                        Email = member.Email,
                                        Telefone = isForeigner ? null : $"{member.DDDPhone}{member.Phone}"
                                    }
                                },
                                Servico = new Servico
                                {
                                    Discriminacao = DescricaoServicoNF,
                                    CodigoTributacaoMunicipio = CodigoTributacaoMunicipioNF,
                                    CodigoMunicipio = CodigoMunicipioPrestacaoServicoNF,
                                    ItemListaServico = ItemListaServicoNF,
                                    Valores = new Valores
                                    {
                                        ValorServicos = valor,
                                        ValorDeducoes = zero,
                                        ValorPis = zero,
                                        ValorCofins = zero,
                                        ValorInss = zero,
                                        ValorIr = zero,
                                        ValorCsll = zero,
                                        IssRetido = ISSRetido,
                                        ValorIss = zero,
                                        ValorIssRetido = zero,
                                        OutrasRetencoes = zero,
                                        BaseCalculo = valor,
                                        Aliquota = aliquota,
                                        ValorLiquidoNfse = valor
                                    }
                                }
                            }
                        }
                    }
                }
            };


            return lote;
        }

        public static string Serialize(EnviarLoteRpsEnvio nf)
        {
            var x = new XmlSerializer(typeof(EnviarLoteRpsEnvio));

            using (var sw = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(sw))
                {
                    x.Serialize(writer, nf);
                    return Encoding.UTF8.GetString(sw.ToArray());
                }
            }
        }
    }
}