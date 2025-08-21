using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace BGD.Wave
{
    public struct WaveObject
    {
        public GameObject EnemyPrefab;
        public string wavePoint;
        public int Cnt;
    }

    [CreateAssetMenu(menuName ="SO/Wave/WaveData", fileName ="WaveDataSO")]
    public class WaveDataSO : ScriptableObject
    {
        public int waveCnt;
        public List<WaveObject> waveDatas;
    }
}
