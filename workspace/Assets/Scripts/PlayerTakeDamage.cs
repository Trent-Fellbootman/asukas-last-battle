using System;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour, TakeDamageInterface
{
    [SerializeField] private GameObject playerDeadUI;

    [SerializeField] private GameObject healthBarObject;

    private HealthBar healthBar;

    private void Awake()
    {
        healthBar = healthBarObject.GetComponent<HealthBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        healthBar.health -= damage;

        if (healthBar.health <= 0)
        {
            healthBar.health = 0;
            playerDeadUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
