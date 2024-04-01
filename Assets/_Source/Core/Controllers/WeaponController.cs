using ScriptableObjects.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponAsset weapon;
        [SerializeField] private InventoryAsset inventory;
        [SerializeField] private List<ItemParameter> parametersToModify;
        [SerializeField] private List<ItemParameter> currentItemState;
        [SerializeField] private SpriteRenderer _weaponRenderer;
        private List<GameObject> _projectiles = new List<GameObject>();
        public WeaponAsset CurrentWeapon
        {
            get { return weapon; }
        }
        

        public void SetWeapon(WeaponAsset weaponItemAsset, List<ItemParameter> itemState)
        {
            if (weapon != null)
                inventory.AddItem(weapon, 1, currentItemState);

            weapon = weaponItemAsset;
            currentItemState = new List<ItemParameter>(itemState);

            _weaponRenderer.sprite = CurrentWeapon.Sprite;
            ModifyParameters();
        }

        private void ModifyParameters()
        {
            foreach (var parameter in parametersToModify)
            {
                if (!currentItemState.Contains(parameter))
                    continue;

                var index = currentItemState.IndexOf(parameter);
                var newValue = currentItemState[index].value + parameter.value;

                currentItemState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue,
                };
            }
        }

        public void AddProjectile(GameObject projectile)
        {
            _projectiles.Add(projectile);
        }
    }
}
