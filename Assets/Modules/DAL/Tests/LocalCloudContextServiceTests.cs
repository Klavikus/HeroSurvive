using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Repositories;
using Modules.DAL.Implementation.Data.Entities;
using Modules.DAL.Implementation.Services;
using Moq;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Modules.DAL.Tests
{
    [TestFixture]
    public class LocalCloudContextServiceTests
    {
        private Mock<ICompositeRepository> _localCompositeRepositoryMock;
        private Mock<ICompositeRepository> _cloudCompositeRepositoryMock;
        private LocalCloudContextService _service;

        [SetUp]
        public void SetUp()
        {
            _localCompositeRepositoryMock = new Mock<ICompositeRepository>();
            _cloudCompositeRepositoryMock = new Mock<ICompositeRepository>();
            _service = new LocalCloudContextService(
                _localCompositeRepositoryMock.Object,
                _cloudCompositeRepositoryMock.Object
            );
        }

        [Test]
        public void LoadLocalContext_CallsLoadOnLocalRepository()
        {
            // Act
            _service.LoadLocalContext();

            // Assert
            _localCompositeRepositoryMock.Verify(repository => repository.Load(), Times.Once);
        }

        [Test]
        public void LoadCloudContext_CallsLoadOnCloudRepository()
        {
            // Act
            _service.LoadCloudContext();

            // Assert
            _cloudCompositeRepositoryMock.Verify(repository => repository.Load(), Times.Once);
        }

        [UnityTest]
        public IEnumerator SaveLocalContext_AddsNewSyncDataWhenNotExists()
        {
            // Arrange
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>()).Returns(new List<SyncData>());

            // Act
            yield return _service.SaveLocalContext().ToCoroutine();

            // Assert
            _localCompositeRepositoryMock.Verify(repository => repository.Add<SyncData>(It.IsAny<SyncData>()), Times.Once);
            _localCompositeRepositoryMock.Verify(repository => repository.Save(), Times.Once);
        }

        [UnityTest]
        public IEnumerator SaveLocalContext_IncreasesSyncCountWhenSyncDataExists()
        {
            // Arrange
            var syncData = new SyncData {SyncCount = 1};
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData> {syncData});

            // Act
            yield return _service.SaveLocalContext().ToCoroutine();

            // Assert
            Assert.AreEqual(2, syncData.SyncCount);
            _localCompositeRepositoryMock.Verify(repository => repository.Save(), Times.Once);
        }

        [Test]
        public void SaveCloudContext_CallsSaveOnCloudRepository()
        {
            // Act
            _service.SaveCloudContext();

            // Assert
            _cloudCompositeRepositoryMock.Verify(repository => repository.Save(), Times.Once);
        }

        [Test]
        public void CheckUpdateFromCloud_ReturnsFalseWhenCloudSyncDataIsEmpty()
        {
            // Arrange
            _cloudCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>()).Returns(new List<SyncData>());

            // Act
            bool result = _service.CheckUpdateFromCloud();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckUpdateFromCloud_ReturnsTrueWhenLocalSyncDataIsEmpty()
        {
            // Arrange
            _cloudCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData> {new()});
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData>());

            // Act
            bool result = _service.CheckUpdateFromCloud();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckUpdateFromCloud_ReturnsTrueWhenCloudSyncCountIsGreaterAndLevelProgressDifferent()
        {
            // Arrange
            _cloudCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData> {new() {SyncCount = 2}});
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData> {new() {SyncCount = 1}});
            _cloudCompositeRepositoryMock.Setup(repository => repository.GetAll<LevelProgress>())
                .Returns(new List<LevelProgress> {new("1") {LevelCurrency = 10}});
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<LevelProgress>())
                .Returns(new List<LevelProgress> {new("1") {LevelCurrency = 5}});

            // Act
            bool result = _service.CheckUpdateFromCloud();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckUpdateFromLocal_ReturnsFalseWhenLocalSyncDataIsEmpty()
        {
            // Arrange
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>()).Returns(new List<SyncData>());

            // Act
            bool result = _service.CheckUpdateFromLocal();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckUpdateFromLocal_ReturnsTrueWhenCloudSyncDataIsEmpty()
        {
            // Arrange
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData> {new()});
            _cloudCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData>());

            // Act
            bool result = _service.CheckUpdateFromLocal();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckUpdateFromLocal_ReturnsTrueWhenLocalSyncCountIsGreaterAndLevelProgressDifferent()
        {
            // Arrange
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData> {new() {SyncCount = 2}});
            _cloudCompositeRepositoryMock.Setup(repository => repository.GetAll<SyncData>())
                .Returns(new List<SyncData> {new() {SyncCount = 1}});
            _localCompositeRepositoryMock.Setup(repository => repository.GetAll<LevelProgress>()).Returns(
                new List<LevelProgress> {new("1") {LevelCurrency = 10}});
            _cloudCompositeRepositoryMock.Setup(repository => repository.GetAll<LevelProgress>()).Returns(
                new List<LevelProgress> {new("1") {LevelCurrency = 5}});

            // Act
            bool result = _service.CheckUpdateFromLocal();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TransferCloudToLocal_CallsCopyFromOnLocalRepository()
        {
            // Act
            _service.TransferCloudToLocal();

            // Assert
            _localCompositeRepositoryMock.Verify(repository => repository.CopyFrom(_cloudCompositeRepositoryMock.Object),
                Times.Once);
        }

        [Test]
        public void TransferLocalToCloud_CallsCopyFromOnCloudRepository()
        {
            // Act
            _service.TransferLocalToCloud();

            // Assert
            _cloudCompositeRepositoryMock.Verify(repository => repository.CopyFrom(_localCompositeRepositoryMock.Object),
                Times.Once);
        }

        [UnityTest]
        public IEnumerator ClearLocalContext_CallsClearAndSaveOnLocalRepository()
        {
            // Act
            yield return _service.ClearLocalContext().ToCoroutine();

            // Assert
            _localCompositeRepositoryMock.Verify(repository => repository.Clear(), Times.Once);
            _localCompositeRepositoryMock.Verify(repository => repository.Save(), Times.Once);
        }

        [UnityTest]
        public IEnumerator ClearCloudContext_CallsClearAndSaveOnCloudRepository()
        {
            // Act
            yield return _service.ClearCloudContext().ToCoroutine();

            // Assert
            _cloudCompositeRepositoryMock.Verify(repository => repository.Clear(), Times.Once);
            _cloudCompositeRepositoryMock.Verify(repository => repository.Save(), Times.Once);
        }
    }
}