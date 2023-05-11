using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject help;
    [SerializeField] private GameObject helpHint;
    [SerializeField] private GameObject healthBarObject;

    [SerializeField] private float easyModePlayerHealth = 1000.0f;
    [SerializeField] private float mediumModePlayerHealth = 300.0f;
    [SerializeField] private float hardModePlayerHealth = 100.0f;
    [SerializeField] private float realisticModePlayerHealth = 10.0f;

    private HealthBar healthBar;

    private void Awake()
    {
        healthBar = healthBarObject.GetComponent<HealthBar>();
        
        healthBar.health = healthBar.health / healthBar.initialHealth * easyModePlayerHealth;
        healthBar.initialHealth = easyModePlayerHealth;
        
        Debug.Log("Player health: " + healthBar.health + "/" + healthBar.initialHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }

        if (Input.GetButtonDown("Help"))
        {
            help.SetActive(true);
            helpHint.SetActive(false);
        }

        if (Input.GetButtonDown("HelpExit"))
        {
            help.SetActive(false);
            helpHint.SetActive(true);
        }

        if (Input.GetButtonDown("EasyMode"))
        {
            healthBar.health = healthBar.health / healthBar.initialHealth * easyModePlayerHealth;
            healthBar.initialHealth = easyModePlayerHealth;
            
            Debug.Log("Player health: " + healthBar.health + "/" + healthBar.initialHealth);
        }
        
        if (Input.GetButtonDown("MediumMode"))
        {
            healthBar.health = healthBar.health / healthBar.initialHealth * mediumModePlayerHealth;
            healthBar.initialHealth = mediumModePlayerHealth;
            
            Debug.Log("Player health: " + healthBar.health + "/" + healthBar.initialHealth);
        }
        
        if (Input.GetButtonDown("HardMode"))
        {
            healthBar.health = healthBar.health / healthBar.initialHealth * hardModePlayerHealth;
            healthBar.initialHealth = hardModePlayerHealth;
            
            Debug.Log("Player health: " + healthBar.health + "/" + healthBar.initialHealth);
        }
        
        if (Input.GetButtonDown("RealisticMode"))
        {
            healthBar.health = healthBar.health / healthBar.initialHealth * realisticModePlayerHealth;
            healthBar.initialHealth = realisticModePlayerHealth;
            
            Debug.Log("Player health: " + healthBar.health + "/" + healthBar.initialHealth);
        }
    }
}