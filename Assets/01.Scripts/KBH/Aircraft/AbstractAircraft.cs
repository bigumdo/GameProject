using System;
using UnityEngine;

namespace Kbh
{
    public static class Collider2DOverlapUtil
    {
        private const int MAX_OVERLAP_COUNT = 100;
        private static Collider2D[] _overlapedCollider2D = new Collider2D[MAX_OVERLAP_COUNT];

        public static bool Overlap(this Collider2D collider, LayerMask mask, Action<Collider2D> Callback)
        {
            ContactFilter2D filter = new ContactFilter2D();

            filter.ClearDepth();
            filter.ClearNormalAngle();
            filter.SetLayerMask(mask);

            int cnt = collider.Overlap(filter, _overlapedCollider2D);

            if (Callback != null)
                for (int i = 0; i < cnt; ++i)
                    Callback(_overlapedCollider2D[i]);

            return cnt > 0;
        }
    }

    namespace Aircraft
    {
        public abstract class AbstractAircraft : MonoBehaviour
        {
            protected Action OnInit;
            protected Action OnDispose;

            [SerializeField] private SpriteRenderer _visual;
            [SerializeField] private Collider2D _collider;
            [SerializeField] private LayerMask _whatIsEnemy;
            private AircraftSO _data;
            

            private void Awake()
            {
                _visual = GetComponent<SpriteRenderer>();
                _collider = GetComponent<Collider2D>();
            }

            public void Init(AircraftSO data)
            {
                _data = data;
                ApplyData(_data);

                _visual.enabled = true;
                _collider.enabled = true;
                OnInit?.Invoke();
            }

            private void ApplyData(AircraftSO data)
            {
                _visual.sprite = data.Sprite;
                
            }

            private void FixedUpdate()
            {
                _collider.Overlap(_whatIsEnemy, HandleOverlaped);
            }

            private void HandleOverlaped(Collider2D collider)
            {
                // enemy가 닿았다면 데미지 입는 로직 
            }

            public void Dispose()
            {
                _visual.enabled = false;
                _collider.enabled = false;
                OnDispose?.Invoke();
            }
        }
    }
    

}
