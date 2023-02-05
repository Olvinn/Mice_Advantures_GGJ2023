using Creatures;
using UnityEngine;

namespace Components
{
    public class EnemySpawnerComponent : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private int _count;

        private int _currentEnemyCount;
        
        private void Start()
        {
            Spawn();
        }

        private void EnemyRecount()
        {
            _currentEnemyCount--;
            
            if(_currentEnemyCount<=0)
                Spawn();
        }

        private void Spawn()
        {
            for (int i = 0; i < _count; i++)
            {
                var enemyInstance = Instantiate(_enemy, transform);
                enemyInstance.OnDead += EnemyRecount;
            }

            _currentEnemyCount = _count;
        }
    }
}