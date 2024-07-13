using UnityEngine;
using ScrollMaster2D.Config;
using ScrollMaster2D.Controllers;

namespace ScrollMaster2D.Controllers
{
    public class Player : MonoBehaviour
    {
        public Character characterConfig;
        public float jumpForce = 10f;
        public float attackCooldown = 0.5f;

        [SerializeField]
        private float currentSpeed;
        [SerializeField]
        private string currentAnimation;

        private AnimatorCharacter animatorController;
        private Rigidbody2D rb;
        private Health healthController;
        private Stats statsController;
        private float nextAttackTime = 0f;
        private bool isGrounded;
        private bool isFacingRight = true;

        void Start()
        {
            if (characterConfig != null)
            {
                InitializeCharacter();
            }
            else
            {
                Debug.LogError("CharacterConfig is not assigned.");
            }
        }

        void Update()
        {
            HandleMovement();
            HandleAttacks();
            HandleSpells();
            UpdateAnimator();
        }

        private void InitializeCharacter()
        {
            animatorController = GetComponent<AnimatorCharacter>();
            if (animatorController == null)
            {
                animatorController = gameObject.AddComponent<AnimatorCharacter>();
            }
            animatorController.Initialize(characterConfig);

            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }

            // Configurar Rigidbody2D
            rb.gravityScale = 3;
            rb.freezeRotation = true;

            healthController = GetComponent<Health>();
            if (healthController == null)
            {
                healthController = gameObject.AddComponent<Health>();
            }
            healthController.Initialize(characterConfig);

            statsController = GetComponent<Stats>();
            if (statsController == null)
            {
                statsController = gameObject.AddComponent<Stats>();
            }
            statsController.Initialize(characterConfig);
        }

        private void HandleMovement()
        {
            float moveInput = Input.GetAxis("Horizontal");
            currentSpeed = moveInput * characterConfig.moveSpeed;
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            animatorController.SetFloat("Speed", Mathf.Abs(moveInput * characterConfig.moveSpeed));

            // Virar o sprite baseado na dire��o do movimento
            if (moveInput > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && isFacingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void HandleAttacks()
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
            {
                animatorController.SetTrigger("Attack");
                // L�gica de ataque aqui, usando statsController.AttackPower
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        private void HandleSpells()
        {
            foreach (var spell in characterConfig.spells)
            {
                if (Input.GetKeyDown(spell.hotkey))
                {
                    CastSpell(spell);
                }
            }
        }

        private void CastSpell(Spell spell)
        {
            if (spell.usePrefab && spell.spellPrefab != null)
            {
                Instantiate(spell.spellPrefab, transform.position, Quaternion.identity);
            }
            else if (!spell.usePrefab && spell.spellSprite != null)
            {
                // L�gica para usar sprite em vez de prefab
            }

            Debug.Log($"{characterConfig.characterName} cast {spell.spellName}");
        }

        private void UpdateAnimator()
        {
            Animator animator = animatorController.GetComponent<Animator>();
            currentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
}
