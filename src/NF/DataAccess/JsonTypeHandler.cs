using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace NF.DataAccess
{
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