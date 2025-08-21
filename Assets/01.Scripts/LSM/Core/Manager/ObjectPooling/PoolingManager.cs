using BGD.Core;
using System.Collections.Generic;
using UnityEngine;

namespace BGD.ObjectPool
{
    public class PoolingManager : MonoSingleton<PoolingManager>
    {
        private Dictionary<string, Pool> _pools
                        = new Dictionary<string, Pool>();

        public PoolingTableSO listSO;

        private void Awake()
        {
            foreach (PoolingSetting item in listSO.datas)
            {
                CreatePool(item);
            }
        }

        private void CreatePool(PoolingSetting item)
        {
            var pool = new Pool(item.prefab, item.typeName, transform, item.poolingSettingCnt);
            _pools.Add(item.prefab.type, pool);
        }


        public IPoolableObject Pop(string type)
        {
            if (_pools.ContainsKey(type) == false)
            {
                Debug.LogError($"Prefab does not exist on pool : {type.ToString()}");
                return null;
            }

            IPoolableObject item = _pools[type].Pop();
            item.ResetObj();
            return item;
        }

        public void Push(IPoolableObject obj, bool resetParent = false)
        {
            if (resetParent)
                obj.prefabObj.transform.SetParent(transform);
            _pools[obj.type].Push(obj);
        }
    }
}
