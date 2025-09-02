using System;
using System.Collections.Generic;
using UnityEngine;

namespace BGD.Wave
{
    [Serializable]
    public struct WaveObject
    {
        public string enemyName;
        public float startDelay;
        public int wavePoint;
        public int spawnCount;
        public float spawnDelay;
    }

    [CreateAssetMenu(menuName ="SO/Wave/WaveData", fileName ="WaveDataSO")]
    public class WaveDataSO : ScriptableObject
    {
        public List<WaveObject> waveDatas;
        public float nextWaveDelayTime;
    }
}
