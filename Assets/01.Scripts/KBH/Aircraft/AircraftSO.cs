using System;
using UnityEngine;

namespace Kbh.Aircraft
{
    [Flags]
    public enum EBulbAttackFlags
    {
        None = 0,
        Guided = 1,
        Bomb = 2,
    }

    public enum EBulbAlignType
    {
        One = 0,
        Gap = 1,
        Radial = 2,
    }

    public enum EBulbMoveType
    {
        Rush = 0,
        bezier = 1,
        Throw = 2,
    }

    [System.Serializable]
    public struct AircraftBulbData
    {
        public AbstractAircraft prefab;
        public int count;
        public float coolTime;

        [Space]
        public EBulbAttackFlags attackFlags;

        [Space]
        public EBulbAlignType alignType;
        public float alignParam;

        [Space]
        public EBulbMoveType moveType;
        public float moveParam;
    }


    [CreateAssetMenu(menuName = "Scriptable Object/Aircraft")]
    public class AircraftSO : ScriptableObject
    {
        [field:SerializeField] public string AircraftName { get; private set; }
        [field:SerializeField] public string AircraftDesc { get; private set; }
        [field:SerializeField] public Sprite Sprite { get; private set; }
        
        [field:Space]
        [field:SerializeField] public AircraftBulbData[] BulbData { get; private set; }
    }
}
