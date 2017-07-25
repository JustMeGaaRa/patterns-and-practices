using System;
using System.Linq;
using Silent.Practices.MetadataProvider.Builders;

namespace Silent.Practices.MetadataProvider.Sample
{
    internal class Program
    {
        private static void Main()
        {
            IMetadataBuilder builder = new MetadataBuilder();
            IMetadataProvider provider = new MetadataProvider(builder);

            builder.Entity<Order>()
                .HasRequired(x => x.OrderId)
                .HasNonEditable(x => x.OrderId)
                .HasRequired(x => x.Price)
                .HasNonEditable(x => x.Price);

            builder.Entity<OrderItem>()
                .Property(x => x.Name)
                .IsRequired()
                .NonEditable();

            TypeMetadata metadata = provider.GetMetadata(typeof(Order));

            Console.WriteLine(metadata);
        }

        private class Order
        {
            public int OrderId { get; set; }

            public string Summary { get; set; }

            public decimal Price
            {
                get { return Items?.Sum(x => x.Price) ?? 0; }
            }

            public OrderItem[] Items { get; set; }
        }

        private class OrderItem
        {
            public string Name { get; set; }

            public int Count { get; set; }

            public decimal Price { get; set; }
        }
    }
}
