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
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { Id = 1 };

            // Act
            bool result = repository.Add(fakeEntity);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Add_NullObject_ShouldReturnFalse()
        {
            // Arrange
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();

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
            uint entityId = 1;

            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity originalEntity = new FakeEntity
            {
                Id = entityId,
                Value = oldValue
            };
            FakeEntity modifiedEntity = new FakeEntity
            {
                Id = entityId,
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
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();

            // Act, Assert
            Assert.Throws<KeyNotFoundException>(() => repository.GetById(1));
        }

        [Fact]
        public void GetById_WithUnexistingId_ShouldThrowException()
        {
            // Arrange
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { Id = 1 };

            // Act
            repository.Add(fakeEntity);

            // Assert
            Assert.Throws<KeyNotFoundException>(() => repository.GetById(2));
        }

        [Fact]
        public void GetById_WithExistingId_ShouldReturnEntity()
        {
            // Arrange
            uint entityId = 1;
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { Id = entityId };

            // Act
            repository.Add(fakeEntity);
            var result = repository.GetById(entityId);

            // Assert
            Assert.NotNull(result);
            Assert.Same(fakeEntity, result);
        }

        private class FakeEntity : EntityBase<uint>
        {
            public string Value { get; set; }
        }
    }
}
