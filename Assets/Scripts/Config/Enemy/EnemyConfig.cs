using UnityEngine;

namespace ScrollMaster2D.Config
{
    public enum CombatType
    {
        Passive,
        Aggressive
    }

    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScrollMaster2D/Enemy Configuration", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Basic Info")]
        public string enemyName;
        public float moveSpeed;
        public int attackDamage;
        public int maxHealth;
        public int expValue; 
        public CombatType combatType;

        [Header("Ranges")]
        public float followRange; 
        public float detectionRange; 
        public float attackRange; 

        [Header("Attack Config")]
        public float attackCooldown;
        public float specialAttackChance; 
        public string attackAnimationName = "isAttacking"; 

        [Header("Spell Config")]
        public float spellEffectRange;

        [Header("Movement")]
        public float jumpForce;

        [Header("Teleport Config")]
        public float teleportAnimationThreshold;

        [Header("Loot Config")]
        public LootItem[] lootItems;
    }

    [System.Serializable]
    public struct LootItem
    {
        public ItemConfig itemConfig; 
        [Range(0, 1)]
        public float dropChance; // (0 = 0%, 1 = 100%)
        public int minQuantity;
        public int maxQuantity; 
    }
}
