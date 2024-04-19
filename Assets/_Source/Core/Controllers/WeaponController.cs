using ScriptableObjects.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponAsset weapon;
        [SerializeField] private InventoryAsset inventory;
        [SerializeField] private List<ItemParameter> parametersToModify;
        [SerializeField] private List<ItemParameter> currentItemState;
        [SerializeField] private SpriteRenderer weaponRenderer;
        [SerializeField] private GameObject activeWeaponSlot;
        public Animator animator;
        private static readonly int HandEquiped = Animator.StringToHash("HandEquiped");
        private static readonly int MacheteEquiped = Animator.StringToHash("MacheteEquiped");
        private static readonly int PistolEquiped = Animator.StringToHash("PistolEquiped");
        public WeaponAsset CurrentWeapon
        {
            get { return weapon; }
        }

        private void Awake()
        {
            UpdateActiveWeaponSlot();
        }

        private void FixedUpdate()
        {
            if (weapon == null)
            {
                animator.SetBool(HandEquiped, true);
                animator.SetBool(MacheteEquiped, false);
                animator.SetBool(PistolEquiped, false);
            } 
            else switch (weapon.name)
            {
                case "Machete":
                    animator.SetBool(HandEquiped, false);
                    animator.SetBool(MacheteEquiped, true);
                    animator.SetBool(PistolEquiped, false);
                    break;
                case "Pistol":
                    animator.SetBool(HandEquiped, false);
                    animator.SetBool(MacheteEquiped, false);
                    animator.SetBool(PistolEquiped, true);
                    break;
            }
        }


        public void SetWeapon(WeaponAsset weaponItemAsset, List<ItemParameter> itemState)
        {
            if (weapon != null)
                inventory.AddItem(weapon, 1, currentItemState);

            weapon = weaponItemAsset;
            currentItemState = new List<ItemParameter>(itemState);

            weaponRenderer.sprite = CurrentWeapon.Sprite;
            UpdateActiveWeaponSlot();
            ModifyParameters();
        }

        public void UpdateActiveWeaponSlot()
        {
            if (weapon == null)
            {
                activeWeaponSlot.SetActive(false);
                return;
            }
            activeWeaponSlot.SetActive(true);
            activeWeaponSlot.GetComponent<Image>().sprite = weapon.Sprite;
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
