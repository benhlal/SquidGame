using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMovement : CharacterMovement
{
    private DollMovement doll;

    private float currentTime;
    private float currentStoppingTime;
    private bool shouldBeCounting = true;

    private void OnEnable()
    {
        if (doll != null) return;
        doll = FindObjectOfType<DollMovement>();
        doll.OnStartCounting += OnStartCounting;
        doll.OnStopCounting += OnStopCounting;
        currentStoppingTime = Random.Range(3, 6);

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBeCounting) currentTime += Time.deltaTime;

        if (currentTime >= currentStoppingTime)
        {
            verticalDirection = 0;
            shouldBeCounting = false;
        }

        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void OnStartCounting()
    {
        verticalDirection = 1;
        currentTime = 0;
        shouldBeCounting = true;
        currentStoppingTime = Random.Range(3, 6);
    }

    private void OnStopCounting()
    {
    }
}