using ScrollMaster2D.Config;
using UnityEngine;
using UnityEngine.UI;

namespace ScrollMaster2D.Controllers
{
    public class EnemyHealthController : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;
        public Slider healthBarSlider; 
        public EnemyConfig enemyConfig;
        private EnemyExpController expController;

        void Start()
        {
            Initialize(enemyConfig.maxHealth);
            expController = GetComponent<EnemyExpController>();
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage, current health: {currentHealth}");

            UpdateHealthBar();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} died.");
            if (expController != null)
            {
                expController.GiveExpToPlayer();
            }
            Destroy(gameObject);
        }

        public void Initialize(int initialHealth)
        {
            maxHealth = initialHealth;
            currentHealth = maxHealth;
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBarSlider != null)
            {
                healthBarSlider.value = (float)currentHealth / maxHealth;
            }
        }
    }
}
