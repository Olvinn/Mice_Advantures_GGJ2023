using System;
using System.Collections;
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

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Fight = Animator.StringToHash("Attack");

        private CreatureState _state;
        private Vector3 _fallingSpeed;
        private bool _grounded;
        private float _actualSpeed;

        private Coroutine _audioCoroutine;

        private void Start()
        {
            _state = CreatureState.Fall;
            _audioCoroutine = StartCoroutine(AudioCoroutine());
        }

        private void Rotate(Vector3 dir)
        {
            if (dir == Vector3.zero)
                return;
            var cur = Vector3.RotateTowards(transform.forward, dir, _rotationSpeed * Time.deltaTime, 1);
            transform.forward = cur;
        }

        private void Update()
        {
            _state = CreatureState.Move;
            //get data from last move which was actually about falling
            if (_characterController.isGrounded && !_grounded)
                _audio.PlayGrounded();
            _grounded = _characterController.isGrounded;
            
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (dir.magnitude > 1) dir.Normalize();
            
            if (Input.GetButtonDown("Fire1"))
                Attack();
            
            Vector3 forwardVector = Camera.main.transform.forward;
            forwardVector.y = 0;
            forwardVector.Normalize();
            
            Quaternion ang = Quaternion.FromToRotation(Vector3.forward, forwardVector);

            var res = ang * dir;
            
            Move(res);
            Rotate(res);

            if (Input.GetButtonDown("Jump") && _grounded)
            {
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
                _state = CreatureState.Fall;
                _fallingSpeed += Physics.gravity * Time.deltaTime;
                _characterController.Move(_fallingSpeed * Time.deltaTime);
            }

            _actualSpeed = res.magnitude * _speed;
            _animator.SetFloat(Speed, _actualSpeed);
            _animator.SetBool(Falling, !_grounded);
        }

        private void Move(Vector3 dir)
        {
            if (_state == CreatureState.Attack)
                return;
            
            _characterController.Move(dir * (_speed * Time.deltaTime));
        }

        public override void Attack()
        {
            _state = CreatureState.Attack;
            _animator.SetTrigger(Fight);
            _attack.Check();
            _audio.PlaySwordSlash();
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
                        if (_actualSpeed > 0.5f)
                            _audio.PlaySteps();
                        else
                        {
                            _audio.StopLoops();
                            if (_delay > 0)
                                break;
                            _audio.PlayIdle();
                            _delay = Random.Range(5, 10);
                        }
                        break;
                    }
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