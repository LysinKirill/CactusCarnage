using System;
using System.Collections;
using UnityEngine;

namespace Core.Enemies
{
    public class EnemyState : MonoBehaviour
    {
        [field: SerializeField] public float MaxHealth { get; private set; }
        [SerializeField] private SpriteRenderer healthBar;
        [SerializeField] private float takeDamageAnimationDuration;

        private SpriteRenderer _spriteRenderer;
        public float Health { get; private set; }
        public event Action<GameObject> OnDeath;
        public event Action<float> OnTakeDamage;
        
        private void Awake()
        {
            Health = MaxHealth;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            OnDeath += HandleDeath;
        }

        private void HandleDeath(GameObject obj)
        {
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            var scale = healthBar.gameObject.transform.localScale;
            scale.x = Health / MaxHealth;
            healthBar.gameObject.transform.localScale = scale;
        }

        public void TakeDamage(float damage)
        {
            if (damage <= 0)
                return;

            Health -= damage;
            OnTakeDamage?.Invoke(damage);
            
            if(IsDead())
            {
                OnDeath?.Invoke(gameObject);
                return;
            }
            StartCoroutine(AnimateTakeDamage(takeDamageAnimationDuration));
        }
        

        public bool IsDead() => Health <= 0;
        
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
        
    }
}
