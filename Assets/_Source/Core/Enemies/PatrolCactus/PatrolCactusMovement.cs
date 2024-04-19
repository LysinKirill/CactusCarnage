using System.Collections;
using UnityEngine;

namespace Core.Enemies.PatrolCactus
{
    public class PatrolCactusMovement : MonoBehaviour
    {
        [SerializeField] private GameObject pointA;
        [SerializeField] private GameObject pointB;
        [SerializeField] private float speed;
        [SerializeField] private Collider2D groundCheck;
        [SerializeField] private LayerMask obstacleLayerMask;
        
        private Rigidbody2D _body;
        private Transform _destination;
        private bool _isFacingRight;
        public bool _isGrounded = true;
        private bool _isStunned;
        
        private const float ReachRadius = 0.2f;
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _destination = pointA.transform;

            if (TryGetComponent(out EnemyState health))
                health.OnTakeDamage += _ => StartCoroutine(GetStunned(0.5f));
        }

        private IEnumerator GetStunned(float stunDuration)
        {
            _isStunned = true;
            yield return new WaitForSecondsRealtime(stunDuration);
            _isStunned = false;
        }


        private void FixedUpdate()
        {
            CheckGround();
            if (_isGrounded)
            {
                var vector2 = _body.velocity;
                vector2.y = 0;
                _body.velocity = vector2;
            }

            if (!_isGrounded || _isStunned)
                return;
            
            CheckDestinationReached();
            UpdateFacingDirection();
                
            
            var directionMultiplier = _destination.transform.position.x > transform.position.x ? 1 : -1;
            var velocity = _body.velocity;
            float y = 0;
            velocity = Vector2.right * (speed * directionMultiplier);
            velocity = new Vector2(velocity.x, y);
            _body.velocity = velocity;
        }

        private void CheckDestinationReached()
        {
            if (_destination == pointA.transform
                && Mathf.Abs(transform.position.x - pointA.transform.position.x) < ReachRadius)
                _destination = pointB.transform;

            if (_destination == pointB.transform
                && Mathf.Abs(transform.position.x - pointB.transform.position.x) < ReachRadius)
                _destination = pointA.transform;
        }
        
        private void CheckGround()
        {
            Vector2 originLeft = groundCheck.bounds.min;
            Vector2 originRight = groundCheck.bounds.max;
            float distance = 0.1f;
            Vector2 direction = Vector2.down;
            
            RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, direction, distance, obstacleLayerMask);
            RaycastHit2D hitRight = Physics2D.Raycast(originRight, direction, distance, obstacleLayerMask);
            
            _isGrounded = hitLeft.collider != null && hitRight.collider != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointA.transform.position, ReachRadius);
            Gizmos.DrawWireSphere(pointB.transform.position, ReachRadius);
        }

        private void UpdateFacingDirection()
        {
            if(_isFacingRight && _body.velocity.x < 0 
               || !_isFacingRight &&  _body.velocity.x > 0)
                Flip();
        }

        private void Flip()
        {
            _isFacingRight = !_isFacingRight;
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
