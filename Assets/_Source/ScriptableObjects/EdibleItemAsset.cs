using Model.ItemModifiers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EdibleItem", menuName = "SO/newEdibleItem")]
    public class EdibleItemAsset :
        ItemAsset,
        IDestroyableItem,
        IItemAction
    {
        [SerializeField] private List<ModifierData> modifiersData = new List<ModifierData>();
        [field: SerializeField] public AudioClip actionSFX { get; private set; }
        
        public string ActionName => "Consume";
        
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach (var data in modifiersData)
                data.statModifier.AffectCharacter(character, data.value);
            return true;
        }
    }

    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierAsset statModifier;
        public float value;
    }
}
