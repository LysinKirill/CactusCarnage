using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Player
{
    public class PlayerState : MonoBehaviour
    {
        [SerializeField] private float takeDamageAnimationDuration = 0.2f;
        [SerializeField] private int healthPoints;
        [SerializeField] private Slider ultimateSlider;
        [SerializeField] private float ultimateGainOnReceiveDamage;
        [field: SerializeField] public float UltimateGainOnDealDamage { get; private set; }
        
        [field: SerializeField] public int MaximumHealth { get; private set; }
        
        private float _ultimateProgress;

        public bool IsUltimateReady => _ultimateProgress >= 1;
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
            ClearFollowers();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnDestroy()
        {
            ClearFollowers();
        }

        public void AddUltimateProgress(float progress)
        {
            _ultimateProgress = Mathf.Clamp(progress + _ultimateProgress, 0, 1);
            ultimateSlider.value = _ultimateProgress;
        }

        private void ClearFollowers()
        {
            OnUpdateHealth = null;
            OnUpdateMaximumHealth = null;
        }

        public void AddHealth(int amount)
        {
            HealthPoints = Mathf.Clamp(HealthPoints + amount, 0, MaximumHealth);
        }

        public void TakeDamage(int damage)
        {
            if(damage < 0)
                return;
            StartCoroutine(AnimateTakeDamage(takeDamageAnimationDuration));
            AddHealth(-damage);
            AddUltimateProgress(ultimateGainOnReceiveDamage);
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
