using Silent.Practices.DDD;
using System;
using System.Collections.Generic;
using Xunit;

namespace Silent.Practices.Persistance.Tests
{
    public class MemoryRepositoryTest
    {
        [Fact]
        public void Add_NewEntity_ShouldReturnTrue()
        {
            // Arrange
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { EntityId = Guid.NewGuid() };

            // Act
            bool result = repository.Add(fakeEntity);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Add_NullObject_ShouldReturnFalse()
        {
            // Arrange
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();

            // Act
            bool result = repository.Add(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Add_ModifiedViaNewInstance_ShouldUpdateOriginal()
        {
            // Arrange
            string oldValue = "Old Value";
            string newValue = "New Value";
            Guid entityId = Guid.NewGuid();

            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity originalEntity = new FakeEntity
            {
                EntityId = entityId,
                Value = oldValue
            };
            FakeEntity modifiedEntity = new FakeEntity
            {
                EntityId = entityId,
                Value = newValue
            };

            // Act
            bool result = repository.Add(originalEntity);
            result = result && repository.Add(modifiedEntity);

            // Assert
            Assert.True(result);
            Assert.NotSame(originalEntity, modifiedEntity);
            Assert.Equal(originalEntity.Value, modifiedEntity.Value);
        }

        [Fact]
        public void GetById_OnEmptyRepository_ShouldThrowException()
        {
            // Arrange
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();

            // Act, Assert
            Assert.Throws<KeyNotFoundException>(() => repository.FindById(Guid.NewGuid()));
        }

        [Fact]
        public void GetById_WithUnexistingId_ShouldThrowException()
        {
            // Arrange
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { EntityId = Guid.NewGuid() };

            // Act
            repository.Add(fakeEntity);

            // Assert
            Assert.Throws<KeyNotFoundException>(() => repository.FindById(Guid.NewGuid()));
        }

        [Fact]
        public void GetById_WithExistingId_ShouldReturnEntity()
        {
            // Arrange
            Guid entityId = Guid.NewGuid();
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { EntityId = entityId };

            // Act
            repository.Add(fakeEntity);
            var result = repository.FindById(entityId);

            // Assert
            Assert.NotNull(result);
            Assert.Same(fakeEntity, result);
        }

        private class FakeEntity : EntityWithGuidKey
        {
            public string Value { get; set; }
        }
    }
}
