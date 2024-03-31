using Core.Enemies.RollingCactus;
using System;
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
        [field: SerializeField] public RollingEnemyState State { get; private set; }

        [field: SerializeField] public Rotation Rotation { get; private set; }

        
        
        public event Action<RollingEnemyState> OnStateChange;
        public event Action<Rotation> OnRotationChange;
        
        public void ChangeState(RollingEnemyState newState)
        {
            State = newState;
            OnStateChange?.Invoke(State);
        }

        public void ChangeRotation(Rotation rotation)
        {
            Rotation = rotation;
            OnRotationChange?.Invoke(Rotation);
        }
    }
    
    public enum Rotation
    {
        Clockwise,
        Counterclockwise,
    }
}
