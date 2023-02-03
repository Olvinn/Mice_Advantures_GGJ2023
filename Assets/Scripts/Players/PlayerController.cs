using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed;
    
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    
    public PlayerState state { get; private set; }

    private void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Move(dir);
    }

    public void Move(Vector3 dir)
    {
        if (state == PlayerState.Attack)
            return;
            
        Vector3 forwardVector = Camera.main.transform.forward;
        forwardVector.y = 0;
        forwardVector.Normalize();
            
        Quaternion ang = Quaternion.FromToRotation(Vector3.forward, forwardVector);

        var res = ang * dir;
            
        characterController.Move(res * speed * Time.deltaTime);
        Rotate(res);
        animator.SetFloat("Speed", res.magnitude * speed);
    }

    public void Rotate(Vector3 dir)
    {
        if (dir == Vector3.zero)
            return;
        Vector3 cur = Vector3.RotateTowards(transform.forward, dir, 1, 1);
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
    Attack
}
