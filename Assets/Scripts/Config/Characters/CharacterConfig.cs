using UnityEngine;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "NewCharacterConfig", menuName = "Configs/Character/New")]
    public class Character : ScriptableObject
    {
        [Header("Informações Básicas")]
        public string characterName = "Default";
        public int level = 1;

        [Header("Atributos")]
        public float maxHealth = 100f;
        public float attackPower = 10f;
        public float defense = 5f;
        public float moveSpeed = 5f;

        [Header("Habilidades")]
        public Spell[] spells;

        [Header("Animação")]
        public RuntimeAnimatorController animatorController;
    }
}