using Core.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.Environment
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private float damageDelay = 1f;
        [SerializeField] private LayerMask playerLayer;

        private bool _isActive = true;

        private void Awake()
        {
            GetComponent<TilemapCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive || (1 << other.gameObject.layer &  playerLayer) == 0)
                return;
            GameObject player = other.transform.gameObject;

            if (player == null || !player.TryGetComponent(out PlayerHealth playerHealth))
                return;

            playerHealth.TakeDamage(damage);
            StartCoroutine(DamageDelay(damageDelay));
        }

        private IEnumerator DamageDelay(float delayDuration)
        {
            _isActive = false;
            float currentTime = 0;
            while (currentTime < delayDuration)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
            _isActive = true;
        }
    }
}
