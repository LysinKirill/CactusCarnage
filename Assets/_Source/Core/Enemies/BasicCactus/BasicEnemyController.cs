using ScriptableObjects.Enemies;
using UnityEngine;

namespace Core.Enemies.BasicCactus
{
    public class BasicEnemyController : MonoBehaviour
    {
        [SerializeField] private PlayerDetectionAsset playerDetection;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private GameObject player;
        private Transform PlayerTransform => player.transform;

        private void Update()
        {
            if (!playerDetection.PlayerDetected && CheckPlayer())
            {
                playerDetection.PlayerDetected = true;
            }
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
            var startPosition = PlayerTransform.position;
            
            var direction = (Vector2)(transform.position - startPosition);
            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, direction.magnitude, obstacleLayerMask);
            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            var startPosition = PlayerTransform.position;
            
            var direction = (Vector2)(transform.position - startPosition);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(startPosition, direction);
        }
    }
}
