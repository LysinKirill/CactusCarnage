using Core.Player;
using ScriptableObjects.Enemies;
using UnityEngine;

namespace Core.Enemies.RollingCactus
{
    public class RollingEnemyController : MonoBehaviour
    {
        [SerializeField] private RollingEnemyAsset asset;
        [SerializeField] private LayerMask obstaclesPlayerLayerMask;

        public Rotation Rotation { get; private set; }
        
        public RollingEnemyState State { get; private set; } = RollingEnemyState.Sleeping;

        public void ChangeState(RollingEnemyState newState) => State = newState;
        
        private void Update()
        {
            if(State == RollingEnemyState.Sleeping)
                AttemptToDetectPlayer();
        }
        

        private void AttemptToDetectPlayer()
        {
            RaycastHit2D[] forwardRayHits = Physics2D.RaycastAll(transform.position, Vector2.right, asset.PlayerDetectionDistance, obstaclesPlayerLayerMask);
            foreach (var hit in forwardRayHits)
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayerState _))
                {
                    StartRollingClockwise();
                    return;
                }
            }
            
            RaycastHit2D[] backwardRayHits = Physics2D.RaycastAll(transform.position, Vector2.left, asset.PlayerDetectionDistance, obstaclesPlayerLayerMask);
            foreach (var hit in backwardRayHits)
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayerState _))
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
            Rotation = Rotation.Counterclockwise;
            State = RollingEnemyState.Rolling;
        }
        
        private void StartRollingClockwise()
        {
            State = RollingEnemyState.Rolling;
            Rotation = Rotation.Clockwise;
        }
    }
}
