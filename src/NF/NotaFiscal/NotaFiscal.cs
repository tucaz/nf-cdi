using System;
using System.Data;
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
        public bool Sent { get; set; }
        public bool IsForeigner { get; set; }
        public bool InvalidAddress { get; set; }
        public DateTime Created { get; set; }
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