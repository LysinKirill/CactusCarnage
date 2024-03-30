using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float takeDamageAnimationDuration = 0.2f;
        [SerializeField] private int healthPoints;
        [field: SerializeField] public int MaximumHealth { get; private set; }
        
        public event Action<int> OnUpdateHealth;
        public event Action<int> OnUpdateMaximumHealth;

        private SpriteRenderer _spriteRenderer;
        
        public int HealthPoints
        {
            get { return healthPoints; }
            private set
            {
                healthPoints = value;
                OnUpdateHealth?.Invoke(value);
            }
        }


        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void AddHealth(int amount)
        {
            HealthPoints = Mathf.Clamp(HealthPoints + amount, 0, MaximumHealth);
        }

        public void TakeDamage(int damage)
        {
            if(damage < 0)
                return;
            AddHealth(-damage);
            StartCoroutine(AnimateTakeDamage(takeDamageAnimationDuration));
        }

        private IEnumerator AnimateTakeDamage(float duration)
        {
            Color damagedColor = new Color(1, 100f / 255, 100f / 255);
            float currentTime = 0;
            duration /= 2;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                _spriteRenderer.color = Color.Lerp(Color.white, damagedColor, currentTime / duration);
                yield return null;
            }

            currentTime = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                _spriteRenderer.color = Color.Lerp(damagedColor, Color.white, currentTime / duration);
                yield return null;
            }
        }

        public void ChangeMaxHealth(int newMaxHealth)
        {
            MaximumHealth = Mathf.Max(1, newMaxHealth);
            OnUpdateMaximumHealth?.Invoke(MaximumHealth);
        }
    }
}
