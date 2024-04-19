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
        
        // private void CheckGround()
        // {
        //     var bounds = groundCheck.bounds;
        //     _isGrounded = Physics2D.OverlapAreaAll(bounds.min, bounds.max, obstacleLayerMask).Length > 0;
        // }
        
        // private void CheckGround()
        // {
        //     Vector2 center = groundCheck.bounds.center; // Center of the groundCheck collider
        //     Vector2 size = groundCheck.bounds.size; // Size of the groundCheck collider
        //
        //     float distance = 0.1f; // Distance to cast the box (adjust as needed)
        //     Vector2 direction = Vector2.down; // Direction to cast the box (downward for ground check)
        //
        //     RaycastHit2D hit = Physics2D.BoxCast(center, size, 0f, direction, distance, obstacleLayerMask);
        //
        //     _isGrounded = hit.collider != null; // Check if the box cast hit any colliders
        // }
        
        // private void CheckGround()
        // {
        //     Vector2 origin = groundCheck.bounds.center; // Start position of the raycast
        //     float distance = 0.1f; // Distance to cast the ray (adjust as needed)
        //     Vector2 direction = Vector2.down; // Direction to cast the ray (downward for ground check)
        //
        //     // Perform the raycast
        //     RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, obstacleLayerMask);
        //
        //     // Check if the raycast hit anything
        //     _isGrounded = hit.collider != null;
        // }
        
        private void CheckGround()
        {
            Vector2 originLeft = groundCheck.bounds.min; // Start position of the left raycast
            Vector2 originRight = groundCheck.bounds.max; // Start position of the right raycast
            float distance = 0.1f; // Distance to cast the ray (adjust as needed)
            Vector2 direction = Vector2.down; // Direction to cast the ray (downward for ground check)

            // Perform the left raycast
            RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, direction, distance, obstacleLayerMask);

            // Perform the right raycast
            RaycastHit2D hitRight = Physics2D.Raycast(originRight, direction, distance, obstacleLayerMask);

            // Check if both rays hit something
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
