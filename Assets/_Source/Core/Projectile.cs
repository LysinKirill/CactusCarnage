using Core.Enemies;
using Core.Player;
using ScriptableObjects.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private LayerMask ignoreCollisionMask;
        [SerializeField] private ImpactPropertiesAsset impactProperties;
        public float Damage { get; private set; } = 1;

        public void SetDamage(float damage) => Damage = damage;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer & ignoreCollisionMask) != 0)
                return;
            
            
            if ((1 << other.gameObject.layer & enemyLayerMask) == 0)
            {
                Destroy(gameObject);
            }
            if (other.gameObject.TryGetComponent(out EnemyState enemyHealth))
            {
                enemyHealth.TakeDamage(Damage);
                ApplyKnockback(other);
                ApplyUpwardForce(other);
                Destroy(gameObject);
            } else if (other.gameObject.TryGetComponent(out PlayerState playerState))
            {
                playerState.TakeDamage((int)Damage);
                Destroy(gameObject);
            }
        }

        private void ApplyKnockback(Collider2D enemyCollider)
        {
            // if (!other.TryGetComponent(out Rigidbody2D enemyBody))
            //     return;
            //
            // var enemyCenterPos = other.bounds.center;
            // var bulletPos = transform.position;
            //
            // var knockbackDirection = (enemyCenterPos - bulletPos).normalized;
            // enemyBody.AddForce(knockbackDirection * impactProperties.KnockbackStrength);
            //
            
            ///////////////////////
            
            
            if (!enemyCollider.TryGetComponent(out Rigidbody2D enemyBody))
                return;
            
            var enemyCenterPos = enemyCollider.bounds.center;

            var knockbackDirection = (enemyCenterPos - transform.position).normalized;
            var knockbackStrength = impactProperties.KnockbackStrength;
            var force = knockbackDirection * knockbackStrength;
            var vector2 = enemyBody.velocity;
            vector2.x = 0;
            enemyBody.velocity = vector2;
            enemyBody.velocity += (Vector2)(force / enemyBody.mass);
        }

        private void ApplyUpwardForce(Collider2D enemyCollider)
        {
            // if (!other.TryGetComponent(out Rigidbody2D enemyBody))
            //     return;
            //
            // enemyBody.AddForce(Vector2.up * impactProperties.UpwardForce);
            
            if (!enemyCollider.TryGetComponent(out Rigidbody2D enemyBody))
                return;
            
            var upwardForce = impactProperties.UpwardForce * Vector2.up;
            enemyBody.velocity += upwardForce / enemyBody.mass;
        }
    }
}
