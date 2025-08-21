using UnityEngine;

namespace BGD.ObjectPool
{
    public interface IPoolableObject
    {
        [SerializeField] public string type { get; set; }
        public GameObject prefabObj { get; }
        public void ResetObj();
    }
}
