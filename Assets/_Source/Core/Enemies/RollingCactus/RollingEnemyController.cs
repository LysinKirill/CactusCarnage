using Core.Player;
using ScriptableObjects;
using ScriptableObjects.Enemies;
using System;
using System.Collections;
using UnityEngine;

namespace Core.Enemies.RollingCactus
{
    public class RollingEnemyController : MonoBehaviour
    {
        [SerializeField] private RollingEnemyAsset asset;
        [SerializeField] private LayerMask obstaclesPlayerLayerMask;
        private void Start()
        {
            asset.ChangeState(RollingEnemyState.Sleeping);
        }

        private void Update()
        {
            if(asset.State == RollingEnemyState.Sleeping)
                AttemptToDetectPlayer();
        }
        

        private void AttemptToDetectPlayer()
        {
            RaycastHit2D[] forwardRayHits = Physics2D.RaycastAll(transform.position, Vector2.right, asset.PlayerDetectionDistance, obstaclesPlayerLayerMask);
            foreach (var hit in forwardRayHits)
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayerHealth _))
                {
                    StartRollingClockwise();
                    return;
                }
            }
            
            RaycastHit2D[] backwardRayHits = Physics2D.RaycastAll(transform.position, Vector2.left, asset.PlayerDetectionDistance, obstaclesPlayerLayerMask);
            foreach (var hit in backwardRayHits)
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayerHealth _))
                {
                    StartRollingCounterClockwise();
                    return;
                }
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawRay(transform.position, Vector2.right * asset.PlayerDetectionDistance);
            Gizmos.DrawRay(transform.position, Vector2.left * asset.PlayerDetectionDistance);
        }

        private void StartRollingCounterClockwise()
        {
            asset.ChangeState(RollingEnemyState.Rolling);
            asset.ChangeRotation(Rotation.Counterclockwise);
        }
        
        private void StartRollingClockwise()
        {
            asset.ChangeState(RollingEnemyState.Rolling);
            asset.ChangeRotation(Rotation.Clockwise);
        }
    }
}
