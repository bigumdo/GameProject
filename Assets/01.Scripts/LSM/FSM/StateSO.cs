using BGD.Animators;
using UnityEngine;

namespace BGD.FSM
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "SO/FSM/StateSO")]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public AnimParamSO animParam;
    }
}
