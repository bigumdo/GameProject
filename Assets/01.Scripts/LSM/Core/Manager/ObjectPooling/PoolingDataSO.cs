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
                    Debug.LogWarning("IPoolable�� �������� ���� ������Ʈ��");
                    prefab = null;
                    prefabObject = null;
                }
            }
        }
    }
}
