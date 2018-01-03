using System;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using NUnit.Framework;
using Beginor.NHibernate.NpgSql.Test.Data;

namespace Beginor.NHibernate.NpgSql.Test {

    [TestFixture]
    public class NpgSqlTest {

        private ISessionFactory factory;

        [OneTimeSetUp]
        public void InitializeDatabase() {
            var configuration = new Configuration();
            var configFile = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "hibernate.config"
            );
            configuration.Configure(configFile);
            factory = configuration.BuildSessionFactory();
        }

        [Test]
        public void CanDoCrud() {
            var random = new Random(100);
            var attEntity = new EntityAttributes() {Age = random.Next(75), IsDead = false, Name = "Yonny"};
            using (var session = factory.OpenSession()) {
                var entity = new TestEntity {
                    Name = "Test " + random.Next(),
                    Tags = new string[] { "hello", "world" },
                    JsonField = "{ \"val\": 1 }",
                    JsonbField = "{ \"val\": 1 }",
                    UpdateTime = DateTime.Now,
                    Int16Arr = new short[] { 1, 2, 3 },
                    Int32Arr = new int[] { 1, 2, 3 },
                    Int64Arr = new long[] { 1L, 2L, 3L },
                    FloatArr = new float[] { 1.1F, 2.2F, 3.3F },
                    DoubleArr = new double[] { 1.1, 2.2, 3.3 },
                    BooleanArr = new bool[] { true, false },
                    Attributes = attEntity,
                    Battributes = attEntity
                };

                session.Save(entity);
                session.Flush();
                session.Clear();

                Assert.Greater(entity.Id, 0);

                Console.WriteLine($"entity id: {entity.Id}");
                Console.WriteLine($"entity Attribute: {entity.Attributes.Name} ({entity.Attributes.Age})");
                Console.WriteLine($"entity Battribute: {entity.Battributes.Name} ({entity.Battributes.Age})");

                var query = session.Query<TestEntity>();
                var entities = query.ToList();
                Assert.NotNull(entities);

                foreach (var e in entities) {
                    session.Delete(e);
                }

                session.Flush();

            }
        }

        [Test]
        public void DoAdd() {
            var random = new Random(100);
            var attEntity = new EntityAttributes() {Age = random.Next(75), IsDead = false, Name = "Yonny"};
            using (var session = factory.OpenSession()) {
                var entity = new TestEntity {
                    Name = "Test " + random.Next(100),
                    Tags = new string[] { "hello", "world" },
                    JsonField = "{ \"val\": 1 }",
                    JsonbField = "{ \"val\": 1 }",
                    UpdateTime = DateTime.Now,
                    Int16Arr = new short[] { 1, 2, 3 },
                    Int32Arr = new int[] { 1, 2, 3 },
                    Int64Arr = new long[] { 1L, 2L, 3L },
                    FloatArr = new float[] { 1.1F, 2.2F, 3.3F },
                    DoubleArr = new double[] { 1.1, 2.2, 3.3 },
                    BooleanArr = new bool[] { true, false },
                    Attributes = attEntity,
                    Battributes = attEntity
                };

                session.Save(entity);
                session.Flush();
                session.Clear();

                Assert.Greater(entity.Id, 0);

                Console.WriteLine($"entity id: {entity.Id}");
                Console.WriteLine($"entity Attribute: {entity.Attributes.Name} ({entity.Attributes.Age})");
                Console.WriteLine($"entity Battribute: {entity.Battributes.Name} ({entity.Battributes.Age})");
            }
        }

        [Test]
        public void Read() {
            using (var session = factory.OpenSession()) {
                var query = session.Query<TestEntity>();
                var entities = query.ToList();

                Assert.NotNull(entities);
                Assert.That(entities.Count, Is.GreaterThan(0));
                foreach (var testEntity in entities) {
                    Assert.That(testEntity.Id, Is.GreaterThan(0));
                    Assert.That(testEntity.JsonField, Is.Not.Empty);
                    Assert.That(testEntity.JsonbField, Is.Not.Empty);
                    Assert.That(testEntity.Attributes.Age, Is.GreaterThan(0));
                    Assert.That(testEntity.Attributes.IsDead, Is.False);
                    Assert.That(testEntity.Attributes.Name, Is.Not.Empty);
                    Assert.That(testEntity.Attributes.Age, Is.EqualTo(testEntity.Battributes.Age));
                    Assert.That(testEntity.Attributes.IsDead, Is.EqualTo(testEntity.Battributes.IsDead));
                    Assert.That(testEntity.Attributes.Name, Is.EqualTo(testEntity.Battributes.Name));
                }
            }
        }

    }

}
