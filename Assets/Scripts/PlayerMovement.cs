using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    public float jumpAmount = 10;

    void Update()
    {
        verticalDirection = Input.GetAxis("Vertical");
        verticalDirection = Mathf.Clamp(verticalDirection, 0, 1);
        animator.SetFloat("Speed", verticalDirection);
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("JUMP PRESS SPACE");

            rb.AddForce(Vector2.up * jumpAmount, ForceMode.Impulse);
        }*/
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