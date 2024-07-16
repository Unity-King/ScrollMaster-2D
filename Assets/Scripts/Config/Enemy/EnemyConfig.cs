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
        public string enemyName;
        public float moveSpeed;
        public int attackDamage;
        public int maxHealth;
        public int expValue; // Valor de experi�ncia que o inimigo d� ao ser derrotado
        public CombatType combatType;
        public float attackCooldown; // Cooldown para usar a habilidade
        public string attackAnimationName = "isAttacking"; // Nome da anima��o de ataque
        public float detectionRange; // Alcance de detec��o do jogador
        public float followRange; // Alcance de persegui��o do jogador
    }
}
