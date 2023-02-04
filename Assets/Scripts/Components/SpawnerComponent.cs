using System;
using UnityEngine;

namespace Components
{
    public class SpawnerComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _count;

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            for (int i = 0; i < _count; i++)
                Instantiate(_prefab, transform);
        }
    }
}