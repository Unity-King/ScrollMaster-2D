using UnityEngine;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "ChestConfig", menuName = "ScrollMaster2D/Chest Configuration", order = 2)]
    public class ChestConfig : ScriptableObject
    {
        public string chestName;
        public ItemConfig[] lootItems;
        public int expAmount;
        public EnemyConfig spawnableMonster;
        public bool canSpawnMonster;
    }
}
