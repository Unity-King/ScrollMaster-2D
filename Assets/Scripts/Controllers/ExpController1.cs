using ScrollMaster2D.Config;
using UnityEngine;

namespace ScrollMaster2D.Controllers
{
    public class EnemyExpController : MonoBehaviour
    {
        public EnemyConfig enemyConfig;

        public void GiveExpToPlayer()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Player player = playerObj.GetComponent<Player>();
                if (player != null)
                {
                    player.expController.AddExperience(enemyConfig.expValue);
                    Debug.Log($"{player.characterConfig.characterName} received {enemyConfig.expValue} EXP from {gameObject.name}.");
                }
            }
        }
    }
}
