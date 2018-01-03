using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NpgsqlTypes;

namespace Beginor.NHibernate.NpgSql {
    public class JsonObjectType<T> : IUserType where T : class {
        public virtual SqlType[] SqlTypes => new SqlType[] {
            new NpgSqlType(DbType.String, NpgsqlDbType.Json)
        };

        public Type ReturnedType => typeof(T);

        public bool IsMutable => true;

        public object Assemble(object cached, object owner) {
            return cached;
        }

        public object DeepCopy(object value) {
            if (value == null) {
                return null;
            }
            var json = Serialize((T)value);
            var obj = Deserialize(json);
            return obj;
        }

        public object Disassemble(object value) {
            return value;
        }

        public new bool Equals(object x, object y) {
            var left = x as T;
            var right = y as T;

            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            return Serialize(left).Equals(Serialize(right));
        }

        public int GetHashCode(object x) {
            if (x == null) {
                return 0;
            }
            return x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner) {
            var returnValue = NHibernateUtil.String.NullSafeGet(rs, names[0], session);

            var json = returnValue == null 
                        ? "{}" 
                        : returnValue.ToString();
            return Deserialize(json);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session) {
            var column = value as T;
            if (value == null) {
                NHibernateUtil.String.NullSafeSet(cmd, "{}", index, session);
                return;
            }
            value = Serialize(column);
            NHibernateUtil.String.NullSafeSet(cmd, value, index, session);
        }

        public object Replace(object original, object target, object owner) {
            return original;
        }

        public T Deserialize(string jsonString) {
            if (string.IsNullOrWhiteSpace(jsonString))
                return CreateObject(typeof(T));

            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public string Serialize(T obj) {
            if (obj == null)
                return "{}";
            return JsonConvert.SerializeObject(obj);
        }

        private static T CreateObject(Type jsonType) {
            object result;
            try {
                result = Activator.CreateInstance(jsonType, true);
            } catch (Exception) {
                result = FormatterServices.GetUninitializedObject(jsonType);
            }

            return (T)result;
        }
    }
}
