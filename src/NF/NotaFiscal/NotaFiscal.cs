using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Dapper;
using Newtonsoft.Json;
using NF.Hotmart;

namespace NF.NotaFiscal
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public Transaction HotmartTransaction { get; set; }
        public Member HotmartMember { get; set; }
        public EnviarLoteRpsEnvio NotaFiscalRequest { get; set; }
        public string NotaFiscalRequestXML { get; set; }
        public EnviarLoteRpsResposta NotaFiscalResponse { get; set; }
        public string NotaFiscalResponseXML { get; set; }
        public bool? Valid { get; set; }
        public Validacao Validation{ get; set; }
        public string ValidationXML { get; set; }

        public bool Sent { get; set; }
        public bool? SuccessfullyTransmitted { get; set; }
        public bool IsForeigner { get; set; }
        public bool InvalidAddress { get; set; }
        public bool IsUSDolar { get; set; }
        public DateTime Created { get; set; }

        public string EnvioLoteRPS => $"{this.Numero}-env-loterps.xml";
        public string RetornoLoteRPS => $"{this.Numero}-ret-loterps.xml";
        
        public string RetornoValidacao => $"{this.Numero}-env-loterps-ret.xml";

        public bool ReadyForValidation =>
            !IsUSDolar && !InvalidAddress && HotmartTransaction.Purchase.Status == "COMPLETE" && !Valid.HasValue && !Sent;

        public bool ReadyForTransmission => Valid.HasValue && Valid.Value && !Sent;
        public bool ReadyForTransmissionResult => Valid.HasValue && Valid.Value && Sent && !SuccessfullyTransmitted.HasValue;

        public static EnviarLoteRpsEnvio GenerateLoteRPS(int numeroLote, int numeroNF, Transaction transaction,
            out bool isForeigner, out bool invalidAddress, out bool isUSDolar)
        {
            isForeigner = transaction.Buyer.Address.Country != "Brasil";
            isUSDolar = transaction.Commission.CurrencyCode == "USD";

            const string CNPJ = "34231972000109";
            const string InscricaoMunicipal = "269905";
            const string SerieNF = "NFSE";
            const string TipoNF = "1";
            const string NaturezaOperacaoNF = "1";
            const string OptanteSimplesNacionalNF = "1"; //1-Sim, 2-Não
            const string IncentivadorCulturalNF = "2"; //1-Sim, 2-Não
            const string StatusNF = "1";
            const string CodigoTributacaoMunicipioNF = "1.05 / 631940002";
            const string CodigoMunicipioPrestacaoServicoNF = "3547809";
            const string ItemListaServicoNF = "105"; //1.05
            const string ISSRetido = "2"; //1-Sim, 2-Não
            const string RegimeTributacaoEspecial = "6";
            string DescricaoServicoNF = "Contratacao do Curso de Ingles NOVO Ingles - Modulo 01 - Codigo pedido <" + transaction.Purchase.Transaction + ">";

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

                if (municipio == null) municipio = MunicipioIBGE.Find(null, address.City);
                
                invalidAddress = municipio == null;

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

            var documentoTomador = new CpfCnpj();
            var documento = transaction.Buyer.Documents.First();
            if (documento.Type == "CPF" || documento.Type == "DOCUMENT")
                documentoTomador.Cpf = documento.Value;
            if (documento.Type == "CNPJ")
                documentoTomador.Cnpj = documento.Value;

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
                                //2019-12-15T15:09:29
                                DataEmissao = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"),
                                NaturezaOperacao = NaturezaOperacaoNF,
                                OptanteSimplesNacional = OptanteSimplesNacionalNF,
                                RegimeEspecialTributacao = RegimeTributacaoEspecial,
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
                                        CpfCnpj = documentoTomador
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

        public static string Serialize<T>(T nf)
        {
            var x = new XmlSerializer(typeof(T));

            using (var sw = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(sw, Encoding.UTF8))
                {
                    x.Serialize(writer, nf);
                    return Encoding.UTF8.GetString(sw.ToArray());
                }
            }
        }

        public static T Deserialize<T>(string xml) where T : class
        {
            XmlAttributes atts = new XmlAttributes();
            // Set to true to preserve namespaces, or false to ignore them.
            atts.Xmlns = true;

            XmlAttributeOverrides xover = new XmlAttributeOverrides();
            // Add the XmlAttributes and specify the name of the 
            // element containing namespaces.
            xover.Add(typeof(T), "ns3", atts);


            var x = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(xml))
            {
                return (T) x.Deserialize(reader);
            }
        }
    }

    public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T> where T : class
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override T Parse(object value)
        {
            if (value == null) return null;
            
            return JsonConvert.DeserializeObject<T>((string)value);
        }
    }
}