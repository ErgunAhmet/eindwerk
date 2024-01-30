using Domain.Entities;
using Domain.Events.Base;
using IRepository;
using Shared.Broker.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DomainEventProcessor : IDomainEventProcessor
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public DomainEventProcessor(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task ProcessDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            if (domainEvents?.Any() != true)
                return;

            var auditLogs = new List<AuditLog>();

            foreach (var domainEvent in domainEvents)
            {
                if (domainEvent is BaseDomainEvent baseEvent)
                {
                    var domainEventAuditLogs = baseEvent.ToAuditLogs();

                    if (domainEventAuditLogs?.Any() == true)
                        auditLogs.AddRange(domainEventAuditLogs);
                }
                else
                {
                    throw new NotImplementedException($"{nameof(IDomainEvent)} {domainEvent.GetType().FullName} base class is not of type {typeof(BaseDomainEvent).FullName}");
                }
            }

            if (auditLogs?.Any() == true)
                await _auditLogRepository.BatchInsertAsync(auditLogs);
        }
    }
}
