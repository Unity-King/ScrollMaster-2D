using UnityEngine;

namespace ScrollMaster2D.Controllers
{
    public class CameraPlayer : MonoBehaviour
    {
        public Transform playerTransform;
        public Vector3 offset;
        public float smoothSpeed = 0.125f;

        void LateUpdate()
        {
            if (playerTransform != null)
            {
                Vector3 desiredPosition = playerTransform.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
            }
        }
    }
}