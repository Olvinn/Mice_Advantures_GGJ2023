using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Creatures
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Creature
    {
        [SerializeField] private Transform _player;
        
        [SerializeField] private float _viewRadius;
        [SerializeField] private LayerCheck _attackCheck;

        private NavMeshAgent _navMeshAgent;

        private float Distance => Vector3.Distance(transform.position, _player.position);

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (Distance < _viewRadius)
                _navMeshAgent.SetDestination(_player.position);
        }

        public override void Attack()
        {
            throw new System.NotImplementedException();
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Vector3.Distance(transform.position, _player.position) < _viewRadius? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, _viewRadius);
        }
#endif
    }
}