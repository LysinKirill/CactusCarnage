using Core.Player;
using ScriptableObjects.Enemies;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Enemies.RollingCactus
{
    public class RollingEnemyMeleeAttack : MonoBehaviour
    {
        [SerializeField] private RollingEnemyAsset asset;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private BoxCollider2D attackBoxCollider;
        [SerializeField] private GameObject player;
        
        private bool _canAttack = true;

        private void Awake()
        {
            SceneManager.sceneUnloaded += _ => StopAllCoroutines();
        }

        private void Update()
        {
            if(!_canAttack || !PlayerInAttackCollider(out _))
                return;
            AttackPlayer(asset.Damage);
        }


        private void AttackPlayer(int enemyDamage)
        {
            if(asset.State == RollingEnemyState.Rolling)
                ThrowPlayerInTheAir();
            
            if(PlayerInAttackCollider(out PlayerHealth playerHealth))
                playerHealth.TakeDamage(enemyDamage);
            asset.ChangeState(RollingEnemyState.Static);
            
            StartCoroutine(StartAttackDelay(asset.AttackDelay));
        }

        private IEnumerator StartAttackDelay(float delay)
        {
            _canAttack = false;
            yield return new WaitForSecondsRealtime(delay);
            _canAttack = true;
        }

        private bool PlayerInAttackCollider(out PlayerHealth playerHealth)
        {
            playerHealth = null;
            var bounds = attackBoxCollider.bounds;
            var center = new Vector2(bounds.center.x, bounds.center.y);
            RaycastHit2D hit =
                Physics2D.BoxCast(center, bounds.size, 0, Vector2.right, 0, playerLayer);
            if (hit.collider == null)
                return false;
            hit.collider.gameObject.TryGetComponent(out playerHealth);
            return true;
        }

        private void OnDrawGizmos()
        {
            if(attackBoxCollider == null)
                return;
            
            var bounds = attackBoxCollider.bounds;
            var center = new Vector2(bounds.center.x, bounds.center.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, bounds.size);
        }

        private void ThrowPlayerInTheAir()
        {
            if (!player.TryGetComponent(out Rigidbody2D body))
                return;

            var newVelocity = body.velocity;
            newVelocity.y = asset.AirTossPower;
            body.velocity = newVelocity;
        }
    }
}
