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
        public int expValue; // Valor de experi�ncia que o inimigo d� ao ser derrotado
        public CombatType combatType;

        [Header("Ranges")]
        public float followRange; // Alcance de persegui��o do jogador
        public float detectionRange; // Alcance de detec��o do jogador
        public float attackRange; // Alcance de ataque do jogador

        [Header("Attack Config")]
        public float attackCooldown; // Cooldown para usar a habilidade
        public float specialAttackChance; // Chance de usar o ataque especial
        public string attackAnimationName = "isAttacking"; // Nome da anima��o de ataque

        [Header("Spell Config")]
        public float spellEffectRange; // Alcance do efeito do feiti�o

        [Header("Movement")]
        public float jumpForce; // For�a do pulo do inimigo

        [Header("Teleport Config")]
        public float teleportAnimationThreshold; // Percentual da anima��o de ataque antes de teleportar (0-1)

        [Header("Loot Config")]
        public LootItem[] lootItems; // Itens de loot configur�veis
    }

    [System.Serializable]
    public struct LootItem
    {
        public ItemConfig itemConfig; // Configura��o do item de loot
        [Range(0, 1)]
        public float dropChance; // Chance de drop (0 = 0%, 1 = 100%)
        public int minQuantity; // Quantidade m�nima
        public int maxQuantity; // Quantidade m�xima
    }
}
