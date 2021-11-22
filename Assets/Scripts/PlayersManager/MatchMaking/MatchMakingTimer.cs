using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;

namespace GamePlayManager.MatchMaking
{
    public class MatchMakingTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private float initialTime = 20f;
        [SerializeField] public float currentTime;

        // Start is called before the first frame update
        void Start()
        {
            currentTime = initialTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                TimeSpan span = TimeSpan.FromSeconds(currentTime);
                timerText.text = span.ToString(@"mm\:ss");
                Debug.Log("Current Time :" + currentTime);
            }
        }
    }
}