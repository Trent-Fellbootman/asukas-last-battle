using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string[] levelLabels;
    [SerializeField] private string[] levelSceneNames;
    [SerializeField] private int defaultLevelIndex;
    
    private DropdownField levelSelector;
    private Button gameStartButton;
    private Button quitButton;
    
    private void OnEnable()
    {
        Debug.Log(String.Join(", ", levelLabels));
        Debug.Log(String.Join(", ", levelSceneNames));
        
        var uiDocument = GetComponent<UIDocument>();
        
        levelSelector = uiDocument.rootVisualElement.Q<DropdownField>("LevelSelector");
        levelSelector.choices = levelLabels.ToList();
        
        gameStartButton = uiDocument.rootVisualElement.Q<Button>("GameStartButton");
        quitButton = uiDocument.rootVisualElement.Q<Button>("QuitButton");
        
        levelSelector.value = levelLabels[defaultLevelIndex];
        gameStartButton.clicked += OnGameStartButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnGameStartButtonClicked()
    {
        int levelIndex = Array.IndexOf(levelLabels, levelSelector.value);
        if (levelIndex >= 0)
        {
            SceneManager.LoadScene(levelSceneNames[levelIndex]);
        }
    }
    
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
