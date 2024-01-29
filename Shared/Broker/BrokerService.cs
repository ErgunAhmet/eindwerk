using Shared.Broker.DomainEvents;
using Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Broker
{
    public static class BrokerService
    {
        private static DomainEventBroker _domainEventBroker;

        public static bool IsInitialized { get; set; }

        public static async Task PostMessageAsync(IMessage message)
        {
            Guard.AgainstNull(message, "Don't post <null> messages, please!");
            await PublishMessageAsync(message);
        }

        private static async Task PublishMessageAsync(IMessage message)
        {
            if (message is IDomainEvent domainEvent)
                await _domainEventBroker.PublishDomainEventAsync(domainEvent);
            else
                throw new NotImplementedException($"MessageType {message.GetType().FullName} not implemented");
        }

        public static async Task PublishAllDomainEventsAsync()
        {
            await _domainEventBroker.PublishAllDomainEventsAsync();
        }

        public static async Task SetupAsync(IServiceProvider serviceProvider)
        {
            var domainEventProcessor = serviceProvider.GetService(typeof(IDomainEventProcessor)) as IDomainEventProcessor;
            Guard.AgainstNull(domainEventProcessor, nameof(IDomainEventProcessor), "Config_NotFound");
            _domainEventBroker = new DomainEventBroker(domainEventProcessor);

            IsInitialized = true;
        }
    }
}
