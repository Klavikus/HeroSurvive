using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;
using Modules.DAL.Abstract.DataContexts;
using Modules.DAL.Implementation.Repositories;
using Modules.DAL.Tests.Internal;
using Moq;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Modules.DAL.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        private Mock<IDataContext> _dataContextMock;
        private Repository _repository;
        private Type _handledType;

        [SetUp]
        public void SetUp()
        {
            _dataContextMock = new Mock<IDataContext>();
            _handledType = typeof(TestEntity1);
            _repository = new Repository(_handledType, _dataContextMock.Object);
        }

        [Test]
        public void GetById_ExistingId_ReturnsEntity()
        {
            // Arrange
            var entities = new List<IEntity>
            {
                new TestEntity1("1"),
                new TestEntity1("2")
            };
            _dataContextMock.Setup(dataContext => dataContext.Set(_handledType)).Returns(entities);

            // Act
            var result = _repository.GetById("1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Id);
        }

        [Test]
        public void GetById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var entities = new List<IEntity>
            {
                new TestEntity1("1"),
                new TestEntity1("2")
            };
            _dataContextMock.Setup(dataContext => dataContext.Set(_handledType)).Returns(entities);

            // Act
            var result = _repository.GetById("3");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetAll_ReturnsAllEntities()
        {
            // Arrange
            var entities = new List<IEntity>
            {
                new TestEntity1("1"),
                new TestEntity1("2")
            };
            _dataContextMock.Setup(dataContext => dataContext.Set(_handledType)).Returns(entities);

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].Id);
            Assert.AreEqual("2", result[1].Id);
        }

        [Test]
        public void Add_AddsEntityToDataContext()
        {
            // Arrange
            var entities = new List<IEntity>();
            _dataContextMock.Setup(dataContext => dataContext.Set(_handledType)).Returns(entities);
            var entity = new TestEntity1("1");

            // Act
            var result = _repository.Add(entity);

            // Assert
            Assert.AreEqual(entity, result);
            Assert.AreEqual(1, entities.Count);
            Assert.Contains(entity, entities);
        }

        [Test]
        public void Delete_RemovesEntityFromDataContext()
        {
            // Arrange
            var entity = new TestEntity1("1");
            var entities = new List<IEntity> {entity};
            _dataContextMock.Setup(dataContext => dataContext.Set(_handledType)).Returns(entities);

            // Act
            _repository.Delete(entity);

            // Assert
            Assert.AreEqual(0, entities.Count);
        }

        [Test]
        public void Clear_ClearsAllEntitiesFromDataContext()
        {
            // Arrange
            var entities = new List<IEntity>
            {
                new TestEntity1("1"),
                new TestEntity1("2")
            };
            _dataContextMock.Setup(dataContext => dataContext.Set(_handledType)).Returns(entities);

            // Act
            _repository.Clear();

            // Assert
            Assert.AreEqual(0, entities.Count);
        }

        [UnityTest]
        public IEnumerator Save_CallsSaveOnDataContext()
        {
            // Arrange
            _dataContextMock.Setup(dataContext => dataContext.Save()).Returns(UniTask.CompletedTask);

            // Act
            yield return _repository.Save().ToCoroutine();

            // Assert
            _dataContextMock.Verify(dataContext => dataContext.Save(), Times.Once);
        }

        [UnityTest]
        public IEnumerator Load_CallsLoadOnDataContext()
        {
            // Arrange
            _dataContextMock.Setup(dataContext => dataContext.Load()).Returns(UniTask.CompletedTask);

            // Act
            yield return _repository.Load().ToCoroutine();

            // Assert
            _dataContextMock.Verify(dataContext => dataContext.Load(), Times.Once);
        }

        [Test]
        public void Delete_WithPredicate_RemovesMatchingEntities()
        {
            // Arrange
            var entities = new List<IEntity>
            {
                new TestEntity1("1") {Name = "Entity 1"},
                new TestEntity1("2") {Name = "Entity 2"},
                new TestEntity1("3") {Name = "Entity 3"},
            };
            _dataContextMock.Setup(dataContext => dataContext.Set(_handledType)).Returns(entities);

            // Act
            _repository.Delete(entity => entity.Id == "2");

            // Assert
            Assert.AreEqual(2, entities.Count);
            Assert.IsFalse(entities.Any(entity => entity.Id == "2"));
        }
    }
}