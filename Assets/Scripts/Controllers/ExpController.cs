using UnityEngine;
using ScrollMaster2D.UI;

namespace ScrollMaster2D.Controllers
{
    public class Exp : MonoBehaviour
    {
        private Player player;
        public int currentExp = 0;
        public int expToNextLevel = 100;
        public float expMultiplier = 1.5f;
        public ExpBar expBar;
        void Start()
        {
            player = GetComponent<Player>();
            expBar = FindObjectOfType<ExpBar>();

            if (player != null && player.characterConfig != null)
            {
                InitializeExp();
            }
            else
            {
                Debug.LogError("Player or CharacterConfig is not assigned.");
            }
        }

        private void Update()
        {
            expBar.SetExp(currentExp);
        }

        private void InitializeExp()
        {
            currentExp = 0;
            expToNextLevel = CalculateExpToNextLevel(player.characterConfig.level);
            if (expBar != null)
            {
                expBar.SetMaxExp(expToNextLevel);
                expBar.SetExp(currentExp);
            }
        }
        public void AddExperience(int amount)
        {
            currentExp += amount;
            if (expBar != null)
            {
                expBar.SetExp(currentExp);
            }
            while (currentExp >= expToNextLevel)
            {
                LevelUp();
            }
        }
        private void LevelUp()
        {
            currentExp -= expToNextLevel;
            player.characterConfig.level++;
            expToNextLevel = CalculateExpToNextLevel(player.characterConfig.level);
            player.characterConfig.maxHealth += 10f;
            player.characterConfig.attackPower += 2f;
            player.characterConfig.defense += 1f;
            player.healthController.UpdateHealth();
            player.statsController.UpdateStats();

            if (expBar != null)
            {
                expBar.SetMaxExp(expToNextLevel);
                expBar.SetExp(currentExp);
            }

            Debug.Log($"{player.characterConfig.characterName} has leveled up to level {player.characterConfig.level}!");
        }
        private int CalculateExpToNextLevel(int level)
        {
            return Mathf.FloorToInt(100 * Mathf.Pow(expMultiplier, level - 1));
        }
    }
}
