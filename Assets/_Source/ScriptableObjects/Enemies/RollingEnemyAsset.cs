using Core.Enemies.RollingCactus;
using UnityEngine;

namespace ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "RollingEnemyAsset", menuName = "SO/Enemies/newRollingEnemy")]
    public class RollingEnemyAsset : ScriptableObject
    {
        [field: SerializeField] public float PlayerDetectionDistance { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float AirTossPower { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float AttackDelay { get; private set; }
    }
}
