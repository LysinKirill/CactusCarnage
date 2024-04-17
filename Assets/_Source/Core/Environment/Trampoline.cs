using Core.Player;
using System;
using UnityEngine;

namespace Core.Environment
{
    public class Trampoline : MonoBehaviour
    {
        [SerializeField] private float bounceForce = 1f;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float stunDuration;

        private BoxCollider2D _boxCollider;
        private bool _isActive = true;
        
        private Vector2 BounceDirection
        {
            get
            {
                var angleDegrees = transform.rotation.eulerAngles.z;
                var angle = Mathf.Deg2Rad * angleDegrees + Mathf.PI / 2;
                var x = Mathf.Cos(angle);
                var y = Mathf.Sin(angle);
                return new Vector2(x, y);
            }
        }

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive || (1 << other.gameObject.layer &  playerLayer) == 0)
                return;
            GameObject player = other.transform.gameObject;

            if (player == null || !player.TryGetComponent(out Rigidbody2D playerBody))
                return;
            
            BouncePlayer(playerBody, BounceDirection);
        }

        private void BouncePlayer(Rigidbody2D playerBody, Vector2 direction)
        {
            var force = bounceForce * direction;
            var momentum = force / playerBody.mass;
            momentum.y = Mathf.Max(momentum.y, 5f);
            playerBody.velocity += momentum;
            
            if(playerBody.TryGetComponent(out PlayerState playerState))
                playerState.StunPlayer(stunDuration);
        }

        private void OnDrawGizmos()
        {
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, BounceDirection);
        }
    }
}
