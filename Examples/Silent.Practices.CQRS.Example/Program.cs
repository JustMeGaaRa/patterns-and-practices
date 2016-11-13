using System;
using System.Collections.Generic;
using Silent.Practices.EventStore;
using Silent.Practices.Messaging;
using Silent.Practices.Persistance;

namespace Silent.Practices.CQRS.Example
{
    internal sealed class Program
    {
        private static void Main()
        {
            // units of work
            IEventStore eventStore = new MemoryEventStore();
            IUnitOfWork aggregatesUnitOfWork = new AggregateUnitOfWorkFactory().Create(eventStore);
            IUnitOfWork domainUnitOfWork = new DomainUnitOfWorkFactory().Create();

            // messaging
            IMessageBus<object> eventBus = new MemoryMessageBus<object>();

            OrderCommandsHandler orderCommandsHandler = new OrderCommandsHandler(aggregatesUnitOfWork);
            eventBus.Subscribe<CreateOrderCommand>(orderCommandsHandler);
            eventBus.Subscribe<ChangeOrderDateCommand>(orderCommandsHandler);
            eventBus.Subscribe<DeleteOrderCommand>(orderCommandsHandler);

            OrderEventsHandler orderEventsHandler = new OrderEventsHandler(domainUnitOfWork, eventBus);
            eventBus.Subscribe<OrderCreatedEvent>(orderEventsHandler);
            eventBus.Subscribe<OrderDateChangedEvent>(orderEventsHandler);
            eventBus.Subscribe<OrderDeletedEvent>(orderEventsHandler);

            // commands simulation
            uint iterations = 3;

            for (uint i = 0; i < iterations; i++)
            {
                eventBus.Publish(new CreateOrderCommand(i));
                eventBus.Publish(new ChangeOrderDateCommand(i, DateTime.Now.AddDays(1)));
                eventBus.Publish(new DeleteOrderCommand(i));
            }

            for (uint i = 0; i < iterations; i++)
            {
                OrderDto found = domainUnitOfWork.GetRepository<OrderDto>().GetById(i);
                Console.WriteLine($"{nameof(OrderDto.Id)}: {found.Id}");
                Console.WriteLine($"{nameof(OrderDto.Date)}: {found.Date}");
            }

            Console.ReadKey();
        }

        #region Units Of Work

        public class AggregateUnitOfWorkFactory : IUnitOfWorkFactory
        {
            public IUnitOfWork Create(params object[] parameters)
            {
                if (parameters == null)
                {
                    throw new ArgumentNullException(nameof(parameters));
                }

                IEventStore eventStore = parameters[0] as IEventStore;

                MemoryUnitOfWork memoryUnitOfWork = new MemoryUnitOfWork();
                memoryUnitOfWork.UseRepository(new MemoryEventAggregateRepository<OrderAggregate>(eventStore));
                return memoryUnitOfWork;
            }
        }

        public class DomainUnitOfWorkFactory : IUnitOfWorkFactory
        {
            public IUnitOfWork Create(params object[] parameters)
            {
                MemoryUnitOfWork memoryUnitOfWork = new MemoryUnitOfWork();
                memoryUnitOfWork.UseRepository(new MemoryRepository<OrderDto>());
                return memoryUnitOfWork;
            }
        }

        #endregion

        #region Commands

        public class CreateOrderCommand : Command
        {
            public CreateOrderCommand(uint id) : base(id)
            {
            }
        }

        public class ChangeOrderDateCommand : Command
        {
            public ChangeOrderDateCommand(uint id, DateTime date) : base(id)
            {
                Date = date;
            }

            public DateTime Date { get; set; }
        }

        public class DeleteOrderCommand : Command
        {
            public DeleteOrderCommand(uint id) : base(id)
            {
            }
        }

        #endregion

        #region Command Handlers

        public class OrderCommandsHandler :
            ICommandHandler<CreateOrderCommand>,
            ICommandHandler<ChangeOrderDateCommand>,
            ICommandHandler<DeleteOrderCommand>
        {
            private readonly IUnitOfWork _unitOfWork;

            public OrderCommandsHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public void Handle(CreateOrderCommand instance)
            {
                OrderAggregate orderAggregate = new OrderAggregate(instance.EntityId);
                _unitOfWork.GetRepository<OrderAggregate>().Save(orderAggregate);
            }

            public void Handle(ChangeOrderDateCommand instance)
            {
                OrderAggregate orderAggregate = _unitOfWork
                    .GetRepository<OrderAggregate>()
                    .GetById(instance.EntityId);
                orderAggregate.ChangeDate(instance.Date);
                _unitOfWork.GetRepository<OrderAggregate>().Save(orderAggregate);
            }

            public void Handle(DeleteOrderCommand instance)
            {
                OrderAggregate orderAggregate = _unitOfWork
                    .GetRepository<OrderAggregate>()
                    .GetById(instance.EntityId);
                orderAggregate.Delete();
                _unitOfWork.GetRepository<OrderAggregate>().Save(orderAggregate);
            }
        }

        #endregion

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
            public void ChangePrice(double price)
            {
                ApplyEvent(new OrderItemPriceChangedEvent(price));
            }
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

        public class OrderItemAddedToOrderEvent : Event
        {

        }

        public class OrderItemRemovedFromOrderEvent : Event
        {

        }

        public class OrderItemPriceChangedEvent : Event
        {
            public OrderItemPriceChangedEvent(double price)
            {
                Price = price;
            }

            public double Price { get; set; }
        }

        #endregion

        #region Event Handlers

        public class OrderEventsHandler :
            IEventHandler<OrderCreatedEvent>,
            IEventHandler<OrderDateChangedEvent>,
            IEventHandler<OrderDeletedEvent>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMessageBus<object> _eventBus;

            public OrderEventsHandler(IUnitOfWork unitOfWork, IMessageBus<object> eventBus)
            {
                _unitOfWork = unitOfWork;
                _eventBus = eventBus;
            }

            public void Handle(OrderCreatedEvent instance)
            {
                OrderDto order = new OrderDto { Id = instance.EntityId };
                _unitOfWork.GetRepository<OrderDto>().Save(order);
            }

            public void Handle(OrderDateChangedEvent instance)
            {
                OrderDto order = _unitOfWork.GetRepository<OrderDto>().GetById(instance.EntityId);
                order.Date = instance.Date;
                _unitOfWork.GetRepository<OrderDto>().Save(order);
            }

            public void Handle(OrderDeletedEvent instance)
            {
                // TODO: delete from repository
                // OrderDto order = _unitOfWork.GetRepository<OrderDto>().GetById(instance.EntityId);
                // _unitOfWork.GetRepository<OrderDto>().Delete(order);
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
