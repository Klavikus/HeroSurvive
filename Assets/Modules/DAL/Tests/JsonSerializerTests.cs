using System;
using System.Collections.Generic;
using System.Reflection;
using Modules.DAL.Implementation.Serialization;
using Modules.DAL.Tests.Internal;
using NUnit.Framework;

namespace Modules.DAL.Tests
{
    [TestFixture]
    public class JsonSerializerTests
    {
        private JsonSerializer _serializer;
        private IList<Type> _knownTypes;

        [SetUp]
        public void SetUp()
        {
            _knownTypes = new List<Type> {typeof(TestEntity1)};
            _serializer = new JsonSerializer(_knownTypes);
        }

        [Test]
        public void Serialize_WithValidObject_ReturnsSerializedString()
        {
            // Arrange
            var testEntity = new TestEntity1("1") {Name = "Test"};

            // Act
            string serializedString = _serializer.Serialize(testEntity);

            // Assert
            Assert.IsNotNull(serializedString);
            Assert.IsNotEmpty(serializedString);
        }

        [Test]
        public void Serialize_WithNullObject_ThrowsNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _serializer.Serialize(null));
        }

        [Test]
        public void Serialize_WithInvalidObject_ThrowsArgumentException()
        {
            // Arrange
            var testEntity = "new TestEntity {Id = 1, Name = Test}";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _serializer.Serialize(testEntity));
        }

        [Test]
        public void Deserialize_WithValidString_ReturnsDeserializedObject()
        {
            // Arrange
            var testEntity = new TestEntity1("1") {Name = "Test"};
            string serializedString = _serializer.Serialize(testEntity);

            // Act
            TestEntity1 deserializedEntity1 = _serializer.Deserialize<TestEntity1>(serializedString);

            // Assert
            Assert.IsNotNull(deserializedEntity1);
            Assert.AreEqual(testEntity.Id, deserializedEntity1.Id);
            Assert.AreEqual(testEntity.Name, deserializedEntity1.Name);
        }

        [Test]
        public void Deserialize_WithEmptyString_ReturnsDefaultValue()
        {
            // Arrange
            string emptyString = string.Empty;

            // Act
            TestEntity1 deserializedEntity1 = _serializer.Deserialize<TestEntity1>(emptyString);

            // Assert
            Assert.IsNull(deserializedEntity1);
        }

        [Test]
        public void Deserialize_WithInvalidString_ReturnsDefaultValue()
        {
            // Arrange
            string invalidString = "invalid json";

            // Act
            TestEntity1 deserializedEntity1 = _serializer.Deserialize<TestEntity1>(invalidString);

            // Assert
            Assert.IsNull(deserializedEntity1);
        }

        [Test]
        public void Serialize_WithValidObject_And_DeserializeThis_ReturnValidObject()
        {
            // Arrange
            var testEntity = new TestEntity1("1") {Name = "Test"};

            // Act
            string serializedString = _serializer.Serialize(testEntity);
            var deserializedObject = _serializer.Deserialize<TestEntity1>(serializedString);

            // Assert
            Assert.AreEqual(testEntity, deserializedObject);
        }

        [Test]
        public void Serialize_WithInvalidObject_And_DeserializeThis_ReturnDefaultValueFromConstruct()
        {
            // Arrange
            var testEntity = new TestEntity1("1") {Name = "Test"};

            // Act
            string serializedString = _serializer.Serialize(testEntity);
            var deserializedObject = _serializer.Deserialize<TestInvalidEntity>(serializedString);

            // Assert
            Assert.AreEqual(new TestInvalidEntity(), deserializedObject);
        }

        [Test]
        public void Constructor_WithKnownTypes_SetsKnownTypes()
        {
            // Arrange
            var knownTypes = new List<Type> {typeof(TestEntity1)};

            // Act
            var serializer = new JsonSerializer(knownTypes);

            // Assert
            Assert.IsNotNull(serializer);
            Assert.AreEqual(knownTypes,
                serializer.GetType().GetField("_knownTypes",
                        BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.GetValue(serializer) as IList<Type>);
        }
    }
}