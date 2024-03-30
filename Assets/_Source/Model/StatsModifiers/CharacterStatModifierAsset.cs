using UnityEngine;

namespace Model.ItemModifiers
{
    public abstract class CharacterStatModifierAsset : ScriptableObject
    {
        public abstract void AffectCharacter(GameObject character, float val);
    }
}
