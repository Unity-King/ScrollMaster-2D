using UnityEngine;
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class Health : MonoBehaviour
    {
        private Character characterConfig;

        [SerializeField]
        private float baseMaxHealth;

        [SerializeField]
        private float healthPerLevel;

        [SerializeField]
        private float currentHealth;

        [SerializeField]
        private float maxHealth;

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
            // Adicionar lógica de morte aqui, como animações ou reiniciar o nível
        }
    }
}