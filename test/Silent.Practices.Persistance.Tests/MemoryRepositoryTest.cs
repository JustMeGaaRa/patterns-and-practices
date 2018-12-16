using Silent.Practices.DDD;
using System;
using System.Collections.Generic;
using Xunit;

namespace Silent.Practices.Persistance.Tests
{
    public class MemoryRepositoryTest
    {
        [Fact]
        public async void Add_NewEntity_ShouldReturnTrue()
        {
            // Arrange
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { EntityId = Guid.NewGuid() };

            // Act
            bool result = await repository.SaveAsync(fakeEntity);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Add_NullObject_ShouldThrowException()
        {
            // Arrange
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            
            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => repository.SaveAsync(null));
        }

        [Fact]
        public async void Add_ModifiedViaNewInstance_ShouldUpdateOriginal()
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
            bool result = await repository.SaveAsync(originalEntity);
            result = result && await repository.SaveAsync(modifiedEntity);

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
            Assert.ThrowsAsync<KeyNotFoundException>(() => repository.FindByIdAsync(Guid.NewGuid()));
        }

        [Fact]
        public void GetById_WithUnexistingId_ShouldThrowException()
        {
            // Arrange
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { EntityId = Guid.NewGuid() };

            // Act
            repository.SaveAsync(fakeEntity);

            // Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => repository.FindByIdAsync(Guid.NewGuid()));
        }

        [Fact]
        public async void GetById_WithExistingId_ShouldReturnEntity()
        {
            // Arrange
            Guid entityId = Guid.NewGuid();
            IRepositoryWithGuidKey<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { EntityId = entityId };

            // Act
            await repository.SaveAsync(fakeEntity);
            FakeEntity result = await repository.FindByIdAsync(entityId);

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
