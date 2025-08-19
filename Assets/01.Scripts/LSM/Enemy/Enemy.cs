using BGD.Agents;
using BGD.FSM;
using UnityEngine;

namespace BGD.Agents.Enemies
{
    public class Enemy : Agent
    {

        [SerializeField]private AgentStateListSO _states;
        private StateMachine _stateMachine;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(this, _states);
        }

        protected override void InitComponent()
        {
            base.InitComponent();
            _stateMachine.Initialize("Idle");
        }

        public void ChangeState(string stateName)
        {
            _stateMachine.ChangeState(stateName);
        }

        private void Update()
        {
            _stateMachine.currentState.Update();
        }
    }
}
