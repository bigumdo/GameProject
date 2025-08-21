using System;
using Unity.VisualScripting;
using UnityEngine;

namespace BGD.Agents
{
    public class AgentMover : MonoBehaviour, IAgentComponent, IAfterInit
    {
        public bool CanMove { get; set; }

        protected Vector2 _movement;
        private Rigidbody2D _rbCompo;
        private float _speed;
        private Agent _agent;

        public void Initialize(Agent agent)
        {
            _agent = agent;
            _rbCompo = GetComponentInParent<Rigidbody2D>();
        }

        public void AfterInitialize()
        {
            _speed = _agent.Getcompo<AgentStat>().GetStat("Speed").Value;
            _agent.Getcompo<AgentStat>().GetStat("Speed").OnValueChange += HandleMoveSpeedChange;
        }

        private void OnDestroy()
        {
            _agent.Getcompo<AgentStat>().GetStat("Speed").OnValueChange -= HandleMoveSpeedChange;
        }

        private void HandleMoveSpeedChange(StatData statData, float changeValue)
        {
            _speed = changeValue;
        }

        public void SetMovement(Vector2 movement) => _movement = movement;

        protected virtual void FixedUpdate()
        {
            Move();
        }
        public void AddForce(Vector3 force, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            _rbCompo.AddForce(force, forceMode);
        }

        public virtual void StopImmediately()
        {
            _movement = Vector3.zero;
            _rbCompo.linearVelocity = Vector3.zero;
        }

        public void Move()
        {
            if(CanMove)
            {
                _rbCompo.linearVelocity = _movement * _speed;
            }
        }

    }
}
