using BGD.Agents;
using BGD.FSM;
using BGD.ObjectPool;
using System;
using UnityEngine;

namespace BGD.Agents.Enemies
{
    public class Enemy : Agent, IPoolable
    {
        [SerializeField]private AgentStateListSO _states;
        private StateMachine _stateMachine;
        private AgentHealth _health;

        [field:SerializeField] public string type { get; set; }

        public GameObject prefabObj => gameObject;

        protected override void InitComponent()
        {
            base.InitComponent();
            _stateMachine = new StateMachine(this, _states);
            _health = Getcompo<AgentHealth>();
        }

        protected override void AfterInitComponent()
        {
            base.AfterInitComponent();
            _stateMachine.Initialize("EnemyIdle");
        }

        public void ChangeState(string stateName)
        {
            _stateMachine.ChangeState(stateName);
        }

        private void Update()
        {
            _stateMachine.currentState.Update();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                _health.ApplyDamage(1000000000);
            }
        }

        public virtual void SetEnemy()
        {

        }

        public void ResetObj()
        {
            
        }
    }
}
