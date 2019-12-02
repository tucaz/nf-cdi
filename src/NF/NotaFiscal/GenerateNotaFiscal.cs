using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dapper;
using Newtonsoft.Json;
using NF.DataAccess;
using NF.Hotmart;

namespace NF.NotaFiscal
{
    public static class NotasFiscais
    {
        public static EnviarLoteRpsEnvio GenerateLoteRPS(int numeroLote, int numeroNF, Transaction transaction,
            out bool isForeigner, out bool invalidAddress)
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

                if (invalidAddress) municipio = MunicipioIBGE.Find(null, address.City);

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

        public static async Task<List<NotaFiscal>> LoadAllNotasFiscais()
        {
            const string sql =
                "SELECT id as Id, member as HotmartMember, purchase as HotmartTransaction , nf_number as Numero, nf_request as NotaFiscalRequest, nf_request_xml as NotaFiscalRequestXML, nf_response as NotaFiscalResponse, nf_response_xml as NotaFiscaResponseXML, sent as Sent, is_foreigner as IsForeigner, invalid_address as InvalidAddress, created as Created FROM nota_fiscal";
            return await SqLiteBaseRepository.DbConnection().Query<NotaFiscal>(sql);
        }

        public static async Task GenerateNotasFiscais()
        {
            var transactions = await GetTransactionsFromHotmart.GetAlltransactions();
            var members = await Api.GetAllMembers();
            var nfs = new List<NotaFiscal>();

            var numeroNF = 5;
            foreach (var transaction in transactions)
            {
                var nf = CreateNotaFiscal(transaction, members, numeroNF);
                nfs.Add(nf);

                File.WriteAllText($"C:\\temp\\nfs\\{numeroNF}-env-loterps.xml", nf.NotaFiscalRequestXML);

                numeroNF++;
            }

            await SqLiteBaseRepository.DbConnection().Execute(conn =>
            {
                foreach (var nf in nfs)
                {
                    InsertNotaFiscal(nf, conn);
                }
            }, true);
        }

        private static void InsertNotaFiscal(NotaFiscal nf, SQLiteConnection conn)
        {
            conn.Execute(
                @"INSERT INTO nota_fiscal (member, purchase, nf_number, nf_request, nf_request_xml, sent, is_foreigner, invalid_address, created) 
VALUES (json(@member), json(@purchase), @nf_number, @nf_request, @nf_request_xml, @sent, @is_foreigner, @invalid_address, @created)",
                new
                {
                    member = JsonConvert.SerializeObject(nf.HotmartMember),
                    purchase = JsonConvert.SerializeObject(nf.HotmartTransaction),
                    nf_number = nf.Numero,
                    nf_request = JsonConvert.SerializeObject(nf.NotaFiscalRequest),
                    nf_request_xml = nf.NotaFiscalRequestXML,
                    sent = 0,
                    is_foreigner = nf.IsForeigner,
                    invalid_address = nf.InvalidAddress,
                    created = DateTime.UtcNow.ToString("o")
                });
        }

        private static NotaFiscal CreateNotaFiscal(Transaction transaction, List<Member> members, int numeroNotaFiscal)
        {
            var member = members.First(m => m.Id == transaction.Buyer.Id);
            var loteRps = GenerateLoteRPS(numeroNotaFiscal, numeroNotaFiscal, transaction,
                out bool isForeigner, out bool invalidAddress);
            var loteRpsXML = Serialize(loteRps);

            var nf = new NotaFiscal
            {
                Numero = numeroNotaFiscal,
                Created = DateTime.Now,
                HotmartTransaction = transaction,
                HotmartMember = member,
                NotaFiscalRequest = loteRps,
                NotaFiscalRequestXML = loteRpsXML,
                IsForeigner = isForeigner,
                InvalidAddress = invalidAddress
            };

            return nf;
        }
    }
}

public static class SQLiteHelper
{
    public static async Task Execute(this SQLiteConnection connection, Action<SQLiteConnection> action,
        bool enableExtensions = false)
    {
        using (connection)
        {
            await connection.OpenAsync();
            try
            {
                if (enableExtensions)
                {
                    connection.EnableExtensions(true);
                    connection.LoadExtension(@"SQLite.Interop.dll", "sqlite3_json_init");
                }

                using (var transaction = connection.BeginTransaction())
                {
                    action(connection);
                    transaction.Commit();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static async Task<List<T>> Query<T>(this SQLiteConnection connection, string sql,
        bool enableExtensions = false)
    {
        using (connection)
        {
            await connection.OpenAsync();
            try
            {
                if (enableExtensions)
                {
                    connection.EnableExtensions(true);
                    connection.LoadExtension(@"SQLite.Interop.dll", "sqlite3_json_init");
                }

                using (var transaction = connection.BeginTransaction())
                {
                    var results = await connection.QueryAsync<T>(sql);
                    transaction.Commit();
                    return results.ToList();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}