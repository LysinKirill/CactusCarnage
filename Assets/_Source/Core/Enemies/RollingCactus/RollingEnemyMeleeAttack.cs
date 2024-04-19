using Core.Player;
using ScriptableObjects.Enemies;
using System.Collections;
using UnityEngine;

namespace Core.Enemies.RollingCactus
{
    public class RollingEnemyMeleeAttack : MonoBehaviour
    {
        [SerializeField] private RollingEnemyAsset asset;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private BoxCollider2D attackBoxCollider;
        [SerializeField] private GameObject player;
        
        private bool _canAttack = true;
        private RollingEnemyController _controller;

        private void Awake()
        {
            _controller = GetComponent<RollingEnemyController>();
        }

        private void Update()
        {
            if(!_canAttack || !PlayerInAttackCollider(out _))
                return;
            AttackPlayer(asset.Damage);
        }


        private void AttackPlayer(int enemyDamage)
        {
            if(_controller.State == RollingEnemyState.Rolling)
                ThrowPlayerInTheAir();
            
            if(PlayerInAttackCollider(out PlayerState playerHealth))
                playerHealth.TakeDamage(enemyDamage);
            _controller.ChangeState(RollingEnemyState.Static) ;
            
            StartCoroutine(StartAttackDelay(asset.AttackDelay));
        }

        private IEnumerator StartAttackDelay(float delay)
        {
            _canAttack = false;
            yield return new WaitForSecondsRealtime(delay);
            _canAttack = true;
        }

        private bool PlayerInAttackCollider(out PlayerState playerState)
        {
            playerState = null;
            var bounds = attackBoxCollider.bounds;
            var center = new Vector2(bounds.center.x, bounds.center.y);
            RaycastHit2D[] hits =
                Physics2D.BoxCastAll(center, bounds.size, 0, Vector2.right, 0, playerLayer);

            foreach (var hit in hits)
                if (hit.collider.gameObject.TryGetComponent(out playerState))
                    return true;
            
            return false;
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
