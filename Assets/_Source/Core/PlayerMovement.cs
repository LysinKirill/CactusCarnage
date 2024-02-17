using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Core
{
    public class PlayerMovement : MonoBehaviour
    {
        [FormerlySerializedAs("_jumpStrength")]
        [SerializeField]
        private float jumpStrength;

        [FormerlySerializedAs("_walkSpeed")]
        [SerializeField]
        private float walkSpeed;
        
        
        private Rigidbody2D _body;
        private PlayerInputActions _playerInputActions;
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
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
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if(context.performed)
                _body.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
    }
}
