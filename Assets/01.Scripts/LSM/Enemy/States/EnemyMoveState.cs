using BGD.Animators;
using BGD.FSM;
using UnityEngine;

namespace BGD.Agents.Enemies
{
    public class EnemyMoveState : AgentState
    {
        private Enemy _enemy;
        private AgentMover _mover;

        public EnemyMoveState(Agent agent, AnimParamSO animParam) : base(agent, animParam)
        {
            _enemy = agent as Enemy;
            _mover = agent.Getcompo<AgentMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.CanMove = true;
            _mover.SetMovement(Vector2.down);
        }

        public override void Exit()
        {
            _mover.CanMove = false;
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
