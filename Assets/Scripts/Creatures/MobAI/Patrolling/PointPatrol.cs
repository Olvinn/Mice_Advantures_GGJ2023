using System.Collections;
using Creatures.MobAI.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures.MobAI.Patrolling
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PointPatrol : Patrol
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _remainingOffset;

        private int _pointIndex;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled) // Да херня полная, согласен...
            {
                _agent.SetDestination(_points[_pointIndex].position);
                yield return null;
                
                if (!(_agent.remainingDistance <= _remainingOffset)) continue;
                _pointIndex = (int)Mathf.Repeat(_pointIndex + 1, _points.Length);
            }
        }
    }
}