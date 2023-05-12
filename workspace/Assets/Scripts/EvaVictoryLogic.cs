using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaVictoryLogic : MonoBehaviour
{
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject[] enemySpawnerObjects;
    
    private SpawnZombie[] enemySpawners;

    private void Awake()
    {
        enemySpawners = new SpawnZombie[enemySpawnerObjects.Length];
        for (int i = 0; i < enemySpawnerObjects.Length; i++)
        {
            enemySpawners[i] = enemySpawnerObjects[i].GetComponent<SpawnZombie>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool victory = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            foreach (var enemySpawner in enemySpawners)
            {
                if (enemySpawner.remainingEnemies > 0)
                {
                    victory = false;
                    break;
                }
            }
        }
        else
        {
            victory = false;
        }

        if (victory)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (enemy.GetComponent<ZombieTakeDamage>().currentHealth > 0)
                {
                    victory = false;
                    break;
                }
            }
        }

        if (victory)
        {
            Destroy(player);
            victoryUI.SetActive(true);
        }
    }
}
