using Shared.Threading;
using Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Broker.DomainEvents
{
    internal class DomainEventBroker
    {
        private readonly IDomainEventProcessor _domainEventProcessor;

        public DomainEventBroker(IDomainEventProcessor domainEventProcessor)
        {
            Guard.AgainstNull(domainEventProcessor, nameof(domainEventProcessor), "Property_IsNull");
            _domainEventProcessor = domainEventProcessor;
        }

        public async Task PublishAllDomainEventsAsync()
        {
            if (Scope<UnitOfWork>.Current == null)
                return;

            var domainEvents = new List<IDomainEvent>();

            foreach (var message in Scope<UnitOfWork>.Current.GetMessages())
            {
                if (message is IDomainEvent @domainEvent)
                    domainEvents.Add(domainEvent);
            }

            if (domainEvents?.Any() == true)
                await _domainEventProcessor.ProcessDomainEventsAsync(domainEvents);
        }

        public async Task PublishDomainEventAsync(IDomainEvent message)
        {
            await _domainEventProcessor.ProcessDomainEventsAsync(new List<IDomainEvent> { message });
        }
    }
}
