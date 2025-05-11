using MLAgent.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MLAgent.Entities.Components
{
    public class EntityState : MonoBehaviour, IEntityComponent, IAfterInitable
    {
        public StateBase CurrentState { get; private set; }

        [SerializeField] private EntityStateListSO _startStateList;
        [SerializeField] private StateSO _startState;

        public Action<string> OnStateChangedEvent;

        private Dictionary<string, StateBase> _stateDictionary;

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void AfterInit()
        {
            if (_stateDictionary == null)
            {
                _stateDictionary = new Dictionary<string, StateBase>();
                foreach (StateSO stateSO in _startStateList.states)
                {
                    string className = $"DKProject.FSM.{stateSO.StateTarget}{stateSO.StateName}State";
                    try
                    {
                        Type type = Type.GetType(className);
                        StateBase stateBase = Activator.CreateInstance(type, _entity) as StateBase;

                        _stateDictionary.Add(stateSO.StateName, stateBase);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"{className}\nError : {ex.ToString()}");
                    }
                }
            }

            ChangeState(_startState.StateName);
        }

        public void Dispose()
        {
            CurrentState?.Enter();
        }

        public void ChangeState(StateSO stateSO)
        {
            ChangeState(stateSO.StateName);
        }
        public void ChangeState(string stateName)
        {
            CurrentState?.Exit();
            CurrentState = _stateDictionary[stateName];
            CurrentState?.Enter();
            OnStateChangedEvent?.Invoke(stateName);
        }

        public void Update()
        {
            CurrentState?.Update();
        }
    }
}
