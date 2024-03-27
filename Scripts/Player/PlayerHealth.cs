using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float playerDef;
    [SerializeField] private HealthBar healthBar;

    private Animator anim;
    public GameObject deathPanel;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = playerMaxHealth;
        healthBar.SetMaxHealth(playerMaxHealth);
    }

    // Update is called once per frame
    public void TakeDamage(float damage)
    {
        float reducedDamage = Mathf.Max(damage - playerDef, 0f);
        currentHealth -= reducedDamage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Player took " + damage + "damage. Health remaining: "+ currentHealth);
        if (currentHealth <= 0f)
        {
            PlayerDie();
        }
    }
    private void PlayerDie()
    {
        anim.SetTrigger("death");
        Debug.Log("Player died.");
        gameObject.SetActive(false);
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }
}
