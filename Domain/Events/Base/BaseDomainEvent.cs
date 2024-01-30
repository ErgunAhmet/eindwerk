using Domain.Entities;
using Domain.Resources;
using Shared.Broker.DomainEvents;
using Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Base
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        public string UpdatedBy { get; private set; }
        public DateTime UpdatedOn { get; private set; }

        public BaseDomainEvent(string updatedBy, DateTime updatedOn)
        {
            SetUpdatedBy(updatedBy);
            SetUpdatedOn(updatedOn);
        }

        private void SetUpdatedBy(string updatedBy)
        {
            if (string.IsNullOrEmpty(updatedBy))
                updatedBy = PropertyNameResource.System;

            UpdatedBy = updatedBy;
        }

        protected void SetUpdatedOn(DateTime updatedOn)
        {
            Guard.ThatIsTrue(updatedOn.Kind == DateTimeKind.Utc, nameof(updatedOn), nameof(ErrorCodeResource.Property_IncorrectValue));
            UpdatedOn = updatedOn;
        }

        public abstract IEnumerable<AuditLog> ToAuditLogs();
    }
}
