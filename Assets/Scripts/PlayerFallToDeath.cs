using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallToDeath : MonoBehaviour
{
    [SerializeField]
    GameObject fallToDeathUI;

    [SerializeField] private float fallToDeathVelocityThreshold = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (Mathf.Abs(other.relativeVelocity.y) > fallToDeathVelocityThreshold)
        {
                fallToDeathUI.SetActive(true);
                Destroy(gameObject);
        }
    }
}
