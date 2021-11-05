using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class PlayerMovement : CharacterMovement
{
    public float jumpAmount = 10;

    private Vector2 moveVector;

    void Update()
    {
        verticalDirection = Input.GetAxis("Vertical");
        horizontalDirection = Input.GetAxis("Horizontal");
        verticalDirection = Mathf.Clamp(verticalDirection, -1, 1);
        horizontalDirection = Mathf.Clamp(horizontalDirection, -1, 1);

        Vector3 movementDirection = new Vector3(horizontalDirection, 0, verticalDirection);

        animator.SetFloat("Speed", verticalDirection);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity = Vector3.forward * verticalDirection * movementSpeed * Time.fixedDeltaTime;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("RightArrow");
            transform.Translate(Vector3.right * Time.deltaTime * movementSpeed * horizontalDirection,
                Space.Self); //LEFT
            animator.SetFloat("Speed", horizontalDirection);
        }
    }

    public override void Die()
    {
        base.Die();

        UIManager.Instance.TriggerLoseMenu();
    }

    public override void Win()
    {
        base.Win();

        animator.SetTrigger("isWinner");

        UIManager.Instance.TriggerWinMenu();

        Debug.Log("WINS NEXT GAME");
    }
}