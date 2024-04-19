using ScriptableObjects.Enemies;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Core.Enemies.BasicCactus
{
    public class BasicEnemyMovement : MonoBehaviour
    {
        [SerializeField] private PlayerDetectionAsset playerDetection;
        [SerializeField] private BoxCollider2D checkGroundInFront;
        [SerializeField] private BoxCollider2D checkWallInFront;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private float speed;
        [SerializeField] private float maxWanderDistance;
        [SerializeField] private GameObject player;
        
        private Rigidbody2D _body;
        private float _walkTargetX;

        private bool _isGrounded = true;
        private bool _isTouchingWall;
        private bool _isFacingRight = true;
        private bool _isStunned;

        private bool _isResting;

        private readonly Random _random = new Random();
        private bool _playerDetected;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            if (TryGetComponent(out EnemyState health))
                health.OnTakeDamage += _ => StartCoroutine(GetStunned(0.5f));
        }
        
        private void Wander()
        {
            var calmWalkingSpeed = speed / 2;
            _body.velocity = new Vector2((_isFacingRight ? 1 : -1) * calmWalkingSpeed, _body.velocity.y);
        }

        private void FollowPlayer()
        {
            if (Mathf.Abs(_walkTargetX - transform.position.x) < 0.4f)
                return;
            var playerToTheRight = _walkTargetX > transform.position.x;
            
            _body.velocity = new Vector2((playerToTheRight ? 1 : -1) * speed, _body.velocity.y);
        }

        private void FixedUpdate()
        {
            if(_isStunned)
                return;
            if (!_playerDetected && CheckPlayer())
                _playerDetected = true;
            
            if (transform.IsDestroyed())
                return;
            if (_isResting)
                return;

            CheckGround();
            CheckWalls();
            UpdateWalkingTarget();

            if (_walkTargetX > transform.position.x && !_isFacingRight
                || _walkTargetX < transform.position.x && _isFacingRight)
                Flip();

            if (!_isGrounded || _isTouchingWall)
            {
                if (_playerDetected)
                {
                    var vector2 = _body.velocity;
                    vector2.x = 0;
                    _body.velocity = vector2;
                    return;
                }
                    
                StartMovingToOppositeDirection();
                return;
            }

            if (!_playerDetected)
            {
                Wander();
                return;
            }

            FollowPlayer();
        }

        private void StartMovingToOppositeDirection()
        {
            if (!_isGrounded || _isTouchingWall)
                Flip();
            
            if (_isFacingRight)
                _walkTargetX = (float)_random.NextDouble() * maxWanderDistance * 0.5f + maxWanderDistance * 0.5f;
            else
                _walkTargetX = -(float)_random.NextDouble() * maxWanderDistance * 0.5f - maxWanderDistance * 0.5f;

            _walkTargetX += transform.position.x;
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
            if (_playerDetected)
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
        
        private IEnumerator GetStunned(float stunDuration)
        {
            _isStunned = true;
            yield return new WaitForSecondsRealtime(stunDuration);
            _isStunned = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var startPosition = transform.position;
            var direction = (Vector2)(player.transform.position - startPosition);
            
            Gizmos.DrawRay(startPosition, direction);
        }
    }
}
