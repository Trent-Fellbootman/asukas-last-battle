using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject help;
    [SerializeField] private GameObject helpHint;

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
    }
}