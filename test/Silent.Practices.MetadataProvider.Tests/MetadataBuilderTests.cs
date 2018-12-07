using Silent.Practices.MetadataProvider.Builders;
using Silent.Practices.MetadataProvider.Extensions;
using Xunit;

namespace Silent.Practices.MetadataProvider.Tests
{
    public class MetadataBuilderTests
    {
        [Fact]
        public void Entity_OnPersonType_ShouldReturnTypeBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            IMetadataBuilder builder = new MetadataBuilder(typeCache);

            // Act
            ITypeMetadataBuilder<Person> entityBuilder = builder.Entity<Person>();

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void Entity_OnPersonTypeWithEntityBuilderAction_ShouldReturnMemberBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            IMetadataBuilder builder = new MetadataBuilder(typeCache);

            // Act
            IMetadataBuilder entityBuilder = builder
                .Entity<Person>(t =>
                {
                    t.Property(p => p.FirstName).IsRequired();
                    t.Property(p => p.LastName).IsRequired();
                    t.Property(p => p.Positions).NonEditable();
                })
                .Entity<Position>(t =>
                {
                    t.Property(p => p.Title).IsRequired().NonEditable();
                    t.Property(p => p.StartDate).IsRequired();
                    t.Property(p => p.EndDate).IsRequired();
                });

            // Assert
            Assert.NotNull(entityBuilder);
        }
    }
}
