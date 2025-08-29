using BGD.ObjectPool;
using UnityEngine;

namespace BGD.Agents.Enemies
{
    public class EnemyHealth : AgentHealth
    {
        private Enemy _enemy;

        public override void Initialize(Agent agent)
        {
            base.Initialize(agent);
            _enemy = agent as Enemy;
        }

        public override void ApplyDamage(float damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxhealth);
            if (_currentHealth <= 0)
            {
                _enemy.OnDeadAction?.Invoke(_enemy);
                PoolingManager.Instance.Push(_enemy);
            }
        }
    }
}
