using Core.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class HeartVisuals : MonoBehaviour
    {
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private float activateHeartDuration = 1f;

        private int _currentHealth;
        private List<GameObject> Hearts { get; } = new List<GameObject>();

        private void Awake()
        {
            playerHealth.OnUpdateHealth += HandleHealthUpdate;
            playerHealth.OnUpdateMaximumHealth += HandleMaxHealthUpdate;
        }
        

        void Start()
        {
            _currentHealth = playerHealth.HealthPoints;
            for (int i = 0; i < playerHealth.MaximumHealth; ++i)
            {
                var heart = Instantiate(heartPrefab, transform, false);
                heart.transform.SetSiblingIndex(i);
                Hearts.Add(heart);
                if(i >= _currentHealth)
                    heart.gameObject.SetActive(false);
            }
        }

        private void HandleHealthUpdate(int newHealth)
        {
            while (newHealth > _currentHealth && _currentHealth < playerHealth.MaximumHealth)
            {
                ++_currentHealth;
                ActivateHeart();
            }

            while (newHealth < _currentHealth && _currentHealth > 0)
            {
                --_currentHealth;
                DeactivateHeart();
            }
        }



        private void HandleMaxHealthUpdate(int obj)
        {
            throw new NotImplementedException();
        }

        private void ActivateHeart()
        {
            var firstInactiveHeart = Hearts.First(heart => !heart.activeInHierarchy);
            firstInactiveHeart.SetActive(true);
            StartCoroutine(AnimateHeartActivation(firstInactiveHeart, activateHeartDuration));
        }
        private void DeactivateHeart()
        {
            if (!gameObject.activeInHierarchy) return;
            var lastActiveHeart = Hearts[_currentHealth];
            StartCoroutine(AnimateHeartDeactivation(lastActiveHeart, activateHeartDuration));
        }
        private IEnumerator AnimateHeartActivation(GameObject heart, float duration)
        {
            float currentTime = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                heart.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, currentTime / duration);
                yield return null;
            }
            heart.transform.localScale = Vector3.one;
        }
        
        private IEnumerator AnimateHeartDeactivation(GameObject heart, float duration)
        {
            float currentTime = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                heart.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, currentTime / duration);
                yield return null;
            }
            heart.transform.localScale = Vector3.zero;
            heart.SetActive(false);
        }
    }
}

