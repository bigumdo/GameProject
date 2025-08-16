using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BGD.Agents
{
    public struct StatData
    {
        public delegate void ValueChangeHandler(StatData statData, float changeValue);
        public event ValueChangeHandler OnValueChange;

        public string statName;
        [SerializeField] private float _baseValue, _maxValue, _minValue;
        public float Value
        {
            get => _baseValue;
            set
            {
                if (value < _minValue)
                    _baseValue = _minValue;
                else if (value > _maxValue)
                    _baseValue = _maxValue;
                else
                    _baseValue = value;
                OnValueChange?.Invoke(this, value);
            }
        }
    }

    public class AgentStat : MonoBehaviour, IAgentComponent, IAfterInit
    {
        [SerializeField] private List<StatData> _statDatas;

        private Agent _agent;
        private Dictionary<string, StatData> _statDataDictionary;

        public void Initialize(Agent agent)
        {
            _agent = agent;
            _statDataDictionary = new Dictionary<string, StatData>();
        }
        public void AfterInitialize()
        {
            _statDatas.ForEach(x => _statDataDictionary.Add(x.statName, x));
        }

        public StatData GetStat(string statName)
        {
            if(_statDataDictionary.TryGetValue(statName, out StatData statData))
            {
                return statData;
            }

            Debug.LogError($"{statName}이 잘못 되었거나, stat이 없음");
            return default;
        }
    }
}
