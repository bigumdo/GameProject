using BGD.ObjectPool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BGD.Agents.Enemies
{
    public class EnemyHealth : AgentHealth
    {
        [SerializeField] private Transform _hpBar;
        
        private Enemy _enemy;

        public override void Initialize(Agent agent)
        {
            base.Initialize(agent);
            _enemy = agent as Enemy;
            ResetHpBar();
        }

        public override void ApplyDamage(float damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxhealth);
            HpBar();
            if (_currentHealth <= 0)
            {
                _enemy.OnDeadAction?.Invoke(_enemy);
                PoolingManager.Instance.Push(_enemy);
            }
        }

        public void HpBar()
        {
            _hpBar.transform.localScale = new Vector3(Mathf.Lerp(0, 1, _currentHealth / _maxhealth), 1, 1);
        }

        public void ResetHpBar()
        {
            _hpBar.transform.localScale = Vector3.one;
        }
    }
}
