using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerEscape : MonoBehaviour
{
    [FormerlySerializedAs("escapeSite")] [SerializeField]
    GameObject victory;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EscapeSite"))
        {
            victory.SetActive(true);
            Destroy(gameObject);
        }
    }
}
