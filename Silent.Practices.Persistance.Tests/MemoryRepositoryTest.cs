using System;
using System.Collections.Generic;
using Xunit;

namespace Silent.Practices.Persistance.Tests
{
    public class MemoryRepositoryTest
    {
        [Fact]
        public void Save_NewEntity_ShouldContainOneItem()
        {
            // Arrange
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { Id = 1 };

            // Act
            bool result = repository.Save(fakeEntity);

            // Assert
            Assert.True(result);
            Assert.Equal(1, repository.Count);
        }

        [Fact]
        public void Save_NullObject_ShouldThrowException()
        {
            // Arrange
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => repository.Save(null));
        }

        [Fact]
        public void Save_ModifiedInstance_ShouldUpdateOriginal()
        {
            // Arrange
            string oldValue = "Old Value";
            string newValue = "New Value";
            int entityId = 1;

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
            bool result = repository.Save(originalEntity);
            result = result && repository.Save(modifiedEntity);

            // Assert
            Assert.True(result);
            Assert.Equal(1, repository.Count);
            Assert.NotSame(originalEntity, modifiedEntity);
            Assert.Equal(originalEntity.Value, modifiedEntity.Value);
        }

        [Fact]
        public void GetById_OnEmptyRepository_ShouldThrowException()
        {
            // Arrange
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();

            // Act, Assert
            Assert.Equal(0, repository.Count);
            Assert.Throws<KeyNotFoundException>(() => repository.GetById(1));
        }

        [Fact]
        public void GetById_WithUnexistingId_ShouldThrowException()
        {
            // Arrange
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { Id = 1 };

            // Act
            repository.Save(fakeEntity);

            // Assert
            Assert.Equal(1, repository.Count);
            Assert.Throws<KeyNotFoundException>(() => repository.GetById(2));
        }

        [Fact]
        public void GetById_WithExistingId_ShouldReturnEntity()
        {
            // Arrange
            int entityId = 1;
            IRepository<FakeEntity> repository = new MemoryRepository<FakeEntity>();
            FakeEntity fakeEntity = new FakeEntity { Id = entityId };

            // Act
            repository.Save(fakeEntity);
            var result = repository.GetById(entityId);

            // Assert
            Assert.Equal(1, repository.Count);
            Assert.NotNull(result);
            Assert.Same(fakeEntity, result);
        }

        private class FakeEntity : EntityBase<int>
        {
            public string Value { get; set; }
        }
    }
}
