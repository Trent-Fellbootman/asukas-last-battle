using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrownBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject drownUI;
    
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
        if (other.gameObject.CompareTag("Ocean"))
        {
            drownUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
