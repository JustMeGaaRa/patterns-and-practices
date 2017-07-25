using Silent.Practices.MetadataProvider.Builders;
using Xunit;

namespace Silent.Practices.MetadataProvider.Tests
{
    public class MetadataProviderTests
    {
        [Fact]
        public void Ctor_WithMetadataBuilder_DoesNotThrowException()
        {
            // Arrange
            IMetadataBuilder builder = new MetadataBuilder();

            // Act
            IMetadataProvider provider = new MetadataProvider(builder);

            // Assert
            Assert.NotNull(provider);
        }

        [Fact]
        public void Ctor_WithNoParameters_DoesNotThrowException()
        {
            // Arrange, Act
            IMetadataProvider provider = new MetadataProvider();

            // Assert
            Assert.NotNull(provider);
        }

        [Fact]
        public void GetMetadata_OnRegisteredType_ShouldReturnMetadata()
        {
            // Arrange
            IMetadataBuilder builder = new MetadataBuilder();
            IMetadataProvider provider = new MetadataProvider(builder);

            // Act
            builder.Entity<FakeEntity>()
                   .HasRequired(x => x.Name)
                   .HasNonEditable(x => x.Name);

            TypeMetadata metadata = provider.GetMetadata(typeof(FakeEntity));

            // Assert
            Assert.NotNull(metadata);
        }

        [Fact]
        public void GetMetadataExtention_OnRegisteredType_ShouldReturnMetadata()
        {
            // Arrange
            IMetadataBuilder builder = new MetadataBuilder();
            IMetadataProvider provider = new MetadataProvider(builder);

            // Act
            builder.Entity<FakeEntity>()
                .HasRequired(x => x.Name)
                .HasNonEditable(x => x.Name);

            TypeMetadata metadata = provider.GetMetadata<FakeEntity>();

            // Assert
            Assert.NotNull(metadata);
        }

        [Fact]
        public void GetMetadata_OnNotRegisteredType_ShouldReturnMetadata()
        {
            // Arrange
            IMetadataProvider provider = new MetadataProvider();

            // Act
            TypeMetadata metadata = provider.GetMetadata<FakeEntity>();

            // Assert
            Assert.NotNull(metadata);
        }

        private class FakeEntity
        {
            public string Name { get; set; }
        }
    }
}
