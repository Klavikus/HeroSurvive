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
    public class CompositeRepositoryTests
    {
        private Mock<IDataContext> _dataContextMock;
        private Mock<IDataContext> _secondaryDataContextMock;
        private CompositeRepository _compositeRepository;
        private List<Type> _repoTypes;
        private Dictionary<Type, List<IEntity>> _entitiesByType;
        private Dictionary<Type, List<IEntity>> _secondaryEntitiesByType;

        [SetUp]
        public void SetUp()
        {
            _dataContextMock = new Mock<IDataContext>();
            _secondaryDataContextMock = new Mock<IDataContext>();
            _repoTypes = new List<Type> {typeof(TestEntity1), typeof(TestEntity2)};
            _compositeRepository = new CompositeRepository(_dataContextMock.Object, _repoTypes);

            _entitiesByType = new Dictionary<Type, List<IEntity>>();
            _secondaryEntitiesByType = new Dictionary<Type, List<IEntity>>();

            _dataContextMock.Setup(x => x.Set(It.IsAny<Type>()))
                .Returns((Type type) =>
                {
                    if (!_entitiesByType.TryGetValue(type, out var entities))
                    {
                        entities = new List<IEntity>();
                        _entitiesByType[type] = entities;
                    }

                    return entities;
                });

            _secondaryDataContextMock.Setup(x => x.Set(It.IsAny<Type>()))
                .Returns((Type type) =>
                {
                    if (!_secondaryEntitiesByType.TryGetValue(type, out var entities))
                    {
                        entities = new List<IEntity>();
                        _secondaryEntitiesByType[type] = entities;
                    }

                    return entities;
                });
        }

        [Test]
        public void Add_ExistingType_AddsEntityToRepository()
        {
            // Arrange
            var entity = new TestEntity1("1");

            // Act
            var result = _compositeRepository.Add<TestEntity1>(entity);

            // Assert
            Assert.AreEqual(entity, result);
            var entityFromRepo = _compositeRepository.GetAll<TestEntity1>();
            Assert.Contains(entity, entityFromRepo);
        }

        [Test]
        public void Add_NonExistingType_ReturnsNull()
        {
            // Arrange
            var entity = new TestEntity3 {Id = "1"};

            // Act
            var result = _compositeRepository.Add<TestEntity3>(entity);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetById_ExistingEntity_ReturnsEntity()
        {
            // Arrange
            var entity = new TestEntity1("1");
            _compositeRepository.Add<TestEntity1>(entity);

            // Act
            var result = _compositeRepository.GetById<TestEntity1>("1");

            // Assert
            Assert.AreEqual(entity, result);
        }

        [Test]
        public void GetById_NonExistingEntity_ReturnsNull()
        {
            // Act
            var result = _compositeRepository.GetById<TestEntity1>("1");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetAll_ExistingType_ReturnsEntities()
        {
            // Arrange
            var entity1 = new TestEntity1("1");
            var entity2 = new TestEntity1("2");
            _compositeRepository.Add<TestEntity1>(entity1);
            _compositeRepository.Add<TestEntity1>(entity2);

            // Act
            var result = _compositeRepository.GetAll<TestEntity1>();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.Contains(entity1, result);
            Assert.Contains(entity2, result);
        }

        [Test]
        public void GetAll_NonExistingType_ReturnsNull()
        {
            // Act
            var result = _compositeRepository.GetAll<TestEntity3>();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetAll_ByType_ExistingType_ReturnsEntities()
        {
            // Arrange
            var entity1 = new TestEntity1("1");
            var entity2 = new TestEntity1("2");
            _compositeRepository.Add<TestEntity1>(entity1);
            _compositeRepository.Add<TestEntity1>(entity2);

            // Act
            var result = _compositeRepository.GetAll(typeof(TestEntity1));

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.Contains(entity1, result);
            Assert.Contains(entity2, result);
        }

        [Test]
        public void GetAll_ByType_NonExistingType_ReturnsNull()
        {
            // Act
            var result = _compositeRepository.GetAll(typeof(TestEntity3));

            // Assert
            Assert.IsNull(result);
        }

        [UnityTest]
        public IEnumerator Load_CallsLoadOnDataContext()
        {
            // Arrange
            _dataContextMock.Setup(x => x.Load()).Returns(UniTask.CompletedTask);

            // Act
            yield return _compositeRepository.Load().ToCoroutine();

            // Assert
            _dataContextMock.Verify(x => x.Load(), Times.Once);
        }

        [UnityTest]
        public IEnumerator Save_CallsSaveOnDataContext()
        {
            // Arrange
            _dataContextMock.Setup(x => x.Save()).Returns(UniTask.CompletedTask);

            // Act
            yield return _compositeRepository.Save().ToCoroutine();

            // Assert
            _dataContextMock.Verify(x => x.Save(), Times.Once);
        }

        [Test]
        public void CopyFrom_CopiesEntitiesFromAnotherRepository()
        {
            // Arrange
            var entity1 = new TestEntity1("1");
            var entity2 = new TestEntity2("2");
            var anotherRepo = new CompositeRepository(_secondaryDataContextMock.Object, _repoTypes);
            anotherRepo.Add<TestEntity1>(entity1);
            anotherRepo.Add<TestEntity2>(entity2);

            // Act
            _compositeRepository.CopyFrom(anotherRepo);

            // Assert
            var result1 = _compositeRepository.GetAll<TestEntity1>();
            var result2 = _compositeRepository.GetAll<TestEntity2>();
            Assert.AreEqual(1, result1.Count);
            Assert.AreEqual(1, result2.Count);
            Assert.Contains(entity1, result1);
            Assert.Contains(entity2, result2);
            Assert.AreNotSame(anotherRepo.GetAll<TestEntity1>().First(),
                _compositeRepository.GetAll<TestEntity1>().First());
        }

        [Test]
        public void Clear_ClearsAllRepositories()
        {
            // Arrange
            var entity1 = new TestEntity1("1");
            var entity2 = new TestEntity2("2");
            _compositeRepository.Add<TestEntity1>(entity1);
            _compositeRepository.Add<TestEntity2>(entity2);

            // Act
            _compositeRepository.Clear();

            // Assert
            var result1 = _compositeRepository.GetAll<TestEntity1>();
            var result2 = _compositeRepository.GetAll<TestEntity2>();
            Assert.AreEqual(0, result1.Count);
            Assert.AreEqual(0, result2.Count);
        }

        [Test]
        public void Remove_Entity_RemovesEntityFromRepository()
        {
            // Arrange
            var entity1 = new TestEntity1("1");
            var entity2 = new TestEntity1("2");
            _compositeRepository.Add<TestEntity1>(entity1);
            _compositeRepository.Add<TestEntity1>(entity2);

            // Act
            _compositeRepository.Remove(entity1);

            // Assert
            var result = _compositeRepository.GetAll<TestEntity1>();
            Assert.AreEqual(1, result.Count);
            Assert.Contains(entity2, result);
        }

        [Test]
        public void Remove_Predicate_RemovesMatchingEntities()
        {
            // Arrange
            var entity1 = new TestEntity1("1") {Name = "Entity 1"};
            var entity2 = new TestEntity1("2") {Name = "Entity 2"};
            var entity3 = new TestEntity1("3") {Name = "Entity 3"};
            _compositeRepository.Add<TestEntity1>(entity1);
            _compositeRepository.Add<TestEntity1>(entity2);
            _compositeRepository.Add<TestEntity1>(entity3);

            // Act
            _compositeRepository.Remove<TestEntity1>(entity => entity.Name.Contains("2"));

            // Assert
            var result = _compositeRepository.GetAll<TestEntity1>();
            Assert.AreEqual(2, result.Count);
            Assert.Contains(entity1, result);
            Assert.Contains(entity3, result);
        }
    }
}