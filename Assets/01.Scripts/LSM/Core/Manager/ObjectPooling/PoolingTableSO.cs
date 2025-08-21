using BGD.ObjectPool;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BGD.ObjectPool
{
    [CreateAssetMenu(fileName = "PoolingTableSO", menuName = "SO/Pool/PoolingTableSO")]
    public class PoolingTableSO : ScriptableObject
    {
        public List<PoolingDataSO> datas = new List<PoolingDataSO>();
    }
}
