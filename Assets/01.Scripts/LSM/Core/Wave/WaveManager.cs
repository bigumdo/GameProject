using BGD.Agents;
using BGD.Agents.Enemies;
using BGD.ObjectPool;
using BGD.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BGD.Core
{
    public class WaveManager : MonoSingleton<WaveManager>
    {
        //public event Action Wave
        public bool IsNextWave { get; private set; } = true;

        [SerializeField] private List<WaveDataSO> _waveDatas;
        [SerializeField] private Transform [] _spawnPoints;
        private List<Enemy> _currentWaveEnemy;
        private Transform _spawnEnemyEmpty;
        private int _waveCount;

        private void Start()
        {
            StartWave();
        }

        public void StartWave()
        {
            _currentWaveEnemy = new List<Enemy>();
            StartCoroutine(WaveRoutine());
        }

        private IEnumerator WaveRoutine()
        {
            foreach(WaveDataSO wave in _waveDatas)
            {
                yield return new WaitUntil(() => IsNextWave);

                for (int i =0;i<wave.waveDatas.Count;++i)
                {
                    for(int j = 0;j < wave.waveDatas[i].spawnCount;++j)
                    {
                        Enemy enemy = PoolingManager.Instance.Pop(wave.waveDatas[i].enemyName) as Enemy;
                        _currentWaveEnemy.Add(enemy);
                        enemy.transform.position = _spawnPoints[wave.waveDatas[i].wavePoint + 1].position;
                        enemy.OnDeadAction += EnemyRemoveHandle;
                        yield return new WaitForSeconds(0.1f);
                    }
                }

                _waveCount++;
                if(_waveCount % 3 ==0)
                    IsNextWave = false;
                yield return new WaitUntil(() => _currentWaveEnemy.Count == 0);
            }
            yield return null;
        }

        private void EnemyRemoveHandle(Agent agent)
        {
            Enemy enemy = agent as Enemy;
            _currentWaveEnemy.Remove(enemy);
            enemy.OnDeadAction -= EnemyRemoveHandle;
        }
    }
}
