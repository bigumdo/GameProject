using BGD.Agents.Enemies;
using BGD.ObjectPool;
using UnityEngine;

namespace BGD.Agents
{
    public class AgentHealth : MonoBehaviour, IAgentComponent
    {
        public float _maxhealth;

        protected float _currentHealth;
        protected Agent _agent;
        protected AgentStat _agnetStat;


        public virtual void Initialize(Agent agent)
        {
            _agnetStat = agent.Getcompo<AgentStat>();
            _currentHealth = _maxhealth = _agnetStat.GetStat("Hp").Value;
        }

        public virtual void ApplyDamage(float damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxhealth);
            if (_currentHealth <= 0)
            {
                _agent.OnDeadAction?.Invoke(_agent);
                //PoolingManager.Instance.Push(_agent);
            }
        }
    }
}
