using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public float speed, rotationSpeed;
    
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
    
        public PlayerState state { get; private set; }
        private float _fallSpeed;

        private void Update()
        {
            if (!characterController.isGrounded)
                state = PlayerState.Falling;

            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            
            Vector3 forwardVector = Camera.main.transform.forward;
            forwardVector.y = 0;
            forwardVector.Normalize();
            
            Quaternion ang = Quaternion.FromToRotation(Vector3.forward, forwardVector);

            var res = ang * dir;
            
            switch (state)
            {
                case PlayerState.Move:
                    _fallSpeed = 1;
                    Move(Vector3.down);
                    Move(res);
                    Rotate(res);
                    break;
                case PlayerState.Falling:
                    _fallSpeed += Physics.gravity.magnitude * Time.deltaTime;
                    Move(Vector3.down * _fallSpeed);
                    Move(res);
                    Rotate(res);
                    break;
            }
        }

        public void Move(Vector3 dir)
        {
            if (state == PlayerState.Attack)
                return;
            
            characterController.Move(dir * (speed * Time.deltaTime));
            animator.SetFloat("Speed", dir.magnitude * speed);
        }

        public void Rotate(Vector3 dir)
        {
            if (dir == Vector3.zero)
                return;
            Vector3 cur = Vector3.RotateTowards(transform.forward, dir, rotationSpeed * Time.deltaTime, 1);
            transform.forward = cur;
        }

        public void Attack()
        {
        
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }
#endif
    }

    public enum PlayerState
    {
        Move,
        Attack,
        Falling
    }
}