using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    public void GoToScene(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }
}