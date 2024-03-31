using System;
using UnityEngine;

namespace Core.Enemies.PatrolCactus
{
    public class PatrolCactusMovement : MonoBehaviour
    {
        [SerializeField] private GameObject pointA;
        [SerializeField] private GameObject pointB;
        [SerializeField] private float speed;
        
        private Rigidbody2D _body;
        private Transform _destination;
        private bool _isFacingRight;
        
        
        private const float ReachRadius = 0.2f;
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _destination = pointA.transform;
        }

        private void FixedUpdate()
        {
            CheckDestinationReached();
            UpdateFacingDirection();
            var directionMultiplier = _destination.transform.position.x > transform.position.x ? 1 : -1;
            _body.velocity = Vector2.right * (speed * directionMultiplier);
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
