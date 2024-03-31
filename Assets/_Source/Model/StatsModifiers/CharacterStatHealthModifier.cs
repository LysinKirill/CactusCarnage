using Core.Player;
using UnityEngine;

namespace Model.StatsModifiers
{
    [CreateAssetMenu(fileName = "HealthModifier", menuName = "SO/newHealthModifier")]
    public class CharacterStatHealthModifier : CharacterStatModifierAsset
    {
        public override void AffectCharacter(GameObject character, float val)
        {
            PlayerHealth playerData = character.GetComponent<PlayerHealth>();
            if(playerData != null)
                playerData.AddHealth((int)val);
        }
    }
}
