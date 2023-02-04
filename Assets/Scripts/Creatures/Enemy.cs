using Creatures.Players;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Creatures
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Creature
    {
        [SerializeField] private float _viewRadius;
        [SerializeField] private CheckSphereOverlap _attackCheck; // Прокидывает raycast
        [SerializeField] private Animator _animator;

        private GameObject _player;
        private GameObject _tree;

        private NavMeshAgent _navMeshAgent;
        private Coroutine _patrolRoutine;

        private bool _isPatrolling; // Факинг флаг....

        private float Distance => Vector3.Distance(transform.position, _player.transform.position);

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _player = FindObjectOfType<PlayerController>().gameObject;
            _tree = GameObject.FindGameObjectWithTag("Tree");
        }

        private void FixedUpdate()
        {
            if (Distance < _viewRadius)
            {
                _isPatrolling = false;
                _navMeshAgent.SetDestination(_player.transform.position);
            }
            else
            {
                if (!_isPatrolling)
                {
                    _isPatrolling = true;
                    _navMeshAgent.SetDestination(_tree.transform.position);
                }
            }

            if (_attackCheck.IsTouching) Attack();

            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        }

        /*private void StartRoutine(ref Coroutine coroutine, IEnumerator enumerator)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(enumerator);
        }*/

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
            Gizmos.color = Vector3.Distance(transform.position, _player.transform.position) < _viewRadius
                ? Color.red
                : Color.green;
            Gizmos.DrawWireSphere(transform.position, _viewRadius);
        }
#endif
    }
}