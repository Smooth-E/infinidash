using System;
using UI;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jumper : MonoBehaviour
    {

        private const string TagGround = "Ground";
        
        private bool _grounded;
        private Rigidbody2D _rigidbody;
        [SerializeField] private float _jumpVelocity = 8.5f;

        public Action Jumped;
        
        private void Jump()
        {
            var velocity = _rigidbody.velocity;
            velocity.y = _jumpVelocity;
            _rigidbody.velocity = velocity;
            Jumped?.Invoke();
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            TouchCatcher.PointerDown += _ =>
            {
                if (_grounded) Jump();
            };
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(TagGround))
            {
                _grounded = true;
                if (TouchCatcher.IsPointerDown) Jump();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(TagGround)) _grounded = false;
        }
        
    }
}
