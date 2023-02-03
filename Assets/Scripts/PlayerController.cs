using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed;
    
    [SerializeField] private CharacterController cc;
    
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
            
        cc.Move(res * speed * Time.deltaTime);
    }

    public void Rotate(Vector3 dir)
    {
        
    }

    public void Attack()
    {
        
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (cc == null)
            cc = GetComponent<CharacterController>();
    }
    #endif
}

public enum PlayerState
{
    Move,
    Attack
}
