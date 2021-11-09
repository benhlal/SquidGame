using System.Collections;
using Model;
using UnityEngine;

namespace GamePlayManager
{
    public class FinishLine : MonoBehaviour
    {
        //*****************************************************************  EVENTS *******************************************************************************

        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponent<Character>();
            if (character == null) return;
            StartCoroutine(DeclareWinner(1f));

            IEnumerator DeclareWinner(float delay)
            {
                yield return new WaitForSeconds(delay);
                character.Win();
            }
        }
    }
}