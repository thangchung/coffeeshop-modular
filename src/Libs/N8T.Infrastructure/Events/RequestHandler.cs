using MediatR;
using N8T.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace N8T.Infrastructure.Events;

public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly IPublisher _publisher;

    public RequestHandler(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

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
