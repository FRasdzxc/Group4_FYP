using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public MobData m_data;
    public GameObject obj;
    public float maxHealth;
    public float beforeHealth;
    public float currentHealth;
    public float damage;
    bool getDamage = false;
    float currentVelocity = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_data = obj.GetComponent<Slime>().m_data;
        maxHealth = m_data.health;
        currentHealth = m_data.health;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReduceHealth(1);
        }
        if(getDamage == true)
        {        
            float newHealth = Mathf.SmoothDamp(healthBar.value, healthBar.value - damage, ref currentVelocity, 100 * Time.deltaTime);
            healthBar.value = newHealth;
            if((double)healthBar.value <= (double)currentHealth)
            {
                getDamage = false;
            }
        }
    }
    public void ReduceHealth(int damage)
    {
        healthBar.DOValue(healthBar.value - damage, 1f).SetEase(Ease.OutCubic);
    }
}
