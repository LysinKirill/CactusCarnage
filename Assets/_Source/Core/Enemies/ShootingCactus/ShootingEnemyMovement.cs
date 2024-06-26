﻿using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Core.Enemies.ShootingCactus
{
    public class ShootingEnemyMovement : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D checkGroundInFront;
        [SerializeField] private BoxCollider2D checkWallInFront;
        [SerializeField] private float speed;
        [SerializeField] private float maxWanderDistance;
        [SerializeField] private GameObject player;
        [SerializeField] private LayerMask obstacleLayerMask;


        private ShootingEnemyController _controller;
        private Rigidbody2D _body;
        private float _walkTargetX;

        private bool _isGrounded = true;
        private bool _isTouchingWall;
        private bool _isFacingRight = true;
        

        private bool _isResting;

        private readonly Random _random = new Random();
        

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _controller = GetComponent<ShootingEnemyController>();
        }
        
        private void Wander() 
            => _body.velocity = new Vector2((_isFacingRight ? 1 : -1) * speed, _body.velocity.y);


        private void FixedUpdate()
        {
            if(_controller.IsStunned)
                return;
            
            if (transform.IsDestroyed() && _isResting)
                return;

            if (_controller.PlayerDetected)
            {
                var playerPosX = player.transform.position.x;
                if (playerPosX > transform.position.x && !_isFacingRight
                    || playerPosX < transform.position.x && _isFacingRight)
                    Flip();
                return;
            }
            
            CheckGround();
            CheckWalls();
            UpdateWalkingTarget();

            if (_walkTargetX > transform.position.x && !_isFacingRight
                || _walkTargetX < transform.position.x && _isFacingRight)
                Flip();

            if (!_isGrounded || _isTouchingWall)
            {
                var vector2 = _body.velocity;
                vector2.x = 0;
                _body.velocity = vector2;
                return;
            }
            
            Wander();
        }
        

        private void CheckWalls()
        {
            var bounds = checkWallInFront.bounds;
            _isTouchingWall = Physics2D.OverlapAreaAll(bounds.min, bounds.max, obstacleLayerMask).Length > 0;
        }

        private void CheckGround()
        {
            var bounds = checkGroundInFront.bounds;
            _isGrounded = Physics2D.OverlapAreaAll(bounds.min, bounds.max, obstacleLayerMask).Length > 0;
        }

        private void UpdateWalkingTarget()
        {
            if (_controller.PlayerDetected)
                _walkTargetX = player.transform.position.x;
            else if (Mathf.Abs(_walkTargetX - transform.position.x) < 0.1f)
            {
                var restTime = (float)_random.NextDouble() * 0.5f + 0.5f;
                StartCoroutine(RestOnTarget(restTime));
                SetNewWalkingTarget();
            }
        }

        private IEnumerator RestOnTarget(float restTime)
        {
            _isResting = true;
            yield return new WaitForSecondsRealtime(restTime);
            _isResting = false;
        }

        private void SetNewWalkingTarget()
        {
            var newTargetShift = (float)_random.NextDouble() * maxWanderDistance / 2 + maxWanderDistance / 2;
            if (_random.Next(0, 2) == 1)
                newTargetShift *= -1;
            _walkTargetX =
                transform.position.x + newTargetShift;
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
