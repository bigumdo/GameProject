using BGD.Animators;
using UnityEngine;

namespace BGD.Agents
{
    public class AgentRenderer : MonoBehaviour, IAgentComponent
    {
        private Agent _agent;
        private SpriteRenderer _renderer;
        private Animator _animator;

        public void Initialize(Agent agent)
        {
            _agent = agent;
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        public void SetParam(AnimParamSO param, bool value) => _animator.SetBool(param.hashValue, value);
        public void SetParam(AnimParamSO param, float value) => _animator.SetFloat(param.hashValue, value);
        public void SetParam(AnimParamSO param, int value) => _animator.SetInteger(param.hashValue, value);
        public void SetParam(AnimParamSO param) => _animator.SetTrigger(param.hashValue);

    }
}
