using System.Collections;
using UnityEngine;
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyConfig enemyConfig;
        public Transform player;
        private bool hasBeenAttacked = false;

        [SerializeField]
        private float currentSpeed;
        [SerializeField]
        private string currentAnimation;

        [Header("Animator Parameters")]
        [SerializeField]
        private string speedParameter = "Speed";
        [SerializeField]
        private string attackParameter = "isAttacking";

        private Animator animator;
        private Rigidbody2D rb;
        private EnemyHealthController healthController;
        private float nextAttackTime = 0f;
        private bool isFacingRight = true;
        private bool isGrounded = false;

        [Header("Ground Check")]
        [SerializeField]
        private Transform groundCheck;
        [SerializeField]
        private float groundCheckRadius = 0.2f;
        [SerializeField]
        private LayerMask groundLayer;

        [Header("Obstacle Check")]
        [SerializeField]
        private Transform obstacleCheck;
        [SerializeField]
        private float obstacleCheckDistance = 0.5f;
        [SerializeField]
        private LayerMask obstacleLayer;

        private bool isSpecialAttackReady = false;

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

        private void FixedUpdate()
        {
            CheckGround();
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

        private void CheckGround()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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
            currentSpeed = enemyConfig.moveSpeed / 2;
            rb.velocity = new Vector2(currentSpeed * (isFacingRight ? 1 : -1), rb.velocity.y);

            if (IsObstacleInFront())
            {
                Flip();
            }
        }

        private void MoveTowardsPlayer()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= enemyConfig.followRange)
            {
                currentSpeed = enemyConfig.moveSpeed;
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * currentSpeed, rb.velocity.y);

                if (direction.x < 0 && !isFacingRight)
                {
                    Flip();
                }
                else if (direction.x > 0 && isFacingRight)
                {
                    Flip();
                }

                if (isGrounded && IsObstacleInFront())
                {
                    Jump();
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        private void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, enemyConfig.jumpForce);
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            Debug.Log("Flipping. isFacingRight: " + isFacingRight);
        }

        private void HandleAttacks()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= enemyConfig.detectionRange && Time.time >= nextAttackTime)
            {
                if (Random.value <= enemyConfig.specialAttackChance)
                {
                    isSpecialAttackReady = true;
                }

                Debug.Log($"{enemyConfig.enemyName} is preparing to attack!");

                StartCoroutine(PerformAttack());
                nextAttackTime = Time.time + enemyConfig.attackCooldown;
            }
        }

        private IEnumerator PerformAttack()
        {
            animator.SetBool(attackParameter, true);

            yield return null;

            float attackAnimationDuration = animator.GetCurrentAnimatorClipInfo(0).Length > 0 ? animator.GetCurrentAnimatorClipInfo(0)[0].clip.length : 1f;
            float teleportTime = attackAnimationDuration * enemyConfig.teleportAnimationThreshold;

            yield return new WaitForSeconds(attackAnimationDuration * enemyConfig.teleportAnimationThreshold);

            if (isSpecialAttackReady)
            {
                TeleportToPlayer();
                isSpecialAttackReady = false;
            }

            yield return new WaitForSeconds(attackAnimationDuration * (1 - enemyConfig.teleportAnimationThreshold));
            animator.SetBool(attackParameter, false);
        }

        private void TeleportToPlayer()
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPosition = (Vector2)player.position - direction * 2f;
            transform.position = newPosition;
            Debug.Log($"Teleported {transform.name} to {newPosition}");
        }

        public void TakeDamage(int damage)
        {
            healthController.TakeDamage(damage);
            hasBeenAttacked = true;
        }

        public void DealDamage()
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
            Gizmos.color = Color.green;
            Gizmos.DrawRay(obstacleCheck.position, (isFacingRight ? Vector2.right : Vector2.left) * obstacleCheckDistance);
        }

        private bool IsObstacleInFront()
        {
            RaycastHit2D hit = Physics2D.Raycast(obstacleCheck.position, isFacingRight ? Vector2.right : Vector2.left, obstacleCheckDistance, obstacleLayer);
            return hit.collider != null;
        }

        private void OnDestroy()
        {
            EnemyLootController lootController = GetComponent<EnemyLootController>();
            if (lootController != null)
            {
                lootController.DropLoot(enemyConfig);
            }
        }
    }
}
