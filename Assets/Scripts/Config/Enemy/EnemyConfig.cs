using UnityEngine;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScrollMaster2D/Enemy Configuration", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        public string enemyName;
        public float moveSpeed;
        public int attackDamage;
        public int maxHealth;
        public int expValue; 
        public Spell[] spells; 
    }
}
