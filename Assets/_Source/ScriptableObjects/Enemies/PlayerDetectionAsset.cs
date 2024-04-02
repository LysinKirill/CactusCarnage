using UnityEngine;

namespace ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "PlayerDetectionAsset", menuName = "SO/newPlayerDetectionAsset")]
    public class PlayerDetectionAsset : ScriptableObject
    {
        [field: SerializeField] public bool LineOfSightRequired { get; private set; }
        [field: SerializeField] public float DetectionRadius { get; private set; }
    }
}
