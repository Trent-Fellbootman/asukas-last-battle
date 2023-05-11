using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float initialHealth = 100.0f;

    [HideInInspector]
    public float health;

    private void Awake()
    {
        health = initialHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(health / initialHealth, 1, 1);
    }
}
