using ScriptableObjects.Enemies;
using System.Collections;
using UnityEngine;

namespace Core.Enemies.ShootingCactus
{
    public class ShootingEnemyController : MonoBehaviour
    {
        [SerializeField] private PlayerDetectionAsset playerDetection;
        [SerializeField] private GameObject player;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float attackDelay;
        public bool IsStunned { get; private set; }
        public bool PlayerDetected { get; private set; }


        private bool _attackCooldownActive;


        private void Awake()
        {
            if (TryGetComponent(out EnemyState health))
                health.OnTakeDamage += _ => StartCoroutine(GetStunned(0.5f));
        }

        private void FixedUpdate()
        {
            if(IsStunned)
                return;
            PlayerDetected = CheckPlayer();

            if (!_attackCooldownActive && PlayerInShootingRadius())
                Shoot();
        }

        private bool PlayerInShootingRadius()
        {
            return Vector2.Distance(player.transform.position, transform.position) <= playerDetection.DetectionRadius;
        }

        private void Shoot()
        {
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            if (projectile.TryGetComponent(out Projectile proj))
                proj.SetDamage(proj.Damage);

            if (projectile.TryGetComponent(out Rigidbody2D projectileBody))
            {
                var shootDirection = player.transform.position - transform.position;
                projectileBody.velocity = shootDirection.normalized * projectileSpeed;
                
                float angle = Mathf.Atan2(projectileBody.velocity.y, projectileBody.velocity.x) * Mathf.Rad2Deg;
                projectileBody.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            
            StartCoroutine(AttackCooldown(attackDelay));
        }
        
        private bool CheckPlayer()
        {
            if (playerDetection.LineOfSightRequired && LineOfSightObstructed())
                return false;

            var distance = Vector3.Distance(transform.position, player.transform.position);
            return distance <= playerDetection.DetectionRadius;
        }
        
        
        private bool LineOfSightObstructed()
        {
            var startPosition = transform.position;

            var direction = (Vector2)(player.transform.position - startPosition);
            RaycastHit2D hit = Physics2D.Raycast(
                startPosition,
                direction,
                direction.magnitude,
                obstacleLayerMask
            );
            if (hit.collider != null)
                return true;

            return Vector2.Distance(startPosition, player.transform.position) >= playerDetection.DetectionRadius;
        }
        
        private IEnumerator AttackCooldown(float delay)
        {
            _attackCooldownActive = true;
            yield return new WaitForSecondsRealtime(delay);
            _attackCooldownActive = false;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var startPosition = transform.position;
            var direction = (Vector2)(player.transform.position - startPosition);
            
            Gizmos.DrawRay(startPosition, direction);
        }
        
        private IEnumerator GetStunned(float stunDuration)
        {
            IsStunned = true;
            yield return new WaitForSecondsRealtime(stunDuration);
            IsStunned = false;
        }
    }
}
