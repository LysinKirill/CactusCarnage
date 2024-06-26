﻿ using System;
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
        [SerializeField] private GameObject sliderHandle;
        [SerializeField] private float ultimateGainOnReceiveDamage;
        [field: SerializeField] public float UltimateGainOnDealDamage { get; private set; }
        
        [field: SerializeField] public int MaximumHealth { get; private set; }
        [field: SerializeField] public float UltimateDuration { get; private set; } = 1f;
        
        public bool IsUltimateActive { get; private set; } = false;
        public bool IsUltimateReady
        {
            get { return Mathf.Approximately(_ultimateProgress, 1f); }
        }
        
        public bool IsStunned { get; private set; }
        
        private float _ultimateProgress;
        
        
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

        private void Update()
        {
            if (IsUltimateReady && Input.GetKeyDown(KeyCode.X))
                UseUltimate();
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

        public void UseUltimate()
        {
            IsUltimateActive = true;
            StartCoroutine(ResetUltimate(UltimateDuration));
        }

        private IEnumerator ResetUltimate(float ultimateDuration)
        {
            sliderHandle.SetActive(true);
            float currentTime = 0;
            while (currentTime < ultimateDuration)
            {
                currentTime += Time.deltaTime;
                ultimateSlider.value = Mathf.Lerp(1, 0, currentTime / ultimateDuration);
                _ultimateProgress = ultimateDuration - currentTime;
                yield return null;
            }
            IsUltimateActive = false;
            sliderHandle.SetActive(false);
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
        public void StunPlayer(float stunDuration)
        {
            StartCoroutine(StartStun(stunDuration));
        }

        private IEnumerator StartStun(float stunDuration)
        {
            IsStunned = true;
            yield return new WaitForSecondsRealtime(stunDuration);
            IsStunned = false;
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
