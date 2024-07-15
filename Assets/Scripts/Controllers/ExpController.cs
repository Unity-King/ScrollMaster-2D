using UnityEngine;

namespace ScrollMaster2D.Controllers
{
    public class ExpController : MonoBehaviour
    {
        public Character characterConfig;
        public int currentExp = 0;
        public int expToNextLevel = 100;
        public float expMultiplier = 1.5f;

        void Start()
        {
            if (characterConfig != null)
            {
                InitializeExp();
            }
            else
            {
                Debug.LogError("CharacterConfig is not assigned.");
            }
        }

        private void InitializeExp()
        {
            currentExp = 0;
            expToNextLevel = CalculateExpToNextLevel(characterConfig.level);
        }

        public void AddExperience(int amount)
        {
            currentExp += amount;
            while (currentExp >= expToNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentExp -= expToNextLevel;
            characterConfig.level++;
            expToNextLevel = CalculateExpToNextLevel(characterConfig.level);

            // Apply attribute increases
            characterConfig.maxHealth += 10f;
            characterConfig.attackPower += 2f;
            characterConfig.defense += 1f;

            Debug.Log($"{characterConfig.characterName} has leveled up to level {characterConfig.level}!");
        }

        private int CalculateExpToNextLevel(int level)
        {
            return Mathf.FloorToInt(expToNextLevel * Mathf.Pow(expMultiplier, level - 1));
        }
    }
}
