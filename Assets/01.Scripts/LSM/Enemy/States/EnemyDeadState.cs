using BGD.Animators;
using BGD.FSM;
using UnityEngine;

namespace BGD.Agents.Enemies
{
    public class EnemyDeadState : AgentState
    {
        public EnemyDeadState(Agent agent, AnimParamSO animParam) : base(agent, animParam)
        {
        }
    }
}
