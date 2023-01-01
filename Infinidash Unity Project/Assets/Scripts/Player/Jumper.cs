using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jumper : MonoBehaviour
    {

        private const string TagGround = "Ground";

        private readonly Dictionary<string, float> _modifiers = new()
        {
            { "Pink Orb", 1 },
            { "Yellow Orb", 1.2f },
            { "Green Orb", -1.2f },
            { "Red Orb", 1.5f }
        };

        private bool _grounded;
        private GameObject _orb;
        private string _orbTag = "None";
        private Rigidbody2D _rigidbody;
        [SerializeField] private float _jumpVelocity = 8.5f;

        public Action Jumped;
        
        private void Jump()
        {
            var modifier = _orbTag == "None" ? 1 : _modifiers[_orbTag];
            var velocity = _rigidbody.velocity;
            velocity.y = _jumpVelocity * modifier * Mathf.Sign(_rigidbody.gravityScale);
            _rigidbody.velocity = velocity;
            Jumped?.Invoke();
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            TouchCatcher.PointerJustDown += _ =>
            {
                if (_grounded || _orbTag != "None") Jump();
            };
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _grounded = true;
                if (TouchCatcher.IsPointerDown) Jump();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground")) _grounded = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Entered some trigger! {other.tag}");
            var otherTag = other.tag;
            if (otherTag.Contains("Orb") && otherTag != "Blue Orb")
            {
                _orbTag = otherTag;
                _orb = other.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var otherTag = other.tag;
            if (otherTag.Contains("Orb") && otherTag != "Blue Orb" && otherTag == _orbTag && _orb == other.gameObject)
            {
                _orbTag = "None";
                _orb = null;
            }
        }
        
    }
}
