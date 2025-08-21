using BGD.ObjectPool;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BGD.ObjectPool
{
    [System.Serializable]
    public struct PoolingSetting
    {
        public string typeName;
        public string poolingName;
        public GameObject prefabObject;
        public IPoolableObject prefab;
        public int poolingSettingCnt;
    }

    [CreateAssetMenu(fileName = "PoolingTableSO", menuName = "SO/PoolingTableSO")]
    public class PoolingTableSO : ScriptableObject
    {
        public List<PoolingSetting> datas = new List<PoolingSetting>();
    }
}
