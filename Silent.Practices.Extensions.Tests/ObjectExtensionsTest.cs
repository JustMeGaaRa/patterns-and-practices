using System;
using Xunit;

namespace Silent.Practices.Extensions.Tests
{
    public class ObjectExtensionsTest
    {
        [Fact]
        public void Patch_OnModifiedProperty_ShouldSetNewValues()
        {
            // Arrange
            FakeReadWriteObject fakeObjectTarget = new FakeReadWriteObject
            {
                Id = 1,
                Value = "Old Value"
            };
            FakeReadWriteObject fakeObjectSource = new FakeReadWriteObject
            {
                Id = 2,
                Value = "New Value"
            };

            // Act
            fakeObjectTarget.Patch(fakeObjectSource);

            // Assert
            Assert.NotSame(fakeObjectTarget, fakeObjectSource);
            Assert.Equal(fakeObjectTarget.Id, fakeObjectSource.Id);
            Assert.Equal(fakeObjectTarget.Value, fakeObjectSource.Value);
        }

        [Fact]
        public void Patch_WithReadonlyProperty_ShouldSetWriteableValuesOnly()
        {
            // Arrange
            FakeWithReadonlyObject fakeObjectTarget = new FakeWithReadonlyObject(1) { Value = "Old Value" };
            FakeWithReadonlyObject fakeObjectSource = new FakeWithReadonlyObject(2) { Value = "New Value" };

            // Act
            fakeObjectTarget.Patch(fakeObjectSource);

            // Assert
            Assert.NotSame(fakeObjectTarget, fakeObjectSource);
            Assert.NotEqual(fakeObjectTarget.Id, fakeObjectSource.Id);
            Assert.Equal(fakeObjectTarget.Value, fakeObjectSource.Value);
        }

        [Fact]
        public void Patch_OnSameInstance_ShouldThrowException()
        {
            // Arrange
            FakeReadWriteObject fakeObject = new FakeReadWriteObject
            {
                Id = 1,
                Value = "Value"
            };

            // Act, Assert
            Assert.Throws<NotSupportedException>(() => fakeObject.Patch(fakeObject));
        }

        public class FakeReadWriteObject
        {
            public int Id { get; set; }

            public string Value { get; set; }
        }

        public class FakeWithReadonlyObject
        {
            public FakeWithReadonlyObject(int id)
            {
                Id = id;
            }

            public int Id { get; }

            public string Value { get; set; }
        }
    }
}
