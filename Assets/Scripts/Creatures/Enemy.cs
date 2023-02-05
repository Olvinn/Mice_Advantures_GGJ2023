using System.Collections;
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
        [SerializeField] private AudioController _audio;

        private GameObject _player;
        private GameObject[] _treeComponents;

        private IEnumerator _current;

        private NavMeshAgent _navMeshAgent;
        private Coroutine _attackRoutine;

        private static readonly int SpeedKey = Animator.StringToHash("Speed");
        private static readonly int AttackKey = Animator.StringToHash("Attack");

        private float Distance => Vector3.Distance(transform.position, _player.transform.position);

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _player = FindObjectOfType<PlayerController>().gameObject;
            _treeComponents = GameObject.FindGameObjectsWithTag("Tree");
        }

        private void Start()
        {
            StartState(WalkToTheTree());
        }

        private IEnumerator AgroToPlayer()
        {
            if (_player != null)
            {
                while (Distance < _viewRadius)
                {
                    if (_player == null)
                        break;
                    _navMeshAgent.SetDestination(_player.transform.position);

                    if (_attackCheck.IsTouching) StartState(Attacking());

                    yield return new WaitForSeconds(0.28f);
                }
            }

            StartState(WalkToTheTree());
        }

        private IEnumerator WalkToTheTree()
        {
            if (_player != null)
            {
                while (Distance >= _viewRadius)
                {
                    if (_player == null)
                        break;
                    _navMeshAgent.SetDestination(_treeComponents[0].transform.position);
                    yield return new WaitForSeconds(0.28f);

                    if (_navMeshAgent.velocity.magnitude <= 1) StartState(Attacking());
                }
            }

            StartState(AgroToPlayer());
        }

        private IEnumerator Attacking()
        {
            while (_attackCheck.IsTouching)
            {
                _navMeshAgent.isStopped = true;
                Attack();
                _animator.SetTrigger(AttackKey);

                yield return new WaitForSeconds(1.2f);
            }

            yield return new WaitForSeconds(2f);
            _navMeshAgent.isStopped = false;

            StartState(AgroToPlayer());
        }

        private void Update()
        {
            _animator.SetFloat(SpeedKey, _navMeshAgent.velocity.magnitude);
            if (_navMeshAgent.velocity.magnitude > .1f)
                _audio.PlaySteps();
            else
                _audio.StopPlayingSteps();
        }

        private void StartState(IEnumerator coroutine)
        {
            if (_current != null)
                StopCoroutine(_current);

            _current = coroutine;
            StartCoroutine(coroutine);
        }

        public override void Attack()
        {
            _attackCheck.SendDamageNotifications();
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