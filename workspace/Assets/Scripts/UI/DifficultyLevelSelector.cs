using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DifficultyLevelSelector : MonoBehaviour
{
    [SerializeField] private String[] difficultyLevelNames;

    [SerializeField] private float[] playerHealthLevels;

    [SerializeField] private int initialDifficultyLevelIndex = 0;

    [SerializeField] private GameObject healthBarObject;

    private HealthBar healthBar;

    private void Awake()
    {
        var dropdownMenu = GetComponent<TMP_Dropdown>();
        dropdownMenu.options = difficultyLevelNames
            .Select(difficultyLevelName => new TMP_Dropdown.OptionData(difficultyLevelName))
            .ToList();
        dropdownMenu.onValueChanged.AddListener(_setDifficultyLevel);
        
        healthBar = healthBarObject.GetComponent<HealthBar>();

        dropdownMenu.value = initialDifficultyLevelIndex;
    }

    private void _setDifficultyLevel(int index)
    {
        healthBar.health = healthBar.health / healthBar.initialHealth * playerHealthLevels[index];
        healthBar.initialHealth = playerHealthLevels[index];
        Debug.Log("Player health: " + healthBar.health + "/" + healthBar.initialHealth);
    }
}