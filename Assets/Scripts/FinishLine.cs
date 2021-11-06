using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character != null)
        {
           
            
            StartCoroutine(ResetCanTP(1f));
            IEnumerator  ResetCanTP (float delay)
            {
                yield return new WaitForSeconds(delay);
                character.Win();
      
            }
            
        }
    }
    
    
}