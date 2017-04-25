using System;
using System.Collections.Generic;
using Silent.Practices.Persistance;

namespace Silent.Practices.EventStore.Sample
{
    internal sealed class Program
    {
        private static void Main()
        {
            IEventStore eventStore = new MemoryEventStore();

            IEventAggregateRepository<OrderAggregate> orderAggregateRepository =
                new MemoryEventAggregateRepository<OrderAggregate>(eventStore);
            IEventAggregateRepository<OrderItemAggregate> orderitemAggregateRepository =
                new MemoryEventAggregateRepository<OrderItemAggregate>(eventStore);

            IRepository<OrderDto> orderRepository = new MemoryRepository<OrderDto>();
            IRepository<OrderItemDto> orderItemRepository = new MemoryRepository<OrderItemDto>();

            for (uint i = 0; i < 10; i++)
            {
                OrderAggregate orderAggregate = new OrderAggregate(i);
                orderAggregate.ChangeDate(DateTime.Now.AddDays(1));
                orderAggregate.Delete();
                orderAggregateRepository.Add(orderAggregate);
            }

            OrderDto found = orderRepository.GetById(1);

            Console.WriteLine($"{nameof(OrderDto.Id)}: {found.Id}");
            Console.WriteLine($"{nameof(OrderDto.Date)}: {found.Date}");
            Console.ReadKey();
        }

        #region Event Aggregates

        public class OrderAggregate : EventAggregate<uint>
        {
            public OrderAggregate()
            {
            }

            public OrderAggregate(uint id) : this()
            {
                ApplyEvent(new OrderCreatedEvent(id));
            }

            public void ChangeDate(DateTime dateTime)
            {
                ApplyEvent(new OrderDateChangedEvent(Id, dateTime));
            }

            public void Delete()
            {
                ApplyEvent(new OrderDeletedEvent(Id));
            }
        }

        public class OrderItemAggregate : EventAggregate<uint>
        {
            
        }

        #endregion

        #region Events

        public class OrderCreatedEvent : Event
        {
            public OrderCreatedEvent(uint id) : base(id)
            {
            }
        }

        public class OrderDateChangedEvent : Event
        {
            public OrderDateChangedEvent(uint id, DateTime date) : base(id)
            {
                Date = date;
            }

            public DateTime Date { get; set; }
        }

        public class OrderDeletedEvent : Event
        {
            public OrderDeletedEvent(uint id) : base(id)
            {
            }
        }

        #endregion

        #region Domain Entities

        public class OrderDto : EntityBase<uint>
        {
            public OrderDto()
            {
                Items = new List<OrderItemDto>();
            }

            public DateTime Date { get; set; }

            public List<OrderItemDto> Items { get; }
        }

        public class OrderItemDto : EntityBase<uint>
        {
            public string Title { get; set; }

            public double Price { get; set; }
        }

        #endregion
    }
}
