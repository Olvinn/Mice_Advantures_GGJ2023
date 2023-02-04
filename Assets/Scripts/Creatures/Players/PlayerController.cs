using UnityEngine;

namespace Creatures.Players
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Creature
    {
        [SerializeField] private float _speed, _rotationSpeed;
    
        [SerializeField] private CharacterController _characterController;
        [SerializeField] protected Animator _animator;
        private CreatureState State { get; set; }

        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Rotate(Vector3 dir)
        {
            if (dir == Vector3.zero)
                return;
            var cur = Vector3.RotateTowards(transform.forward, dir, _rotationSpeed * Time.deltaTime, 1);
            transform.forward = cur;
        }
        

        private void Update()
        {
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Move(dir);
        }

        private void Move(Vector3 dir)
        {
            if (State == CreatureState.Attack)
                return;
            
            Vector3 forwardVector = Camera.main.transform.forward;
            forwardVector.y = 0;
            forwardVector.Normalize();
            
            Quaternion ang = Quaternion.FromToRotation(Vector3.forward, forwardVector);

            var res = ang * dir;
            
            _characterController.Move(res * _speed * Time.deltaTime);
            Rotate(res);
            _animator.SetFloat(Speed, res.magnitude * _speed);
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