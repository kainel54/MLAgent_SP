using MLAgent.Entities;
using MLAgent.Entities.Components;
using System;

namespace MLAgent.FSM
{
    [Flags]
    public enum EAnimationEventType
    {
        Start = 1,
        End = 2,
        Trigger = 4,
    }

    [Serializable]
    public abstract class StateBase
    {
        protected Entity _entity;
        protected EntityState _entityState;
        
        private EAnimationEventType _isTriggerCall;

        public StateBase(Entity entity)
        {
            _entity = entity;
            _entityState = entity.GetCompo<EntityState>();
        }

        public virtual void Enter()
        {
            _isTriggerCall = 0;
        }

        protected virtual void HandleAnimationEvent(EAnimationEventType type)
        {
            AddTriggerCall(type);
        }
        public void AddTriggerCall(EAnimationEventType type) => _isTriggerCall |= type;
        public bool HasTriggerCall(EAnimationEventType type) => _isTriggerCall.HasFlag(type);
        public void RemoveTriggerCall(EAnimationEventType type) => _isTriggerCall &= ~type;

        public virtual void Update() { }

        public virtual void Exit()
        {
            
        }
    }
}
