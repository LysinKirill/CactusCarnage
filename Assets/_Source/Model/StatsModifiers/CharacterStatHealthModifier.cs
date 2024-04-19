using Core.Player;
using UnityEngine;

namespace Model.StatsModifiers
{
    [CreateAssetMenu(fileName = "HealthModifier", menuName = "SO/newHealthModifier")]
    public class CharacterStatHealthModifier : CharacterStatModifierAsset
    {
        public override void AffectCharacter(GameObject character, float val)
        {
            PlayerState playerData = character.GetComponent<PlayerState>();
            if(playerData != null)
                playerData.AddHealth((int)val);
        }
    }
}
