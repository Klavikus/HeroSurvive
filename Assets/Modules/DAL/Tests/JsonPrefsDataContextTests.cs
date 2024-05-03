using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;
using Modules.DAL.Abstract.DataContexts;
using Modules.DAL.Implementation.DataContexts;
using Modules.DAL.Implementation.Serialization;
using Modules.DAL.Tests.Internal;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Modules.DAL.Tests
{
    [TestFixture]
    public class JsonPrefsDataContextTests
    {
        private const string TestKey = "TestKey";
        private Mock<IData> _dataMock;
        private JsonPrefsDataContext _dataContext;

        [SetUp]
        public void SetUp()
        {
            _dataMock = new Mock<IData>();
            _dataMock.Setup(d => d.ContainedTypes).Returns(new[] {typeof(TestEntity1)});
            _dataContext = new JsonPrefsDataContext(_dataMock.Object, TestKey);
        }

        [TearDown]
        public void TearDown()
        {
            PlayerPrefs.DeleteKey(TestKey);
        }

        [UnityTest]
        public IEnumerator Load_WithExistingData_LoadsDataFromPlayerPrefs()
        {
            // Arrange
            var expectedEntities = new List<IEntity> {new TestEntity1("1")};
            var gameDataDto = new GameDataDto(new Dictionary<Type, List<IEntity>>
                {{typeof(TestEntity1), expectedEntities}});

            var jsonSerializer = new JsonSerializer(new[] {typeof(TestEntity1), typeof(GameDataDto)});
            string json = jsonSerializer.Serialize(gameDataDto);
            PlayerPrefs.SetString(TestKey, json);

            // Act
            yield return _dataContext.Load().ToCoroutine();

            // Assert
            _dataMock.Verify(d => d.CopyFrom(It.Is<Dictionary<Type, List<IEntity>>>(data =>
                    data.ContainsKey(typeof(TestEntity1)) &&
                    data[typeof(TestEntity1)].SequenceEqual(expectedEntities))),
                Times.Once);
        }

        [UnityTest]
        public IEnumerator Load_WithNonExistingData_DoesNotCallCopyFrom()
        {
            // Act
            yield return _dataContext.Load().ToCoroutine();

            // Assert
            _dataMock.Verify(data => data.CopyFrom(It.IsAny<Dictionary<Type, List<IEntity>>>()), Times.Never);
        }

        [UnityTest]
        public IEnumerator Save_SavesDataToPlayerPrefs()
        {
            // Arrange
            var entities = new List<IEntity> {new TestEntity1("1")};
            _dataMock.Setup(data => data.Set(typeof(TestEntity1))).Returns(entities);

            // Act
            yield return _dataContext.Save().ToCoroutine();

            // Assert
            string json = PlayerPrefs.GetString(TestKey);
            Assert.IsNotEmpty(json);
            var jsonSerializer = new JsonSerializer(new[] {typeof(TestEntity1)});
            var gameDataDto = jsonSerializer.Deserialize<GameDataDto>(json);
            Assert.IsNotNull(gameDataDto);
            Assert.IsTrue(gameDataDto.Data.ContainsKey(typeof(TestEntity1)));
            Assert.AreEqual(1, gameDataDto.Data[typeof(TestEntity1)].Count);
        }

        [Test]
        public void Set_ReturnsListOfEntitiesFromData()
        {
            // Arrange
            var expectedEntities = new List<IEntity>();
            _dataMock.Setup(data => data.Set(typeof(TestEntity1))).Returns(expectedEntities);

            // Act
            var entities = _dataContext.Set(typeof(TestEntity1));

            // Assert
            Assert.AreSame(expectedEntities, entities);
        }

        [Test]
        public void Clear_CallsClearOnData()
        {
            // Act
            _dataContext.Clear();

            // Assert
            _dataMock.Verify(data => data.Clear(), Times.Once);
        }

        [Test]
        public void CopyFrom_WithValidDataContext_CopiesDataFromDataContext()
        {
            // Arrange
            var containedTypes = new[] {typeof(TestEntity1)};
            _dataMock.Setup(data => data.ContainedTypes).Returns(containedTypes);
            var sourceDataContext = new Mock<IDataContext>();
            sourceDataContext.Setup(dataContext => dataContext.ContainedTypes).Returns(containedTypes);
            var expectedEntities = new List<IEntity> {new TestEntity1("1")};
            sourceDataContext.Setup(dataContext => dataContext.Set(typeof(TestEntity1))).Returns(expectedEntities);

            // Act
            _dataContext.CopyFrom(sourceDataContext.Object);

            // Assert
            _dataMock.Verify(data => data.InjectWithReplace(expectedEntities), Times.Once);
        }

        [Test]
        public void CopyFrom_WithNullDataContext_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _dataContext.CopyFrom(null));
        }

        [Test]
        public void CopyFrom_WithIncompatibleDataContext_ThrowsArgumentException()
        {
            // Arrange
            var sourceDataContext = new Mock<IDataContext>();
            sourceDataContext.Setup(dataContext => dataContext.ContainedTypes).Returns(new[] {typeof(object)});

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _dataContext.CopyFrom(sourceDataContext.Object));
        }
    }
}