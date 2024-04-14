using UnityEngine;

namespace ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "ImpactProperties", menuName = "SO/Items/newImpactProperties")]
    public class ImpactPropertiesAsset : ScriptableObject
    {
        [field: SerializeField] public float KnockbackStrength { get; private set; }
        [field: SerializeField] public float UpwardForce { get; private set; }
    }
}