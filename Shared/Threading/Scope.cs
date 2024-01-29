using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Threading
{
    public class Scope<T> : IDisposable
    {
        private static AsyncLocal<Scope<T>> _instance = new AsyncLocal<Scope<T>>();
        private T _value;

        private bool _disposed;
        private readonly bool _toDispose;
        private readonly Scope<T> _parent;

        protected Scope()
        {
        }

        public Scope(T instance)
          : this(instance, true)
        {
        }

        public Scope(T instance, bool toDispose)
        {
            _value = instance;
            _parent = _instance.Value;
            _instance.Value = this;
            _toDispose = toDispose;
        }

        public static T Current
        {
            get
            {
                if (_instance.Value != null)
                    return _instance.Value._value;

                return default;
            }
            set
            {
                _instance.Value._value = value;
            }
        }

        protected T ParentValue
        {
            get
            {
                if (_parent != null)
                    return _parent._value;

                return default;
            }
        }

        protected static Scope<T> Ambient
        {
            get
            {
                return _instance.Value;
            }
        }

        public virtual void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                if (_toDispose)
                    (Current as IDisposable)?.Dispose();
            }

            _instance.Value = _parent;
        }
    }
}
