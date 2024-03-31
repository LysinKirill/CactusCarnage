using ScriptableObjects;
using ScriptableObjects.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private EquippableItemAsset weapon;
        [SerializeField] private InventoryAsset inventory;
        [SerializeField] private List<ItemParameter> parametersToModify;
        [SerializeField] private List<ItemParameter> currentItemState;

        public void SetWeapon(EquippableItemAsset weaponItemAsset, List<ItemParameter> itemState)
        {
            if (weapon != null)
                inventory.AddItem(weapon, 1, currentItemState);

            weapon = weaponItemAsset;
            currentItemState = new List<ItemParameter>(itemState);

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
    }
}
