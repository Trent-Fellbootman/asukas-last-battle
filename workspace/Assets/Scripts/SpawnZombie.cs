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

    public int remainingEnemies
    {
        get => totalSpawnCount;
    }

    private float _currentTime;

    // Start is called before the first frame update
    void Start()
    {
        _currentTime = Random.Range(0, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= spawnInterval && totalSpawnCount > 0)
        {
            _currentTime = 0.0f;
            _spawnZombie();
            totalSpawnCount--;
        }
    }

    private void _spawnZombie() {
        if (NavMesh.SamplePosition(transform.position, out var hit, 1.0f, NavMesh.AllAreas))
        {
            Instantiate(zombiePrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Not hit");
        }
    }
}
