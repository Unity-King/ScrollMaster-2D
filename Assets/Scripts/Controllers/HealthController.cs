using UnityEngine;
using ScrollMaster2D.Config;
using ScrollMaster2D.UI;
using System;

namespace ScrollMaster2D.Controllers
{
    public class Health : MonoBehaviour
    {
        private Character characterConfig;
        public HealthBar healthBar;

        [SerializeField]
        private float baseMaxHealth;

        [SerializeField]
        private float healthPerLevel;

        [SerializeField]
        private float currentHealth;

        [SerializeField]
        private float maxHealth;

        private void Update()
        {
            healthBar.SetHealth(currentHealth);
        }
        public void Initialize(Character config)
        {
            characterConfig = config;
            baseMaxHealth = characterConfig.maxHealth;
            UpdateHealth();
        }
        public void UpdateHealth()
        {
            maxHealth = baseMaxHealth + (characterConfig.level * healthPerLevel);
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(currentHealth);
            healthBar.SetHealth(currentHealth);
            Debug.Log("Teste Update Health Function Current Health : " + currentHealth);

        }
        public void TakeDamage(float damage)
        {
            float effectiveDamage = damage - characterConfig.defense;
            if (effectiveDamage < 0) effectiveDamage = 0;

            currentHealth -= effectiveDamage;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        public void Heal(float amount)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        private void Die()
        {
            Debug.Log($"{characterConfig.characterName} has died.");
        }
    }
}