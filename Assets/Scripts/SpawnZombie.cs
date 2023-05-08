using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnZombie : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private float spawnInterval = 5.0f;
    [SerializeField]
    private int totalSpawnCount = 10;

    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = Random.Range(0, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= spawnInterval && totalSpawnCount > 0)
        {
            currentTime = 0.0f;
            spawnZombie();
            totalSpawnCount--;
        }
    }

    private void spawnZombie() {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 100.0f, NavMesh.AllAreas))
        {
            Instantiate(zombiePrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Not hit");
        }
    }
}
