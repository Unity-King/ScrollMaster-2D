using UnityEngine;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "NewSpellConfig", menuName = "Configs/Character/Spell/New")]
    public class Spell : ScriptableObject
    {
        [Header("Informações Básicas")]
        public string spellName = "DefaultSpell";
        public float damage = 10f;
        public float speed = 5f;
        public KeyCode hotkey = KeyCode.None;

        [Header("Visual")]
        public GameObject spellPrefab;
        public Sprite spellSprite;
        public bool usePrefab = true;
    }
}