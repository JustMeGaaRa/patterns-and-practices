using System;
using System.Collections.Generic;

namespace Silent.Practices.EventStore.Example
{
    internal sealed class Program
    {
        private static void Main()
        {
            IEventStore eventStore = new MemoryEventStore();
            IEventAggregateRepository<Order> orderRepository =
                new MemoryEventAggregateRepository<Order>(eventStore);

            for (uint i = 0; i < 10; i++)
            {
                Order order = new Order(i);
                order.Date = DateTime.Now;
                orderRepository.Save(order);
            }
            
            Order found = orderRepository.GetById(1);

            Console.WriteLine($"{nameof(Order.Id)}: {found.Id}");
            Console.WriteLine($"{nameof(Order.Date)}: {found.Date}");
            Console.ReadKey();
        }

        public class Order : EventAggregate<uint>
        {
            public Order()
            {
                Items = new List<OrderItem>();
                OnEvent<OrderCreatedEvent>(x => Id = x.EntityId);
                OnEvent<OrderDateChangedEvent>(x => Date = x.Date);
                OnEvent<OrderDeletedEvent>(x => { });
            }

            public Order(uint id) : this()
            {
                Apply(new OrderCreatedEvent(id));
            }

            public DateTime Date { get; set; }

            public ICollection<OrderItem> Items { get; }

            private class OrderCreatedEvent : Event
            {
                public OrderCreatedEvent(uint id) : base(id)
                {
                }
            }

            private class OrderDateChangedEvent : Event
            {
                public OrderDateChangedEvent(uint id, DateTime date) : base(id)
                {
                    Date = date;
                }

                public DateTime Date { get; set; }
            }

            private class OrderDeletedEvent : Event
            {
                public OrderDeletedEvent(uint id) : base(id)
                {
                }
            }
        }

        public class OrderItem
        {
            public uint ItemId { get; set; }

            public string Title { get; set; }

            public double Price { get; set; }
        }
    }
}
