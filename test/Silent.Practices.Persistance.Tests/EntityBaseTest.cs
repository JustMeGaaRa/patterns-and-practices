using Silent.Practices.DDD;
using Xunit;

namespace Silent.Practices.Persistance.Tests
{
    public class EntityBaseTest
    {
        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public void Equals_OnInstances_ShouldReturnCorrectResult(int firstId, int secondId, bool expected)
        {
            // Arrange
            FakeEntity fakeEntity1 = new FakeEntity { EntityId = firstId };
            FakeEntity fakeEntity2 = new FakeEntity { EntityId = secondId };

            // Act
            bool result = fakeEntity1.Equals(fakeEntity2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public void GetHashCode_OnInstancesWithSameIds_ShouldHaveSameHasCode(int firstId, int secondId, bool expected)
        {
            // Arrange
            FakeEntity fakeEntity1 = new FakeEntity { EntityId = firstId };
            FakeEntity fakeEntity2 = new FakeEntity { EntityId = secondId };

            // Act
            int hashCode1 = fakeEntity1.GetHashCode();
            int hashCode2 = fakeEntity2.GetHashCode();

            // Assert
            Assert.Equal(expected, hashCode1 == hashCode2);
        }

        private class FakeEntity : EntityWithIntKey
        {
        }
    }
}
