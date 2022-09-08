using NHibernate.Event;

namespace Utility.Negocio
{
    public class EventListener : 
        IPostInsertEventListener, 
        IPostDeleteEventListener, 
        IPostUpdateEventListener, 
        IPostCollectionUpdateEventListener
    {
        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            DispatchEvents(@event.Entity as AggregateRoot);
            return Task.CompletedTask;
        }

        public void OnPostUpdate(PostUpdateEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
        {
            DispatchEvents(@event.Entity as AggregateRoot);
            return Task.CompletedTask;
        }

        public void OnPostDelete(PostDeleteEvent @event)
        {
            DispatchEvents(@event.Entity as AggregateRoot);
        }

        public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            DispatchEvents(@event.Entity as AggregateRoot);
            return Task.CompletedTask;
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            DispatchEvents(@event.Entity as AggregateRoot);
        }

        public Task OnPostUpdateCollectionAsync(PostCollectionUpdateEvent @event, CancellationToken cancellationToken)
        {
            DispatchEvents(@event.AffectedOwnerOrNull as AggregateRoot);
            return Task.CompletedTask;
        }

        public void OnPostUpdateCollection(PostCollectionUpdateEvent @event)
        {
            DispatchEvents(@event.AffectedOwnerOrNull as AggregateRoot);
        }

        private void DispatchEvents(AggregateRoot? aggregateRoot)
        {
            if (aggregateRoot == null)
                return;

            foreach (IDomainEvent domainEvent in aggregateRoot.DomainEvents)
            {
                DomainEvents.Dispatch(domainEvent);
            }

            aggregateRoot.ClearEvents();
        }
    }
}
