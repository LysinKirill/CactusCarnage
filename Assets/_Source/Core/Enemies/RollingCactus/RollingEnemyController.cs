using Core.Player;
using ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Core.Enemies.RollingCactus
{
    public class RollingEnemyController : MonoBehaviour
    {
        [SerializeField] private float playerDetectionDistance;
        [SerializeField] private GameObject player;
        [SerializeField] private float speed;
        [SerializeField] private float damageDelay;
        //private Animator _animator;
        private RollingEnemyState _state = RollingEnemyState.Sleeping;
        private Collider2D _enemyCollider;
        private Collider2D _playerCollider;

        private void Awake()
        {
            _enemyCollider = GetComponent<Collider2D>();
            _playerCollider = player.GetComponent<Collider2D>();
        }

        private void Update()
        {
            switch(_state)
            {
                case RollingEnemyState.Sleeping:
                    CheckPlayer();
                    break;
                case RollingEnemyState.Rolling:
                    CheckPlayerHitbox();
                    break;
                case RollingEnemyState.Static:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckPlayerHitbox()
        {
            
        }

        private void CheckPlayer()
        {
            RaycastHit2D forwardRay = Physics2D.Raycast(transform.position, Vector2.right, playerDetectionDistance);
            if (forwardRay.collider != null && forwardRay.collider.TryGetComponent(out PlayerHealth _))
            {
                StartRollingClockwise();
                return;
            }
            
            RaycastHit2D backwardRay = Physics2D.Raycast(transform.position, Vector2.left, playerDetectionDistance);
            if (backwardRay.collider != null && backwardRay.collider.TryGetComponent(out PlayerHealth _))
            {
                StartRollingCounterClockwise();
                return;
            }
        }

        private void StartRollingCounterClockwise()
        {
            _state = RollingEnemyState.Rolling;
            
            //StartCoroutine(RollCounterClockwise());
        }

        // private IEnumerator RollCounterClockwise()
        // {
        //     StartRollingCounterClockwise();
        // }

        private void StartRollingClockwise()
        {
            _state = RollingEnemyState.Rolling;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerHealth playerHealth))
                return;
            
            
        }
    }
}
