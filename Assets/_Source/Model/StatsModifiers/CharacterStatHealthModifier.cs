using Core;
using System;
using UnityEngine;

namespace Model.ItemModifiers
{
    [CreateAssetMenu(fileName = "HealthModifier", menuName = "SO/newHealthModifier")]
    public class CharacterStatHealthModifier : CharacterStatModifierAsset
    {
        public override void AffectCharacter(GameObject character, float val)
        {
            PlayerData playerData = character.GetComponent<PlayerData>();
            if(playerData != null)
                playerData.AddHealth((int)val);
        }
    }
}
