using UnityEngine;

namespace GamePlayManager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject loseMenu;
        [SerializeField] private GameObject winMenu;


//*****************************************************************  EVENTS *******************************************************************************

        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void TriggerLoseMenu() => loseMenu.SetActive(true);


        public void TriggerWinMenu() => winMenu.SetActive(true);
    }
}