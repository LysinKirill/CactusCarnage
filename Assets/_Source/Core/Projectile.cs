using Core.Enemies;
using UnityEngine;

namespace Core
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private LayerMask IgnoreColliionMask;
        private float _damage = 1;

        public void SetDamage(float damage) => _damage = damage;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer & IgnoreColliionMask) != 0)
                return;
            if ((1 << other.gameObject.layer & enemyLayerMask) == 0)
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
