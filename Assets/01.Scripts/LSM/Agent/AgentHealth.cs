using UnityEngine;

namespace BGD.Agents
{
    public class AgentHealth : MonoBehaviour, IAgentComponent
    {
        public float maxhealth;

        private float _currentHealth;
        private Agent _agent;

        public void Initialize(Agent agent)
        {
            _agent = agent;
        }
    }
}
