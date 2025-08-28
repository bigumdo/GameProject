using BGD.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace BGD
{
    public class Pool
    {
        private Queue<IPoolable> _pool = new Queue<IPoolable>();
        private IPoolable _prefab;
        private Transform _parent;

        private string _type;

        public Pool(IPoolable prefab, string type, Transform parent, int count)
        {
            _prefab = prefab;
            _type = type;
            _parent = parent;

            for (int i = 0; i < count; i++)
            {
                GameObject obj = GameObject.Instantiate(_prefab.prefabObj, _parent);
                obj.gameObject.name = _type.ToString();
                obj.gameObject.SetActive(false);
                IPoolable poolObj = obj.GetComponent<IPoolable>();
                poolObj.type = _type;
                _pool.Enqueue(poolObj);
            }
        }


        public IPoolable Pop()
        {
            IPoolable poolObj;
            GameObject obj = null;
            if (_pool.Count <= 0)
            {
                obj = GameObject.Instantiate(_prefab.prefabObj, _parent);
                obj.gameObject.name = _type.ToString();
                poolObj = obj.GetComponent<IPoolable>();
                poolObj.type = _type;
            }
            else
            {
                poolObj = _pool.Dequeue();
                poolObj.prefabObj.SetActive(true);
            }
            return poolObj;
        }

        public void Push(IPoolable obj)
        {
            obj.prefabObj.SetActive(false);
            _pool.Enqueue(obj);
        }

    }
}
