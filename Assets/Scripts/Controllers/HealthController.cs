using ScrollMaster2D.Config;
using UnityEngine;

namespace ScrollMaster2D.Controllers
{
    public class Health : MonoBehaviour
    {
        private Character characterConfig;
        [SerializeField]private float currentHealth;

        void Start()
        {
            // Inicializa a saúde com a saúde máxima do personagem
            currentHealth = characterConfig.maxHealth;
        }

        public void Initialize(Character config)
        {
            characterConfig = config;
            currentHealth = characterConfig.maxHealth;
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
            if (currentHealth > characterConfig.maxHealth)
            {
                currentHealth = characterConfig.maxHealth;
            }
        }

        private void Die()
        {
            Debug.Log($"{characterConfig.characterName} has died.");
            // Adicionar lógica de morte aqui, como animações ou reiniciar o nível
        }
    }
}