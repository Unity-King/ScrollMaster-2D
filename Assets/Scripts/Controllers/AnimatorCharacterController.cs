using UnityEngine;
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class AnimatorCharacter : MonoBehaviour
    {
        private Animator animator;
        private Character characterConfig;

        public void Initialize(Character config)
        {
            characterConfig = config;
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = gameObject.AddComponent<Animator>();
            }
            animator.runtimeAnimatorController = characterConfig.animatorController;
        }

        public void SetFloat(string parameterName, float value)
        {
            if (animator != null)
            {
                animator.SetFloat(parameterName, value);
            }
        }

        public void SetTrigger(string parameterName)
        {
            if (animator != null)
            {
                animator.SetTrigger(parameterName);
            }
        }
    }
}
