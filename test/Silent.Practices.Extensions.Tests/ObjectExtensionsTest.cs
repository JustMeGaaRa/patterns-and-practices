using System.Collections.Generic;
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
        
        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("Hello World!")]
        public void AsArray_OnObject_ShouldCreateArray(object dummy)
        {
            // Act
            object[] array = dummy.AsArray();

            // Assert
            Assert.NotEmpty(array);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("Hello World!")]
        public void AsList_OnObject_ShouldCreateArray(object dummy)
        {
            // Act
            List<object> array = dummy.AsList();

            // Assert
            Assert.NotEmpty(array);
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
