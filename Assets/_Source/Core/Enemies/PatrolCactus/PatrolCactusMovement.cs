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
        private bool _isGrounded = true;
        private bool _isStunned;
        
        private const float ReachRadius = 0.2f;
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _destination = pointA.transform;

            if (TryGetComponent(out EnemyHealth health))
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
            if (!_isGrounded || _isStunned)
                return;
            
            CheckDestinationReached();
            UpdateFacingDirection();
            var directionMultiplier = _destination.transform.position.x > transform.position.x ? 1 : -1;
            var velocity = _body.velocity;
            //float y = velocity.y;
            float y = 0;
            velocity = Vector2.right * (speed * directionMultiplier);
            velocity = new Vector2(velocity.x, y);
            _body.velocity = velocity;
        }

        private void CheckDestinationReached()
        {
            if (_destination == pointA.transform
                && Vector2.Distance(transform.position, pointA.transform.position) < ReachRadius)
                _destination = pointB.transform;

            if (_destination == pointB.transform
                && Vector2.Distance(transform.position, pointB.transform.position) < ReachRadius)
                _destination = pointA.transform;
        }
        
        private void CheckGround()
        {
            var bounds = groundCheck.bounds;
            _isGrounded = Physics2D.OverlapAreaAll(bounds.min, bounds.max, obstacleLayerMask).Length > 0;
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
