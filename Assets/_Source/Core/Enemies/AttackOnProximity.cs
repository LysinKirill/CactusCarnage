using Core.Player;
using System.Collections;
using UnityEngine;

namespace Core.Enemies
{
    public class AttackOnProximity : MonoBehaviour
    {
        [SerializeField] private float attackDelay = 1f;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private int damage = 1;
        [SerializeField] private BoxCollider2D attackBoxCollider;
        private bool _canAttack = true;
        

        private void Update()
        {
            if(!_canAttack || !PlayerInAttackBox(out _))
                return;
            AttackPlayer(damage);
        }


        private void AttackPlayer(int enemyDamage)
        {
            PlayAttackAnimation();
            if(PlayerInAttackBox(out PlayerHealth playerHealth))
                playerHealth.TakeDamage(enemyDamage);
            StartCoroutine(StartAttackDelay(attackDelay));
        }

        private IEnumerator StartAttackDelay(float delay)
        {
            _canAttack = false;
            yield return new WaitForSecondsRealtime(delay);
            _canAttack = true;
        }

        private bool PlayerInAttackBox(out PlayerHealth playerHealth)
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

        private void PlayAttackAnimation()
        {
            StartCoroutine(StartAttackAnimation(1f));
        }

        private IEnumerator StartAttackAnimation(float duration)
        {
            var position = transform.position;
            var startPosition = position;
            var newPosition = position - Vector3.right;
            float currentTime = 0;

            while (currentTime < duration / 2)
            {
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, newPosition, currentTime * 2 / duration);
                yield return null;
            }

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(newPosition, startPosition, (currentTime - duration / 2) * 2 / duration);
                yield return null;
            }

            transform.position = startPosition;
        }
    }
}
