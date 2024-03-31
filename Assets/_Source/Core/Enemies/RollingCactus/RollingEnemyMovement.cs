using ScriptableObjects.Enemies;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Enemies.RollingCactus
{
    public class RollingEnemyMovement : MonoBehaviour
    {
        [SerializeField] private RollingEnemyAsset asset;
        private Rigidbody2D _body;
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            asset.OnRotationChange += SetOrientation;
            SceneManager.sceneUnloaded += _ => StopAllCoroutines();
        }

        private void Start()
        {
            _body.velocity = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if(transform.IsDestroyed())
                return;
            if (asset.State == RollingEnemyState.Sleeping || asset.State == RollingEnemyState.Static)
            {
                var vector2 = _body.velocity;
                vector2.x *= 0.95f;
                _body.velocity = vector2;
                return;
            }
            
            var orientationMultiplier = asset.Rotation == Rotation.Clockwise ? 1 : -1;
            _body.velocity = new Vector2(asset.Speed * orientationMultiplier, _body.velocity.y);
        }
        
        private void SetOrientation(Rotation rotation)
        {
            if(transform.IsDestroyed())
                return;
            var localScale = transform.localScale;
            localScale.x = rotation == Rotation.Clockwise ? 1 : -1;
            transform.localScale = localScale;
        }
        
    }
}
