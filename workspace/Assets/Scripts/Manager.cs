using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    [Tooltip("These objects will be disabled when the game is paused.")]
    [SerializeField] private GameObject[] hudUIElements;

    [SerializeField] private GameObject musicPlayerObject;

    private AudioSource musicPlayer;
    
    private bool[] lastHudUIElementsEnabled;
    private float lastTimeScale = 1;

    private void Awake()
    {
        musicPlayer = musicPlayerObject.GetComponent<AudioSource>();
        
        lastHudUIElementsEnabled = new bool[hudUIElements.Length];

        for (int i = 0; i < hudUIElements.Length; i++)
        {
            lastHudUIElementsEnabled[i] = hudUIElements[i].activeSelf;
        }

        _resumeGame();
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleMenu"))
        {
            if (menuUI.activeSelf)
            {
                _resumeGame();
            }
            else
            {
                _pauseGame();
            }
        }
    }

    private void _pauseGame()
    {
        menuUI.SetActive(true);
        lastTimeScale = Time.timeScale;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        musicPlayer.Pause();

        for (int i = 0; i < hudUIElements.Length; i++)
        {
            lastHudUIElementsEnabled[i] = hudUIElements[i].activeSelf;
            hudUIElements[i].SetActive(false);
        }
    }

    private void _resumeGame()
    {
        menuUI.SetActive(false);
        musicPlayer.UnPause();
        
        Time.timeScale = lastTimeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        for (int i = 0; i < hudUIElements.Length; i++)
        {
            hudUIElements[i].SetActive(lastHudUIElementsEnabled[i]);
        }
    }
}