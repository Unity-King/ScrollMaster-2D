using UnityEngine;
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class Stats : MonoBehaviour
    {
        private Character characterConfig;

        [SerializeField]
        private float baseAttackPower;

        [SerializeField]
        private float baseDefense;

        [SerializeField]
        private float attackPowerPerLevel;

        [SerializeField]
        private float defensePerLevel;

        [SerializeField]
        private float attackPower;

        [SerializeField]
        private float defense;

        public float AttackPower { get { return attackPower; } }
        public float Defense { get { return defense; } }

        public void Initialize(Character config)
        {
            characterConfig = config;
            baseAttackPower = characterConfig.attackPower;
            baseDefense = characterConfig.defense;
            UpdateStats();
        }

        public void UpdateStats()
        {
            attackPower = baseAttackPower + (characterConfig.level * attackPowerPerLevel);
            defense = baseDefense + (characterConfig.level * defensePerLevel);
        }

        public void IncreaseAttackPower(float amount)
        {
            attackPower += amount;
        }

        public void DecreaseAttackPower(float amount)
        {
            attackPower = Mathf.Max(0, attackPower - amount);
        }

        public void IncreaseDefense(float amount)
        {
            defense += amount;
        }

        public void DecreaseDefense(float amount)
        {
            defense = Mathf.Max(0, defense - amount);
        }
    }
}
