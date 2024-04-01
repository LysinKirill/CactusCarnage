using UnityEngine;

namespace ScriptableObjects.Items
{
    [CreateAssetMenu(menuName = "SO/Weapon/newRangedWeapon")]
    public class RangedWeaponAsset : WeaponAsset
    {
        [field: SerializeField] public float PreparationTime { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }
    }
}
