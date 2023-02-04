using System;
using UnityEngine;

namespace Creatures.Players
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Creature
    {
        [SerializeField] private float _speed, _rotationSpeed, _jumpPower = 10f;
    
        [SerializeField] private CharacterController _characterController;
        [SerializeField] protected Animator _animator;
        private CreatureState State { get; set; }

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Falling = Animator.StringToHash("Falling");

        private Vector3 _fallingSpeed;

        private void Rotate(Vector3 dir)
        {
            if (dir == Vector3.zero)
                return;
            var cur = Vector3.RotateTowards(transform.forward, dir, _rotationSpeed * Time.deltaTime, 1);
            transform.forward = cur;
        }

        private void Update()
        {
            //get data from last move which was actually about falling
            bool grounded = _characterController.isGrounded;
            
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            
            Vector3 forwardVector = Camera.main.transform.forward;
            forwardVector.y = 0;
            forwardVector.Normalize();
            
            Quaternion ang = Quaternion.FromToRotation(Vector3.forward, forwardVector);

            var res = ang * dir;
            
            Move(res);
            Rotate(res);

            if (Input.GetButtonDown("Jump") && grounded)
            {
                _fallingSpeed = Vector3.up * _jumpPower;
                grounded = false;
            }

            //falling logic
            if (grounded)
            {
                _fallingSpeed = Vector3.down;
                _characterController.Move(_fallingSpeed);
            }
            else
            {
                _fallingSpeed += Physics.gravity * Time.deltaTime;
                _characterController.Move(_fallingSpeed * Time.deltaTime);
            }

            _animator.SetFloat(Speed, res.magnitude * _speed);
            _animator.SetBool(Falling, !grounded);
        }

        private void Move(Vector3 dir)
        {
            if (State == CreatureState.Attack)
                return;
            
            _characterController.Move(dir * (_speed * Time.deltaTime));
        }

        public override void Attack()
        {
        
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