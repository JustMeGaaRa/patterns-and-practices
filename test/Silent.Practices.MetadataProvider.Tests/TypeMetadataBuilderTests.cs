using System.Reflection;
using Silent.Practices.MetadataProvider.Builders;
using Silent.Practices.MetadataProvider.Context;
using Silent.Practices.MetadataProvider.Extensions;
using Xunit;

namespace Silent.Practices.MetadataProvider.Tests
{
    public class TypeMetadataBuilderTests
    {
        [Fact]
        public void HasNonEditable_OnFirstNameProperty_ShouldReturnTypeBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            TypeContext context = new TypeContext(typeof(Person).GetTypeInfo());
            ITypeMetadataBuilder<Person> builder =
                new TypeMetadataBuilderWrapper<Person>(
                    new TypeMetadataBuilder(context, typeCache));

            // Act
            ITypeMetadataBuilder<Person> entityBuilder = builder.HasNonEditable(p => p.FirstName);

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void HasRequired_OnFirstNameProperty_ShouldReturnTypeBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            TypeContext context = new TypeContext(typeof(Person).GetTypeInfo());
            ITypeMetadataBuilder<Person> builder =
                new TypeMetadataBuilderWrapper<Person>(
                    new TypeMetadataBuilder(context, typeCache));

            // Act
            ITypeMetadataBuilder<Person> entityBuilder = builder.HasRequired(p => p.FirstName);

            // Assert
            Assert.NotNull(entityBuilder);
        }

        [Fact]
        public void Property_OnFirstNamePropertyExpression_ShouldReturnMemberBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            TypeContext context = new TypeContext(typeof(Person).GetTypeInfo());
            ITypeMetadataBuilder<Person> builder =
                new TypeMetadataBuilderWrapper<Person>(
                    new TypeMetadataBuilder(context, typeCache));

            // Act
            IMemberMetadataBuilder entityBuilder = builder.Property(p => p.FirstName);

            // Assert
            Assert.NotNull(entityBuilder);
        }



        [Fact]
        public void Property_OnFirstNamePropertyName_ShouldReturnMemberBuilder()
        {
            // Arrange
            TypeCache typeCache = new TypeCache();
            TypeContext context = new TypeContext(typeof(Person).GetTypeInfo());
            ITypeMetadataBuilder<Person> builder =
                new TypeMetadataBuilderWrapper<Person>(
                    new TypeMetadataBuilder(context, typeCache));

            // Act
            IMemberMetadataBuilder entityBuilder = builder.Property(nameof(Person.FirstName));

            // Assert
            Assert.NotNull(entityBuilder);
        }
    }
}
