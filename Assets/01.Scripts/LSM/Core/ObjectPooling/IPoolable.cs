using UnityEngine;

namespace BGD.ObjectPool
{
    public interface IPoolable
    {
        [SerializeField] public string type { get; set; }
        public GameObject prefabObj { get; }
        public void ResetObj();
    }
}
