using System;
using System.Collections.Generic;
using System.Linq;
using Modules.DAL.Abstract.Data;
using Modules.DAL.Implementation.Data;
using Modules.DAL.Tests.Internal;
using NUnit.Framework;

namespace Modules.DAL.Tests
{
    public class GameDataTests
    {
        [Test]
        public void Constructor_WithValidDataTypes_InitializesProgressByType()
        {
            // Arrange
            var dataTypes = new[] {typeof(TestEntity1), typeof(TestEntity2)};

            // Act
            var gameData = new GameData(dataTypes);

            // Assert
            Assert.AreEqual(2, gameData.ContainedTypes.Count());
            Assert.IsTrue(gameData.ContainedTypes.Contains(typeof(TestEntity1)));
            Assert.IsTrue(gameData.ContainedTypes.Contains(typeof(TestEntity2)));
        }

        [Test]
        public void Constructor_WithNullDataTypes_ThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<Type> dataTypes = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new GameData(dataTypes));
        }

        [Test]
        public void Constructor_WithEmptyDataTypes_ThrowsArgumentException()
        {
            // Arrange
            var dataTypes = Array.Empty<Type>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new GameData(dataTypes));
        }

        [Test]
        public void Constructor_WithInvalidDataType_ThrowsInvalidCastException()
        {
            // Arrange
            var dataTypes = new[] {typeof(object)};

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => new GameData(dataTypes));
        }

        [Test]
        public void Set_ReturnsListOfEntitiesForGivenType()
        {
            // Arrange
            var dataTypes = new[] {typeof(TestEntity1), typeof(TestEntity2)};
            var gameData = new GameData(dataTypes);

            // Act
            var entities = gameData.Set(typeof(TestEntity1));

            // Assert
            Assert.IsNotNull(entities);
            Assert.IsInstanceOf<List<IEntity>>(entities);
        }

        [Test]
        public void Set_WithInvalidType_ThrowsInvalidArgumentException()
        {
            // Arrange
            var dataTypes = new[] {typeof(TestEntity1)};
            var gameData = new GameData(dataTypes);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => gameData.Set(typeof(TestEntity2)));
        }

        [Test]
        public void InjectWithReplace_ReplacesEntitiesOfGivenType()
        {
            // Arrange
            var dataTypes = new[] {typeof(TestEntity1)};
            var gameData = new GameData(dataTypes);
            var entitiesBefore = new List<TestEntity1> {new("Before_1"), new("Before_2")};
            var newEntities = new List<TestEntity1> {new("1"), new("2")};

            // Act
            gameData.Set(typeof(TestEntity1)).AddRange(entitiesBefore);
            var injectedEntities = gameData.InjectWithReplace(newEntities);

            // Assert
            Assert.AreEqual(2, injectedEntities.Count);
            Assert.AreEqual(2, gameData.Set(typeof(TestEntity1)).Count);
        }

        [Test]
        public void InjectWithReplace_WithNullList_ThrowsNullReferenceException()
        {
            // Arrange
            var dataTypes = new[] {typeof(TestEntity1)};
            var gameData = new GameData(dataTypes);
            List<TestEntity1> newEntities = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => gameData.InjectWithReplace(newEntities));
        }

        [Test]
        public void CopyFrom_CopiesEntitiesFromDeserializedData()
        {
            // Arrange
            var dataTypes = new[] {typeof(TestEntity1), typeof(TestEntity2)};
            var gameData = new GameData(dataTypes);
            var deserializedData = new Dictionary<Type, List<IEntity>>
            {
                {typeof(TestEntity1), new List<IEntity> {new TestEntity1(nameof(TestEntity1))}},
                {typeof(TestEntity2), new List<IEntity> {new TestEntity2(nameof(TestEntity2))}}
            };

            // Act
            gameData.CopyFrom(deserializedData);

            // Assert
            Assert.AreEqual(1, gameData.Set(typeof(TestEntity1)).Count);
            Assert.AreEqual(1, gameData.Set(typeof(TestEntity2)).Count);
        }

        [Test]
        public void CopyFrom_WithNullDeserializedData_ThrowsNullReferenceException()
        {
            // Arrange
            var dataTypes = new[] {typeof(TestEntity1), typeof(TestEntity2)};
            var gameData = new GameData(dataTypes);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => gameData.CopyFrom(null));
        }

        [Test]
        public void Clear_ClearsAllEntities()
        {
            // Arrange
            var dataTypes = new[] {typeof(TestEntity1), typeof(TestEntity2)};
            var gameData = new GameData(dataTypes);
            gameData.Set(typeof(TestEntity1)).Add(new TestEntity1(nameof(TestEntity1)));
            gameData.Set(typeof(TestEntity2)).Add(new TestEntity2(nameof(TestEntity2)));

            // Act
            gameData.Clear();

            // Assert
            Assert.AreEqual(0, gameData.Set(typeof(TestEntity1)).Count);
            Assert.AreEqual(0, gameData.Set(typeof(TestEntity2)).Count);
        }
    }
}