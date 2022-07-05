using System.Threading;
using System.Threading.Tasks;
using MediatR;
using N8T.Core.Domain;

namespace N8T.Infrastructure.Events;

public abstract class DomainEventHandler<TEvent> : INotificationHandler<EventWrapper>
    where TEvent : IDomainEvent
{
    protected readonly IPublisher _publisher;    
    
    public DomainEventHandler(IPublisher publisher)
    {
        _publisher = publisher;
    }
    
    public abstract Task HandleEvent(TEvent @event, CancellationToken cancellationToken);

    public virtual async Task Handle(EventWrapper @eventWrapper, CancellationToken cancellationToken)
    {
        if (@eventWrapper.Event is TEvent @event)
        {
            await HandleEvent(@event, cancellationToken);
        }
    }

    protected async Task RelayAndPublishEvents(IAggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
    {
        var @events = new IDomainEvent[aggregateRoot.DomainEvents.Count];
        aggregateRoot.DomainEvents.CopyTo(@events);
        aggregateRoot.DomainEvents.Clear();

        foreach (var @event in @events)
        {
            await _publisher.Publish(new EventWrapper(@event), cancellationToken);
        }
    }
}
