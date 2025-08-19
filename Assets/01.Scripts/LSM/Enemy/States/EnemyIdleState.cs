using BGD.Animators;
using BGD.FSM;
using UnityEngine;

namespace BGD.Agents.Enemies
{
    public class EnemyIdleState : AgentState
    {
        private Enemy _enemy;
        public EnemyIdleState(Agent agent, AnimParamSO animParam) : base(agent, animParam)
        {
            _enemy = agent as Enemy;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
