using System;
using System.Linq.Expressions;
using System.Reflection;
using Silent.Practices.MetadataProvider.Builders;
using Silent.Practices.MetadataProvider.Context;
using Silent.Practices.MetadataProvider.Extensions;
using Xunit;

namespace Silent.Practices.MetadataProvider.Tests
{
    public class MemberMetadataBuilderTests
    {
        [Fact]
        public void Property_OnPersonEntity_ShouldReturnMemberBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            IMetadataBuilder builder = new MetadataBuilder(typeCache);

            // Act
            IMemberMetadataBuilder entityBuilder =
                builder.Entity<Person>()
                    .Property(p => p.FirstName);

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void NonEditable_OnFirstNameProperty_ShouldReturnMemberBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            TypeContext context = new TypeContext(typeof(Person).GetTypeInfo());
            ITypeMetadataBuilder<Person> builder =
                new TypeMetadataBuilderWrapper<Person>(
                    new TypeMetadataBuilder(context, typeCache));

            // Act
            IMemberMetadataBuilder entityBuilder =
                builder.Property(p => p.FirstName)
                    .NonEditable();

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void IsRequired_OnFirstNameProperty_ShouldReturnMemberBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            TypeContext context = new TypeContext(typeof(Person).GetTypeInfo());
            ITypeMetadataBuilder<Person> builder =
                new TypeMetadataBuilderWrapper<Person>(
                    new TypeMetadataBuilder(context, typeCache));

            // Act
            IMemberMetadataBuilder entityBuilder =
                builder.Property(p => p.FirstName)
                    .IsRequired();

            // Assert
            Assert.NotNull(entityBuilder);
        }
    }
}
