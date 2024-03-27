using UnityEngine;
using UnityEngine.InputSystem;


namespace Core
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float jumpStrength;
        
        [SerializeField]
        private float walkSpeed;

        public LayerMask groundLayer;

        public BoxCollider2D groundCheck;

        private float horizontal;
        private bool isFacingRight = true;

        public bool isGrounded;
        
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
            var velocity = _body.velocity;
            
            if (isGrounded)
            {
                velocity.x *= DragCoefficient;
                _body.velocity = velocity;
            }

            if (horizontal != 0)
                _body.velocity = new Vector2(horizontal * walkSpeed, _body.velocity.y);
            
            if (!isFacingRight && horizontal > 0 || isFacingRight && horizontal < 0)
                Flip();
        }
        

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
                _body.velocity = new Vector2(_body.velocity.x, jumpStrength);

            var velocity = _body.velocity;
            if (context.canceled && _body.velocity.y > 0)
                _body.velocity = new Vector2(velocity.x, velocity.y * 0.5f);
        }

        public void Move(InputAction.CallbackContext context)
        {
            horizontal = context.ReadValue<Vector2>().x;
        }


        void CheckGround()
        {
            var bounds = groundCheck.bounds;
            isGrounded = Physics2D.OverlapAreaAll(bounds.min, bounds.max, groundLayer).Length > 0;
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
