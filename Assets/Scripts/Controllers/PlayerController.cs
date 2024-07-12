using UnityEngine;
using ScrollMaster2D.Config.Character;

public class PlayerController : MonoBehaviour
{
    public CharacterConfig characterConfig;
    private Animator animator;
    private Rigidbody2D rb;

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
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }
        animator.runtimeAnimatorController = characterConfig.animatorController;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * characterConfig.moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, characterConfig.moveSpeed);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    private void HandleAttacks()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            // Lógica de ataque aqui
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

    private void CastSpell(SpellConfig spell)
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
}
