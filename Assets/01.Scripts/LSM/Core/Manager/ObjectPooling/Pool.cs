using BGD.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace BGD
{
    public class Pool
    {
        private Queue<IPoolableObject> _pool = new Queue<IPoolableObject>();
        private IPoolableObject _prefab;
        private Transform _parent;

        private string _type;

        public Pool(IPoolableObject prefab, string type, Transform parent, int count)
        {
            _prefab = prefab;
            _type = type;
            _parent = parent;

            for (int i = 0; i < count; i++)
            {
                GameObject obj = GameObject.Instantiate(_prefab.prefabObj, _parent);
                obj.gameObject.name = _type.ToString();
                obj.gameObject.SetActive(false);
                IPoolableObject poolObj = obj.GetComponent<IPoolableObject>();
                poolObj.type = _type;
                _pool.Enqueue(poolObj);
            }
        }


        public IPoolableObject Pop()
        {
            IPoolableObject poolObj;
            GameObject obj = null;
            if (_pool.Count <= 0)
            {
                obj = GameObject.Instantiate(_prefab.prefabObj, _parent);
                obj.gameObject.name = _type.ToString();
                poolObj = obj.GetComponent<IPoolableObject>();
                poolObj.type = _type;
            }
            else
            {
                poolObj = _pool.Dequeue();
                poolObj.prefabObj.SetActive(true);
            }
            return poolObj;
        }

        public void Push(IPoolableObject obj)
        {
            obj.prefabObj.SetActive(false);
            _pool.Enqueue(obj);
        }

    }
}
