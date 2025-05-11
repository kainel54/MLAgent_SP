using UnityEngine;

namespace MLAgent.FSM
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "SO/FSM/StateSO")]
    public class StateSO : ScriptableObject
    {
        public string StateTarget;
        public string StateName;
    }
}
