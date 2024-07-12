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

        private AnimatorCharacter animatorController;
        private Rigidbody2D rb;
        private Health healthController;
        private Stats statsController;
        private float nextAttackTime = 0f;
        private bool isGrounded;

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
            rb.velocity = new Vector2(moveInput * characterConfig.moveSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            animatorController.SetFloat("Speed", Mathf.Abs(moveInput));
        }

        private void HandleAttacks()
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
            {
                animatorController.SetTrigger("Attack");
                // Lógica de ataque aqui, usando statsController.AttackPower
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
                // Lógica para usar sprite em vez de prefab
            }

            Debug.Log($"{characterConfig.characterName} cast {spell.spellName}");
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
