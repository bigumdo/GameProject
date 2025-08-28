using BGD.Agents.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BGD.Agents
{
    public class Agent : MonoBehaviour
    {
        public Action<Agent> OnDeadAction;

        protected Dictionary<Type, IAgentComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IAgentComponent>();
            GetComponentsInChildren<IAgentComponent>(true).ToList()
                .ForEach(x => _components.Add(x.GetType(), x));

            InitComponent();
            AfterInitComponent();
        }

        protected virtual void InitComponent()
        {
            _components.Values.ToList().ForEach(x => x.Initialize(this));
        }

        protected virtual void AfterInitComponent()
        {
            _components.Values.ToList().ForEach(component =>
            {
                if (component is IAfterInit afterInit)
                {
                    afterInit.AfterInitialize();
                }
            });
        }

        public T Getcompo<T>(bool isDerived = false) where T : IAgentComponent
        {
            if (_components.TryGetValue(typeof(T), out IAgentComponent compo))
            {
                return (T)compo;
            }

            if (!isDerived)
                return default;

            Type findType = _components.Keys.FirstOrDefault(t => t.IsSubclassOf(typeof(T)));

            if (findType != null)
                return (T)_components[findType];

            return default;
        }

        public Coroutine StartDelayCallback(float delayTime, Action callback)
        {
            return StartCoroutine(DelayCoroutine(delayTime, callback));
        }

        private IEnumerator DelayCoroutine(float delayTime, Action callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke();
        }
    }
}
