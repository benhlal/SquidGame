using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 100f;

    private float verticalDirection;

    private Rigidbody rb;

    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity =
            Vector3.forward *
            verticalDirection *
            movementSpeed *
            Time.fixedDeltaTime;
    }

    public bool IsMoving()
    {
        return rb.velocity.magnitude > 0.1f;
    }

    void Update()
    {
        verticalDirection = Input.GetAxis("Vertical");
        verticalDirection = Mathf.Clamp(verticalDirection, 0, 1);
        animator.SetFloat("Speed", verticalDirection);
    }
}
