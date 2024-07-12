using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScrollMaster2D.Controllers
{
    public class Background : MonoBehaviour
    {
        public float speed;

        [SerializeField]
        private Renderer bgRenderer;

        private Player playerController;

        void Start()
        {
            playerController = FindObjectOfType<Player>();
            if (playerController == null)
            {
                Debug.LogError("PlayerController not found in the scene.");
            }
        }

        void Update()
        {
            if (playerController != null)
            {
                speed = playerController.characterConfig.moveSpeed;
                bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
            }
        }
    }
}
