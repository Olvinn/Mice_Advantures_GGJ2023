using System.Collections;
using Components;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Creatures.Players
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Creature
    {
        [SerializeField] private float _speed, _rotationSpeed, _jumpPower = 10f;
    
        [SerializeField] private CharacterController _characterController;
        [SerializeField] protected Animator _animator;
        [SerializeField] private AudioController _audio;
        [SerializeField] private CheckSphereOverlap _attack;
        [SerializeField] private AttackAnimationEventCatcher _eventCathcer;
        [SerializeField] private ParticleSystem _dust;
        [SerializeField] private HealthComponent _hp;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Fight = Animator.StringToHash("Attack");
        private static readonly int block = Animator.StringToHash("Block");

        private CreatureState _state;
        private Vector3 _fallingSpeed;
        private bool _grounded;
        private float _actualSpeed;

        private Coroutine _audioCoroutine;

        private void Start()
        {
            _state = CreatureState.Fall;
            _audioCoroutine = StartCoroutine(AudioCoroutine());

            _eventCathcer.onHit = Hit;
            _eventCathcer.onEndAttack = AttackEnded;
            _eventCathcer.onSound = _audio.PlaySwordSlash;
        }

        private void OnDestroy()
        {
            _eventCathcer.onHit = null;
            _eventCathcer.onEndAttack = null;
            _eventCathcer.onSound = null;
        }

        private void Rotate(Vector3 dir)
        {
            if (dir == Vector3.zero || _state == CreatureState.Attack)
                return;
            var cur = Vector3.RotateTowards(transform.forward, dir, _rotationSpeed * Time.deltaTime, 1);
            transform.forward = cur;
        }

        private void Update()
        {
            //get data from last move which was actually about falling
            if (_characterController.isGrounded && !_grounded)
            {
                _audio.PlayGrounded();
                _state = CreatureState.Move;
                _dust.Play();
            }

            _grounded = _characterController.isGrounded;
            
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (dir.magnitude > 1) dir.Normalize();
            
            if (Input.GetButtonDown("Fire1"))
                Attack();

            if (Input.GetButton("Fire2"))
                Block();
            else if (_state == CreatureState.Block)
            {
                _state = CreatureState.Move;
                _animator.SetBool(block, false);
                _hp.isShielded = false;
            }

            Vector3 forwardVector = Camera.main.transform.forward;
            forwardVector.y = 0;
            forwardVector.Normalize();
            
            Quaternion ang = Quaternion.FromToRotation(Vector3.forward, forwardVector);

            var res = ang * dir;
            
            _actualSpeed = res.magnitude * _speed;
            
            Move(res);
            res = _state == CreatureState.Block ? forwardVector : res;
            Rotate(res);

            if (Input.GetButtonDown("Jump") && _grounded)
            {
                _dust.Stop();
                _audio.PlayJump();
                _fallingSpeed = Vector3.up * _jumpPower;
                _grounded = false;
            }

            //falling logic
            if (_grounded)
            {
                _fallingSpeed = Vector3.down;
                _characterController.Move(_fallingSpeed);
            }
            else
            {
                _dust.Stop();
                _state = CreatureState.Fall;
                _fallingSpeed += Physics.gravity * Time.deltaTime;
                _characterController.Move(_fallingSpeed * Time.deltaTime);
            }

            _animator.SetFloat(Speed, _actualSpeed);
            _animator.SetBool(Falling, !_grounded);
        }

        private void Move(Vector3 dir)
        {
            if (_state != CreatureState.Move && _state != CreatureState.Fall)
                return;
            
            _characterController.Move(dir * (_speed * Time.deltaTime));
        }

        public override void Attack()
        {
            if (_state == CreatureState.Attack)
                return;
            _state = CreatureState.Attack;
            _animator.SetTrigger(Fight);
        }

        public void Block()
        {
            if (_state == CreatureState.Block)
                return;
            _state = CreatureState.Block;
            _animator.SetBool(block, true);
            _hp.isShielded = true;
        }

        private void Hit()
        {
            _attack.SendDamageNotifications();
        }

        private void AttackEnded()
        {
            _state = CreatureState.Move;
        }

        IEnumerator AudioCoroutine()
        {
            float _delay = Random.Range(5, 10);
            while (true)
            {
                _delay -= Time.deltaTime;
                switch (_state)
                {
                    case CreatureState.Move:
                    {
                        if (_actualSpeed > 0.1f)
                            _audio.PlaySteps();
                        else
                            _audio.StopPlayingSteps();
                        
                        if (_delay > 0)
                            break;
                        _audio.PlayIdle();
                        _delay = Random.Range(10, 20);
                        break;
                    }
                    case CreatureState.Fall:
                    case CreatureState.Block:
                    case CreatureState.Attack:
                        _audio.StopPlayingSteps();
                        break;
                }

                yield return null;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
        }
#endif
    }
}