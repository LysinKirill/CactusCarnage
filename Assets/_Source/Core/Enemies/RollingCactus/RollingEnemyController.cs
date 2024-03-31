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
            RaycastHit2D forwardRay = Physics2D.Raycast(transform.position, Vector2.right, asset.PlayerDetectionDistance, obstaclesPlayerLayerMask);
            if (forwardRay.collider != null && forwardRay.collider.TryGetComponent(out PlayerHealth _))
            {
                StartRollingClockwise();
                return;
            }
            
            RaycastHit2D backwardRay = Physics2D.Raycast(transform.position, Vector2.left, asset.PlayerDetectionDistance, obstaclesPlayerLayerMask);
            if (backwardRay.collider != null && backwardRay.collider.TryGetComponent(out PlayerHealth _))
            {
                StartRollingCounterClockwise();
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
            
            //StartCoroutine(RollCounterClockwise());
        }

        // private IEnumerator RollCounterClockwise()
        // {
        //     StartRollingCounterClockwise();
        // }

        private void StartRollingClockwise()
        {
            asset.ChangeState(RollingEnemyState.Rolling);
            asset.ChangeRotation(Rotation.Clockwise);
        }
    }
}
