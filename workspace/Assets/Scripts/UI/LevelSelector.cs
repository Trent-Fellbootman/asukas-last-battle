using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private string[] levelNames;

    [SerializeField] private string[] scenesNames;

    private void Awake()
    {
        var dropdownMenu = GetComponent<TMP_Dropdown>();
        dropdownMenu.options =
            levelNames.Select(((levelName, index) => new TMP_Dropdown.OptionData(levelName))).ToList();
        
        dropdownMenu.onValueChanged.AddListener(_loadLevel);
    }

    private void Update()
    {
        if (levelNames.Length != scenesNames.Length)
        {
            throw new Exception("Level names and scenes must be the same length!");
        }
    }

    private void _loadLevel(int index)
    {
        SceneManager.LoadScene(scenesNames[index]);
    }
}