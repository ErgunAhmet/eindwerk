using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Broker.DomainEvents
{
    public interface IDomainEventProcessor
    {
        Task ProcessDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents);
    }
}
