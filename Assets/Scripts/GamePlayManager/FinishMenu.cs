using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlayManager
{
    public class FinishMenu : MonoBehaviour
    {
        
        
        //*****************************************************************  EVENTS *******************************************************************************
        public void GoToScene(string targetScene)
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}