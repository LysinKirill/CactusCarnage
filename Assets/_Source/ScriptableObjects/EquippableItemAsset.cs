using Core;
using Core.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "SO/newEquippableItem")]
    public class EquippableItemAsset : 
        ItemAsset,
        IDestroyableItem,
        IItemAction
    {

        [field: SerializeField] public AudioClip actionSFX { get; private set; }
        public string ActionName => "Equip";
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            WeaponController weaponSystem = character.GetComponent<WeaponController>();
            if (weaponSystem == null)
                return false;
            weaponSystem.SetWeapon(this, itemState ?? DefaultParameterList);
            return true;
        }
    }
}
