using UnityEngine;

namespace BGD.ObjectPool
{
    [CreateAssetMenu(fileName = "PoolingDataSO", menuName = "SO/Pool/PoolingDataSO")]
    public class PoolingDataSO : ScriptableObject
    {
        public string typeName;
        public GameObject prefabObject;
        public IPoolable prefab;
        public int poolingSettingCnt;

        private void OnValidate()
        {
            if(prefabObject != null)
            {
                if(prefabObject.TryGetComponent(out IPoolable poolable))
                {
                    prefab = poolable;
                }
                else
                {
                    Debug.LogWarning("IPoolable을 구현하지 않은 오브젝트임");
                    prefab = null;
                    prefabObject = null;
                }
            }
        }
    }
}
