using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject WinMenu;


    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerLoseMenu() => loseMenu.SetActive(true);
    public void TriggerWinMenu() => WinMenu.SetActive(true);
}