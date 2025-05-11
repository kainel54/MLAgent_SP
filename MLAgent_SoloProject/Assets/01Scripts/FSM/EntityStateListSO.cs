using System;
using System.Collections.Generic;
using UnityEngine;

namespace MLAgent.FSM
{
    public static class StateName
    {
        public readonly static string Idle = "Idle";
        public readonly static string Move = "Move";
        public readonly static string Jump = "Jump";
        public readonly static string Die = "Die";
    }

    [CreateAssetMenu(fileName = "EntityStateListSO", menuName = "SO/FSM/EntityStateList")]
    public class EntityStateListSO : ScriptableObject
    {
        public List<StateSO> states;
    }
}
