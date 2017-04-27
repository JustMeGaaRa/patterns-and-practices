using System;
using System.Collections.Generic;
using Silent.Practices.Persistance;

namespace Silent.Practices.EventStore.Sample
{
    internal sealed class Program
    {
        private static void Main()
        {
            IComparer<Event<uint>> comparer = CreateEventComparer();
            IEventStore<uint, Event<uint>> eventStore = new MemoryEventStore<uint, Event<uint>>(comparer);

            for (uint i = 0; i < 10; i++)
            {
                List<Event<uint>> events = new List<Event<uint>>();
                events.Add(new OrderCreatedEvent(i));
                events.Add(new OrderDateChangedEvent(i, DateTime.Now.AddDays(i)));
                events.Add(new OrderDeletedEvent(i));
                eventStore.SaveEvents(i, events);
            }

            IReadOnlyCollection<Event<uint>> found = eventStore.GetEventsById(1);

            Console.WriteLine($"Found events: {found.Count}");
            Console.ReadKey();
        }

        private static IComparer<Event<uint>> CreateEventComparer()
        {
            return Comparer<IEvent>.Create(CompareEvents);
        }

        private static int CompareEvents(IEvent left, IEvent right)
        {
            if (left.Timestamp > right.Timestamp)
            {
                return -1;
            }
            if (left.Timestamp < right.Timestamp)
            {
                return 1;
            }
            return 0;
        }

        #region Events

        public class OrderCreatedEvent : Event<uint>
        {
            public OrderCreatedEvent(uint id) : base(id)
            {
            }
        }

        public class OrderDateChangedEvent : Event<uint>
        {
            public OrderDateChangedEvent(uint id, DateTime date) : base(id)
            {
                Date = date;
            }

            public DateTime Date { get; set; }
        }

        public class OrderDeletedEvent : Event<uint>
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
