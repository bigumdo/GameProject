using BGD.Agents.Enemies;
using BGD.Wave;
using System.Collections.Generic;
using UnityEngine;

namespace BGD.Core
{
    public class WaveManger : MonoSingleton<WaveManger>
    {
        public List<WaveDataSO> waveDatas;

        private List<Enemy> _currentWaveEnemy;
    }
}
