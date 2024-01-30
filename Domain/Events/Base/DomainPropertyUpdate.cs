using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Base
{
    public class DomainPropertyUpdate
    {
        public string Name { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }

        private DomainPropertyUpdate() { }

        public static DomainPropertyUpdate Create(string propertyName, string oldValue, string newValue)
        {
            return new DomainPropertyUpdate
            {
                Name = propertyName,
                OldValue = oldValue,
                NewValue = newValue
            };
        }
    }
}
