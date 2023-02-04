using System.Collections;
using Creatures.MobAI.Interfaces;
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
        [SerializeField] private CheckSphereOverlap _attackCheck;// Прокидывает raycast
        [SerializeField] private Patrol _patrol;
        [SerializeField] private Animator _animator;

        private NavMeshAgent _navMeshAgent;
        private Coroutine _patrolRoutine;

        private bool _isPatrolling; // Факинг флаг....

        private float Distance => Vector3.Distance(transform.position, _player.position);

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (Distance < _viewRadius)
            {
                _isPatrolling = false;
                _navMeshAgent.SetDestination(_player.position);
            }
            else
            {
                if (!_isPatrolling)
                {
                    _isPatrolling = true;
                    StartRoutine(ref _patrolRoutine, _patrol.DoPatrol());
                }
            }
            
            if(_attackCheck.IsTouching) Attack();
            
            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        }

        private void StartRoutine(ref Coroutine coroutine, IEnumerator enumerator)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(enumerator);
        }

        public override void Attack()
        {
            Debug.Log("Attacked!");
            _attackCheck.Check();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_player == null)
                return;
            Gizmos.color = Vector3.Distance(transform.position, _player.position) < _viewRadius
                ? Color.red
                : Color.green;
            Gizmos.DrawWireSphere(transform.position, _viewRadius);
        }
#endif
    }
}