using UnityEngine;
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyConfig enemyConfig;
        public Transform player; // Referência ao jogador
        public float attackRange = 1.5f;
        public float attackCooldown = 1f;

        [SerializeField]
        private float currentSpeed;
        [SerializeField]
        private string currentAnimation;

        [Header("Animator Parameters")]
        [SerializeField]
        private string speedParameter = "Speed";
        [SerializeField]
        private string isAttackingParameter = "isAttacking";

        private Animator animator;
        private Rigidbody2D rb;
        private EnemyHealthController healthController;
        private float nextAttackTime = 0f;
        private bool isFacingRight = true;

        void Start()
        {
            if (enemyConfig != null)
            {
                InitializeEnemy();
            }
            else
            {
                Debug.LogError("EnemyConfig is not assigned.");
            }

            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            if (player != null)
            {
                HandleMovement();
                HandleAttacks();
                LogCurrentAnimation(); // Adicionar para logar a animação atual
            }
        }

        private void InitializeEnemy()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }

            rb.gravityScale = 3;
            rb.freezeRotation = true;

            healthController = GetComponent<EnemyHealthController>();
            if (healthController == null)
            {
                healthController = gameObject.AddComponent<EnemyHealthController>();
            }
            healthController.Initialize(enemyConfig.maxHealth);
        }

        private void HandleMovement()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > attackRange)
            {
                currentSpeed = enemyConfig.moveSpeed;
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * currentSpeed, rb.velocity.y);

                animator.SetBool(isAttackingParameter, false);

                if (direction.x < 0 && !isFacingRight)
                {
                    Flip();
                }
                else if (direction.x > 0 && isFacingRight)
                {
                    Flip();
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            animator.SetFloat(speedParameter, Mathf.Abs(rb.velocity.x));
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x = isFacingRight ? Mathf.Abs(theScale.x) : -Mathf.Abs(theScale.x);
            transform.localScale = theScale;

            Debug.Log("Flipping. isFacingRight: " + isFacingRight);
        }

        private void HandleAttacks()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                animator.SetBool(isAttackingParameter, true);
                nextAttackTime = Time.time + attackCooldown;
            }
            else
            {
                animator.SetBool(isAttackingParameter, false);
            }
        }

        public void DealDamage() // Chamado pelo evento de animação
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(enemyConfig.attackDamage);
                }
            }
        }

        private void LogCurrentAnimation()
        {
            if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
                currentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                Debug.Log($"Current Animation: {currentAnimation}");
            }
        }

        public void TakeDamage(int damage)
        {
            healthController.TakeDamage(damage);
        }
    }
}
