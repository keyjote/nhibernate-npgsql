using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace Beginor.NHibernate.NpgSql {

    public class JsonbObjectType<T> : JsonObjectType<T> where T : class {

        public override SqlType[] SqlTypes => new SqlType[] {
            new NpgSqlType(DbType.Binary, NpgsqlDbType.Jsonb)
        };

    }

}
