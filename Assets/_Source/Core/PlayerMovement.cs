using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


namespace Core
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float jumpStrength;
        [SerializeField] private float walkSpeed;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask wallsLayer;
        [SerializeField] private BoxCollider2D groundCheckCollider;
        [SerializeField] private BoxCollider2D wallCheckCollider;

        private float _horizontal;
        private bool _isFacingRight = true;

        private bool _isGrounded;
        public bool isTouchingWall;
        
        private Rigidbody2D _body;
        private PlayerInputActions _playerInputActions;

        private const float DragCoefficient = 0.1f;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
        }
        
        private void FixedUpdate()
        {
            CheckGround();
            CheckWalls();
            var velocity = _body.velocity;
            
            if (_isGrounded)
            {
                velocity.x *= DragCoefficient;
                _body.velocity = velocity;
            }

            if (_horizontal != 0 && (!isTouchingWall || _isGrounded))
                _body.velocity = new Vector2(_horizontal * walkSpeed, _body.velocity.y);
            
            if (!_isFacingRight && _horizontal > 0 || _isFacingRight && _horizontal < 0)
                Flip();
        }
        

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed && _isGrounded)
                _body.velocity = new Vector2(_body.velocity.x, jumpStrength);

            var velocity = _body.velocity;
            if (context.canceled && _body.velocity.y > 0)
                _body.velocity = new Vector2(velocity.x, velocity.y * 0.5f);
        }

        public void Move(InputAction.CallbackContext context)
        {
            _horizontal = context.ReadValue<Vector2>().x;
        }


        void CheckGround()
        {
            var bounds = groundCheckCollider.bounds;
            _isGrounded = Physics2D.OverlapAreaAll(bounds.min, bounds.max, groundLayer).Length > 0;
        }

        void CheckWalls()
        {
            var bounds = wallCheckCollider.bounds;
            isTouchingWall = Physics2D.OverlapAreaAll(bounds.min, bounds.max, wallsLayer).Length > 0;
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
