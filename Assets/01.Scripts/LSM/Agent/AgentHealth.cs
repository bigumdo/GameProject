using UnityEngine;

namespace BGD.Agents
{
    public class AgentHealth : MonoBehaviour, IAgentComponent
    {
        public float _maxhealth;

        private float _currentHealth;
        private Agent _agent;
        private AgentStat _agnetStat;


        public void Initialize(Agent agent)
        {
            _agnetStat = agent.Getcompo<AgentStat>();
            _currentHealth = _maxhealth = _agnetStat.GetStat("Hp").Value;
        }

        public void ApplyDamage(float damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxhealth);
            if (_currentHealth <= 0)
                _agent.OnDeadAction?.Invoke(_agent);
        }
    }
}
