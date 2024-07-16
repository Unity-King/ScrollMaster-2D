using UnityEngine;
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyConfig enemyConfig;
        public Transform player; // Referência ao jogador
        private bool hasBeenAttacked = false;

        [SerializeField]
        private float currentSpeed;
        [SerializeField]
        private string currentAnimation;

        [Header("Animator Parameters")]
        [SerializeField]
        private string speedParameter = "Speed";

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
                LogCurrentAnimation();
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

            if (enemyConfig.combatType == CombatType.Passive)
            {
                if (hasBeenAttacked || distanceToPlayer <= enemyConfig.detectionRange)
                {
                    MoveTowardsPlayer();
                }
                else
                {
                    Patrol();
                }
            }
            else if (enemyConfig.combatType == CombatType.Aggressive)
            {
                if (distanceToPlayer <= enemyConfig.detectionRange)
                {
                    MoveTowardsPlayer();
                }
                else
                {
                    Patrol();
                }
            }

            animator.SetFloat(speedParameter, Mathf.Abs(rb.velocity.x));
        }

        private void Patrol()
        {
            // Lógica de patrulha básica: andar de um lado para o outro
            currentSpeed = enemyConfig.moveSpeed / 2;
            rb.velocity = new Vector2(currentSpeed * (isFacingRight ? 1 : -1), rb.velocity.y);
        }

        private void MoveTowardsPlayer()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= enemyConfig.followRange)
            {
                currentSpeed = enemyConfig.moveSpeed;
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * currentSpeed, rb.velocity.y);

                if (direction.x > 0 && !isFacingRight)
                {
                    Flip();
                }
                else if (direction.x < 0 && isFacingRight)
                {
                    Flip();
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
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
            if (distanceToPlayer <= enemyConfig.detectionRange && Time.time >= nextAttackTime)
            {
                animator.SetBool(enemyConfig.attackAnimationName, true);
                nextAttackTime = Time.time + enemyConfig.attackCooldown;
            }
            else
            {
                animator.SetBool(enemyConfig.attackAnimationName, false);
            }
        }

        public void TakeDamage(int damage)
        {
            healthController.TakeDamage(damage);
            hasBeenAttacked = true;
        }

        public void DealDamage() // Chamado pelo evento de animação
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= enemyConfig.detectionRange)
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyConfig.detectionRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, enemyConfig.followRange);
        }
    }
}
