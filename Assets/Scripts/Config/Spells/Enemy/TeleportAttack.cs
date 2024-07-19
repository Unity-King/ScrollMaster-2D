using UnityEngine;

namespace ScrollMaster2D.Controllers
{
    [CreateAssetMenu(fileName = "TeleportAttack", menuName = "ScrollMaster2D/Abilities/TeleportAttack", order = 1)]
    public class TeleportAttack : ScriptableObject
    {
        public float teleportDistance = 2f;

        public void Execute(Transform enemyTransform, Transform playerTransform)
        {
            Vector2 direction = (playerTransform.position - enemyTransform.position).normalized;
            Vector2 newPosition = (Vector2)playerTransform.position - direction * teleportDistance;
            enemyTransform.position = newPosition;
            Debug.Log($"Teleported {enemyTransform.name} to {newPosition}");
        }
    }
}
