using ScrollMaster2D.Config;
using UnityEngine;

namespace ScrollMaster2D.Controllers
{
    public class Stats : MonoBehaviour
    {
        private Character characterConfig;
        public float AttackPower { get; private set; }
        public float Defense { get; private set; }

        public void Initialize(Character config)
        {
            characterConfig = config;
            AttackPower = characterConfig.attackPower;
            Defense = characterConfig.defense;
        }

        public void IncreaseAttackPower(float amount)
        {
            AttackPower += amount;
        }

        public void DecreaseAttackPower(float amount)
        {
            AttackPower = Mathf.Max(0, AttackPower - amount);
        }

        public void IncreaseDefense(float amount)
        {
            Defense += amount;
        }

        public void DecreaseDefense(float amount)
        {
            Defense = Mathf.Max(0, Defense - amount);
        }
    }
}