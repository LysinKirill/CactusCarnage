using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Core
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float jumpStrength;
        
        [SerializeField]
        private float walkSpeed;

        private Animator _animator;
        private bool _isGrounded = true;
        
        private Rigidbody2D _body;
        private PlayerInputActions _playerInputActions;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
            _animator = GetComponent<Animator>();
        }

        
        private void FixedUpdate()
        {
            Vector2 input = _playerInputActions.Player.Movement.ReadValue<Vector2>();
            //_body.AddForce(new Vector2(input.x * walkSpeed, 0), ForceMode2D.Impulse);
            if(input.x != 0)
            {
                var newXVelocity = Mathf.Lerp(_body.velocity.x, input.x * walkSpeed, 0.15f);
                var velocity = _body.velocity;
                velocity = new Vector2(newXVelocity, velocity.y);
                _body.velocity = velocity;
                transform.localScale = new Vector3(velocity.x >= 0 ? 1 : -1, 1, 1);
            }
            else
            {
                var newXVelocity = Mathf.Lerp(_body.velocity.x, 0, 0.05f);
                _body.velocity = new Vector2(newXVelocity, _body.velocity.y);
            }
            
            _animator.SetBool(IsWalking, input.x != 0);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed && _isGrounded)
            {
                _body.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
                _isGrounded = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
                _isGrounded = true;
                
        }
    }
}
