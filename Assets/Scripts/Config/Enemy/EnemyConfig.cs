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
        public int expValue; // Valor de experiência que o inimigo dá ao ser derrotado
        public CombatType combatType;

        [Header("Ranges")]
        public float followRange; // Alcance de perseguição do jogador
        public float detectionRange; // Alcance de detecção do jogador
        public float attackRange; // Alcance de ataque do jogador

        [Header("Attack Config")]
        public float attackCooldown; // Cooldown para usar a habilidade
        public float specialAttackChance; // Chance de usar o ataque especial
        public string attackAnimationName = "isAttacking"; // Nome da animação de ataque

        [Header("Spell Config")]
        public float spellEffectRange; // Alcance do efeito do feitiço

        [Header("Movement")]
        public float jumpForce; // Força do pulo do inimigo

        [Header("Teleport Config")]
        public float teleportAnimationThreshold; // Percentual da animação de ataque antes de teleportar (0-1)

        [Header("Loot Config")]
        public LootItem[] lootItems; // Itens de loot configuráveis
    }

    [System.Serializable]
    public struct LootItem
    {
        public ItemConfig itemConfig; // Configuração do item de loot
        [Range(0, 1)]
        public float dropChance; // Chance de drop (0 = 0%, 1 = 100%)
        public int minQuantity; // Quantidade mínima
        public int maxQuantity; // Quantidade máxima
    }
}
