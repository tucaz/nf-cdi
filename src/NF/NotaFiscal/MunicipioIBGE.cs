using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;

namespace NF.NotaFiscal
{
    public class MunicipioIBGE
    {
        private static Dictionary<string, string> _estados = new Dictionary<string, string>();

        static MunicipioIBGE()
        {
            _estados.Add("Acre", "AC");
            _estados.Add("Alagoas", "AL");
            _estados.Add("Amapá", "AP");
            _estados.Add("Amazonas", "AM");
            _estados.Add("Bahia", "BA");
            _estados.Add("Ceará", "CE");
            _estados.Add("Distrito Federal", "DF");
            _estados.Add("Espírito Santo", "ES");
            _estados.Add("Goiás", "GO");
            _estados.Add("Maranhão", "MA");
            _estados.Add("Mato Grosso", "MT");
            _estados.Add("Mato Grosso do Sul", "MS");
            _estados.Add("Minas Gerais", "MG");
            _estados.Add("Pará", "PA");
            _estados.Add("Paraíba", "PB");
            _estados.Add("Paraná", "PR");
            _estados.Add("Pernambuco", "PE");
            _estados.Add("Piauí", "PI");
            _estados.Add("Rio de Janeiro", "RJ");
            _estados.Add("Rio Grande do Norte", "RN");
            _estados.Add("Rio Grande do Sul", "RS");
            _estados.Add("Rondônia", "RO");
            _estados.Add("Roraima", "RR");
            _estados.Add("Santa Catarina", "SC");
            _estados.Add("São Paulo", "SP");
            _estados.Add("Sergipe", "SE");
            _estados.Add("Tocantins", "TO");
        }

        private static List<MunicipioIBGE> LoadAll()
        {
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("NF.NotaFiscal.municipios_ibge.csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var csv = new CsvReader(reader))
                    {
                        csv.Configuration.RegisterClassMap<MunicipioIBGEMap>();
                        var municipios = csv.GetRecords<MunicipioIBGE>().ToList();

                        foreach (var municipio in municipios)
                        {
                            municipio.SiglaUF = _estados[municipio.NomeUF];
                        }

                        return municipios;
                    }
                }
            }
        }

        public static MunicipioIBGE Find(string uf, string cidade)
        {
            cidade = cidade.Trim();

            if (uf == null)
                return All.Find(ibge => string.Equals(ibge.NomeMunicipio.RemoveDiacritics(), cidade.RemoveDiacritics(),
                    StringComparison.CurrentCultureIgnoreCase));
            
            return All.Find(ibge =>
                string.Equals(ibge.NomeMunicipio.RemoveDiacritics(), cidade.RemoveDiacritics(),
                    StringComparison.CurrentCultureIgnoreCase) && ibge.SiglaUF == uf);
        }

        private static List<MunicipioIBGE> _all;
        public static List<MunicipioIBGE> All => _all ?? (_all = LoadAll());

        public string CodigoUF { get; set; }
        public string SiglaUF { get; set; }
        public string NomeUF { get; set; }
        public string MesoregiaoGeografica { get; set; }
        public string NomeMesoregiaoGeografica { get; set; }
        public string MicroregiaoGeografica { get; set; }
        public string NomeMicroregiaoGeografica { get; set; }
        public string Municipio { get; set; }
        public string CodigoMunicipio { get; set; }
        public string NomeMunicipio { get; set; }

        private class MunicipioIBGEMap : ClassMap<MunicipioIBGE>
        {
            public MunicipioIBGEMap()
            {
                Map(x => x.CodigoUF).Name("UF");
                Map(x => x.NomeUF).Name("Nome_UF");
                Map(x => x.MesoregiaoGeografica).Name("Mesorregião Geográfica");
                Map(x => x.NomeMesoregiaoGeografica).Name("Nome_Mesorregião");
                Map(x => x.MicroregiaoGeografica).Name("Microrregião Geográfica");
                Map(x => x.NomeMicroregiaoGeografica).Name("Nome_Microrregião");
                Map(x => x.Municipio).Name("Município");
                Map(x => x.CodigoMunicipio).Name("Código Município Completo");
                Map(x => x.NomeMunicipio).Name("Nome_Município");
            }
        }
    }
}