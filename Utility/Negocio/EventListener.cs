using NHibernate.Event;

namespace Utility.Negocio
{
    public class EventListener : 
        IPostInsertEventListener, 
        IPostDeleteEventListener, 
        IPostUpdateEventListener, 
        IPostCollectionUpdateEventListener
    {
        public Task OnPostUpdateAsync(PostUpdateEvent ev, CancellationToken cancellationToken)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
            return Task.CompletedTask;
        }

        public void OnPostUpdate(PostUpdateEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public Task OnPostDeleteAsync(PostDeleteEvent ev, CancellationToken cancellationToken)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
            return Task.CompletedTask;
        }

        public void OnPostDelete(PostDeleteEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public Task OnPostInsertAsync(PostInsertEvent ev, CancellationToken cancellationToken)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
            return Task.CompletedTask;
        }

        public void OnPostInsert(PostInsertEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public Task OnPostUpdateCollectionAsync(PostCollectionUpdateEvent ev, CancellationToken cancellationToken)
        {
            DispatchEvents(ev.AffectedOwnerOrNull as AggregateRoot);
            return Task.CompletedTask;
        }

        public void OnPostUpdateCollection(PostCollectionUpdateEvent ev)
        {
            DispatchEvents(ev.AffectedOwnerOrNull as AggregateRoot);
        }

        private void DispatchEvents(AggregateRoot aggregateRoot)
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
