# Beginor.NHibernate.NpgSql

NHibernate Extension for supporting of PostgreSQL's `json`, `jsonb` and
`string[]` field.  
The `json` is for mapping it to a string as well as to defined class.


## Usage

### 1. Set `connection.driver_class` to `Beginor.NHibernate.NpgSql.NpgSqlDriver,Beginor.NHibernate.NpgSql` as flow:

```xml
<hibernate-configuration xmlns="urn:nhibernate-configuration-2.2">
  <session-factory>
    <property name="connection.driver_class">Beginor.NHibernate.NpgSql.NpgSqlDriver,Beginor.NHibernate.NpgSql</property>
    <property name="dialect">NHibernate.Dialect.PostgreSQL82Dialect</property>
    <!-- other properties -->
  </session-factory>
</hibernate-configuration>
```

### 2. Reference the types needed to mapping file

`TestEntity.hbm.xml`

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<hibernate-mapping
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:xsd="http://www.w3.org/2001/XMLSchema"
    xmlns="urn:nhibernate-mapping-2.2"
    namespace="Beginor.NHibernate.NpgSql.Test.Data"
    assembly="Beginor.NHibernate.NpgSql.Test"
    >

    <typedef name="npgJson" class="Beginor.NHibernate.NpgSql.JsonType, Beginor.NHibernate.NpgSql" />
    <typedef name="npgJsonb" class="Beginor.NHibernate.NpgSql.JsonbType, Beginor.NHibernate.NpgSql" />
    <typedef name="npgStringArray" class="Beginor.NHibernate.NpgSql.StringArrayType, Beginor.NHibernate.NpgSql" />
    <typedef name="npgStringArray" class="Beginor.NHibernate.NpgSql.StringArrayType, Beginor.NHibernate.NpgSql" />

</hibernate-mapping>
```

### 3. Use theme in mapping file

```xml
<class name="TestEntity" table="test_table" schema="public">
    <id name="Id" column="id" type="int">
        <generator class="sequence">
            <param name="sequence">public.test_table_id_seq</param>
        </generator>
    </id>
    <property name="Name" column="name" type="string" length="32" />
    <property name="Tags" column="tags" type="npgStringArray" length="32"></property>
    <property name="JsonField" column="json_field" type="npgJson" />
    <property name="JsonbField" column="jsonb_field" type="npgJson" />
    <property name="UpdateTime" column="update_time" type="datetime" />
</class>
```

`TestEntity` is something like:

```cs
using System;

namespace Beginor.NHibernate.NpgSql.Test.Data {

    public class TestEntity {

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string[] Tags { get; set; }

        public virtual string JsonField { get; set; }

        public virtual string JsonbField { get; set; }

        public virtual DateTime UpdateTime { get; set; }
    }

}
```

### 4. Done !

## Usage for mapping an object to a json column

### 1. Same as above

### 2. Create class which inherits from JsonObjectType<T>

You need to have an existing class which describes the json in the database column. In this case I will cal it EntityAttributes
```cs
    public class EntityAttributesNhJson : JsonObjectType<EntityAttributes> { }
```

### 3. Reference the types needed to mapping file and use theme in mapping file

`TestEntity.hbm.xml`

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<hibernate-mapping
...
    <typedef name="attJson" class="Beginor.NHibernate.NpgSql.Test.Data.EntityAttributesNhJson, Beginor.NHibernate.NpgSql.Test" />
...

    <class name="TestEntity" table="test_table" schema="public">
...
        <property name="Attributes" column="att_att" type="attJson" />
    </class>
</hibernate-mapping
```

Update `TestEntity` to something like:

```cs
namespace Beginor.NHibernate.NpgSql.Test.Data {

    public class TestEntity {

...

        public virtual EntityAttributes Attributes { get; set; }

    }

}
```

### 4. Done !
