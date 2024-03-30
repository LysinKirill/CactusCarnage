using System;
using UnityEngine;

namespace Core
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private int healthPoints = 0;
        [field: SerializeField] public int MaximumHealth { get; private set; } = 0;
        
        public int HealthPoints
        {
            get { return healthPoints; }
            private set
            {
                healthPoints = value;
                OnUpdateHealth?.Invoke(value);
            }
        }
        

        public event Action<int> OnUpdateHealth;
        
        public void AddHealth(int amount)
        {
            HealthPoints = Mathf.Clamp(HealthPoints + amount, 0, MaximumHealth);
        }

        public void TakeDamage(int damage) => AddHealth(-damage);
    }
}
