using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MLAgent.Entities
{
    public class Entity : MonoBehaviour
    {
        public virtual void OnDie()
        {

        }

        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            FindComponents();
            InitComponents();
            AfterInitComponents();
        }

        protected virtual void FindComponents()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            GetComponentsInChildren<IEntityComponent>(true).ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }

        protected virtual void InitComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }

        protected virtual void AfterInitComponents()
        {
            _components.Values.ToList().ForEach(component =>
            {
                if (component is IAfterInitable afterInitable)
                {
                    afterInitable.AfterInit();
                }
            });
        }

        protected virtual void DisposeComponents()
        {
            _components.Values.ToList().ForEach(component =>
            {
                if (component is IAfterInitable disposeable)
                {
                    disposeable.Dispose();
                }
            });
        }

        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
            {
                return (T)component;
            }
            
            if(isDerived == false)
                return default;

            Type findType = _components.Keys.FirstOrDefault(t => t.IsSubclassOf(typeof(T)));
            if(findType != null) 
                return (T)_components[findType];
            
            return default;
        }

        protected virtual void OnDestroy()
        {
            DisposeComponents();
        }
    }
}
