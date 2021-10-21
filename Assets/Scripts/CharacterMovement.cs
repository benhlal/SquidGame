using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    protected float movementSpeed = 100f;

    protected Rigidbody rb;

    protected Animator animator;

    protected float verticalDirection= 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public bool IsMoving()
    {
        return rb.velocity.magnitude > 0.1f;
    }

    void FixedUpdate()
    {
        rb.velocity =
            Vector3.forward *
            verticalDirection *
            movementSpeed *
            Time.fixedDeltaTime;
    }
}
