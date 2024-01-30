using Domain.Entities.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Base
{
    public abstract class RelationshipChangedDomainEvent<T> : BaseDomainEvent where T : IEntity
    {
        private T _entity;
        private string _name;
        private string _parentName;

        public string ObjectName { get; private set; }
        public List<DomainRelationShipChanged> Changes { get; private set; }
        protected bool LinkToParent { get; set; } = true;

        public RelationshipChangedDomainEvent(T entity, string name)
            : this(entity, name, null)
        {
        }


        public RelationshipChangedDomainEvent(T entity, string name, string parentName)
            : this(entity, name, parentName, null)
        {
        }


        public RelationshipChangedDomainEvent(T entity, string name, string parentName, string updatedBy)
            : base(updatedBy, DateTime.UtcNow)
        {
            _entity = entity;
            _name = name;
            _parentName = parentName;
            Changes = new List<DomainRelationShipChanged>();
            ObjectName = typeof(T).Name;
        }

        public void AddRelationShipChanged<TChanged>(TChanged oldValue, TChanged newValue) where TChanged : IEntity
        {
            Changes.Add(DomainRelationShipChanged.Create(_entity, _name, oldValue, newValue));

            if (LinkToParent)
            {
                if (oldValue == null && newValue != null)
                    Changes.Add(DomainRelationShipChanged.Create(newValue, _parentName, null, _entity));
                else if (oldValue != null && newValue == null)
                    Changes.Add(DomainRelationShipChanged.Create(oldValue, _parentName, _entity, null));
                else if (oldValue != null && newValue != null)
                {
                    Changes.Add(DomainRelationShipChanged.Create(oldValue, _parentName, _entity, null));
                    Changes.Add(DomainRelationShipChanged.Create(newValue, _parentName, null, _entity));
                }
            }
        }

        public override IEnumerable<AuditLog> ToAuditLogs()
        {
            return AuditLog.CreateFromRelationshipChangedEvent(this) ?? new List<AuditLog>();
        }
    }
}
