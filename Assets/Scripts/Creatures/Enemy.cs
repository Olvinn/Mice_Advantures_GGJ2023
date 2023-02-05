using System;
using System.Collections;
using Components;
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
        private GameObject _tree;

        private IEnumerator _current;

        private NavMeshAgent _navMeshAgent;
        private Coroutine _attackRoutine;

        private static readonly int SpeedKey = Animator.StringToHash("Speed");
        private static readonly int AttackKey = Animator.StringToHash("Attack");

        private float Distance => Vector3.Distance(transform.position, _player.transform.position);

        public event Action OnDead;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _player = FindObjectOfType<PlayerController>().gameObject;
            _tree = GameObject.FindGameObjectWithTag("Tree");
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

                    if (_attackCheck.IsTouching) yield return Attacking();

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
                    _navMeshAgent.SetDestination(_tree
                        .transform.position);

                    if (_attackCheck.IsTouching) yield return Attacking();
                    yield return new WaitForSeconds(0.28f);
                }
            }

            StartState(AgroToPlayer());
        }

        private IEnumerator Attacking()
        {
            _navMeshAgent.isStopped = true;
            Attack();
            _animator.SetTrigger(AttackKey);

            yield return new WaitForSeconds(2);

            _navMeshAgent.isStopped = false;
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

        private void OnDestroy()
        {
            OnDead?.Invoke();
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