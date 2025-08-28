using System;
using System.Collections.Generic;
using UnityEngine;

namespace BGD.Wave
{
    [Serializable]
    public struct WaveObject
    {
        public string enemyName;
        public int wavePoint;
        public int spawnCount;
    }

    [CreateAssetMenu(menuName ="SO/Wave/WaveData", fileName ="WaveDataSO")]
    public class WaveDataSO : ScriptableObject
    {
        public List<WaveObject> waveDatas;
    }
}
