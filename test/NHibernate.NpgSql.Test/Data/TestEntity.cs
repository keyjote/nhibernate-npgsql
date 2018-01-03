﻿using System;

namespace Beginor.NHibernate.NpgSql.Test.Data {

    public class TestEntity {

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string[] Tags { get; set; }

        public virtual string JsonField { get; set; }

        public virtual string JsonbField { get; set; }

        public virtual DateTime UpdateTime { get; set; }

        public virtual EntityAttributes Attributes { get; set; }

        public virtual EntityAttributes Battributes { get; set; }
    }

    public class EntityAttributes {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsDead { get; set; }
    }

    public class EntityAttributesNhJson : JsonObjectType<EntityAttributes> { }

    public class EntityBattributesNhJson : JsonObjectType<EntityAttributes> { }
}
