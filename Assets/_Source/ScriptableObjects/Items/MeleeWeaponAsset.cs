using UnityEngine;

namespace ScriptableObjects.Items
{
    [CreateAssetMenu(menuName = "SO/Weapon/newMeleeWeapon")]
    public class MeleeWeaponAsset : WeaponAsset
    {
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
    }
}
