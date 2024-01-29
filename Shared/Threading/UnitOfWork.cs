using Shared.Broker;
using Shared.MongoDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Threading
{
    public class UnitOfWork : IDisposable
    {
        public bool IsTransactional { get; private set; }
        public IMongoDBSession TransactionalSession { get; set; }
        public int MessageCount
        {
            get => _messages?.Count ?? 0;
        }

        private List<IMessage> _messages = new List<IMessage>();
        private Stopwatch _watch;

        public UnitOfWork(bool isTransactional = false)
        {
            IsTransactional = isTransactional;
            _watch = new Stopwatch();
            _watch.Start();
        }

        public async Task RollbackAsync()
        {
            if (TransactionalSession != null)
                await TransactionalSession.AbortTransactionAsync();

            DisposeTransactionalSession();
        }

        public async Task CommitAsync()
        {
            if (TransactionalSession != null)
                await TransactionalSession.CommitTransactionAsync();

            DisposeTransactionalSession();
        }

        public void PostMessage(IMessage message)
        {
            _messages.Add(message);
        }

        public TimeSpan StopTheWatch()
        {
            if (_watch.IsRunning)
                _watch.Stop();

            return _watch.Elapsed;
        }

        public void Dispose()
        {
            _messages?.Clear();
            DisposeTransactionalSession();
        }

        private void DisposeTransactionalSession()
        {
            TransactionalSession?.Dispose();
            TransactionalSession = null;
        }

        public IEnumerable<IMessage> GetMessages()
        {
            return _messages;
        }

        public void RemoveMessage(IMessage message)
        {
            _messages?.Remove(message);
        }
    }
}
