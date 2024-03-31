using UnityEngine;

namespace Model.StatsModifiers
{
    public abstract class CharacterStatModifierAsset : ScriptableObject
    {
        public abstract void AffectCharacter(GameObject character, float val);
    }
}
