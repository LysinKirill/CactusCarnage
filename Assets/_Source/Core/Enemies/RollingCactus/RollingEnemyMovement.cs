using ScriptableObjects.Enemies;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Enemies.RollingCactus
{
    public class RollingEnemyMovement : MonoBehaviour
    {
        [SerializeField] private RollingEnemyAsset asset;

        private RollingEnemyController _controller;
        private Rigidbody2D _body;
        private Rotation _currentRotation;
        
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _currentRotation = Rotation.Clockwise;
            _controller = GetComponent<RollingEnemyController>();
        }
        

        private void Start()
        {
            _body.velocity = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if(transform.IsDestroyed())
                return;
            
            if(_currentRotation != _controller.Rotation)
                SetOrientation(_controller.Rotation);
            
            if (_controller.State == RollingEnemyState.Sleeping || _controller.State == RollingEnemyState.Static)
            {
                var vector2 = _body.velocity;
                vector2.x *= 0.95f;
                _body.velocity = vector2;
                return;
            }
            
            var orientationMultiplier = _currentRotation == Rotation.Clockwise ? 1 : -1;
            _body.velocity = new Vector2(asset.Speed * orientationMultiplier, _body.velocity.y);
        }
        
        private void SetOrientation(Rotation rotation)
        {
            if(gameObject.IsDestroyed())
                return;
            _currentRotation = rotation;
            var localScale = transform.localScale;
            localScale.x = rotation == Rotation.Clockwise ? 1 : -1;
            transform.localScale = localScale;
        }
    }
}
