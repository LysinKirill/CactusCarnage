﻿using Core.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Items
{
    public abstract class WeaponAsset : 
        ItemAsset,
        IDestroyableItem,
        IItemAction
    {
        [field: SerializeField] public AudioClip ActionSfx { get; private set; }
        [field: SerializeField] public float AttackDelay { get; private set; }
        public string ActionName => "Выбрать";
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            WeaponController weaponController = character.GetComponent<WeaponController>();
            if (weaponController == null)
                return false;
            weaponController.SetWeapon(this, itemState ?? DefaultParameterList);
            return true;
        }
    }
}
