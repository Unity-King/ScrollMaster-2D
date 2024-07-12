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
            // Inicializa a sa�de com a sa�de m�xima do personagem
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
            // Adicionar l�gica de morte aqui, como anima��es ou reiniciar o n�vel
        }
    }
}