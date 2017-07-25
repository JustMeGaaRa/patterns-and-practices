using System;
using System.Linq.Expressions;
using System.Reflection;
using Silent.Practices.MetadataProvider.Builders;
using Silent.Practices.MetadataProvider.Context;
using Xunit;

namespace Silent.Practices.MetadataProvider.Tests
{
    public class MetadataBuilderTests
    {
        [Fact]
        public void Entity_OnType_ShouldReturnTypeBuilder()
        {
            // Arrange
            IMetadataBuilder builder = new MetadataBuilder();

            // Act
            ITypeMetadataBuilder<FakeEntity> entityBuilder = builder.Entity<FakeEntity>();

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void HasRequired_OnEntity_ShouldReturnTypeBuilder()
        {
            // Arrange
            IMetadataBuilder builder = new MetadataBuilder();
            Expression<Func<FakeEntity, string>> propertyExpression = p => p.Name;

            // Act
            ITypeMetadataBuilder<FakeEntity> entityBuilder =
                builder.Entity<FakeEntity>()
                       .HasRequired(propertyExpression);

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void HasNonEditable_OnEntity_ShouldReturnTypeBuilder()
        {
            // Arrange
            IMetadataBuilder builder = new MetadataBuilder();
            Expression<Func<FakeEntity, string>> propertyExpression = p => p.Name;

            // Act
            ITypeMetadataBuilder<FakeEntity> entityBuilder =
                builder.Entity<FakeEntity>()
                       .HasNonEditable(propertyExpression);

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void Property_OnEntity_ShouldReturnMemberBuilder()
        {
            // Arrange
            IMetadataBuilder builder = new MetadataBuilder();
            Expression<Func<FakeEntity, string>> propertyExpression = p => p.Name;

            // Act
            IMemberMetadataBuilder entityBuilder =
                builder.Entity<FakeEntity>()
                       .Property(propertyExpression);

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void IsRequired_OnProperty_ShouldReturnMemberBuilder()
        {
            // Arrange
            TypeContext context = new TypeContext(typeof(FakeEntity).GetTypeInfo());
            ITypeMetadataBuilder<FakeEntity> builder = new TypeMetadataBuilder<FakeEntity>(context);

            // Act
            IMemberMetadataBuilder entityBuilder =
                builder.Property(p => p.Name)
                       .IsRequired();

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void NonEditable_OnProperty_ShouldReturnMemberBuilder()
        {
            // Arrange
            TypeContext context = new TypeContext(typeof(FakeEntity).GetTypeInfo());
            ITypeMetadataBuilder<FakeEntity> builder = new TypeMetadataBuilder<FakeEntity>(context);

            // Act
            IMemberMetadataBuilder entityBuilder =
                builder.Property(p => p.Name)
                       .NonEditable();

            // Assert
            Assert.NotNull(entityBuilder);
        }

        private class FakeEntity
        {
            public string Name { get; set; }
        }
    }
}
