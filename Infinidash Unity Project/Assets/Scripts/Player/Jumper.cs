using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Tools;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jumper : MonoBehaviour
    {

        private const string TagGround = "Ground";

        private bool _grounded;
        private GameObject _orb;
        private string _orbTag = "None";
        private Rigidbody2D _rigidbody;
        private bool _buffering;
        private GameObject _lastCollidedGroundPiece = null;

        public Action Jumped;
        
        private void Jump()
        {
            var modifier = _orbTag == "None" ? 1 * 1.5f : Constants.JumpModifiersDictionary[_orbTag];
            if (_orbTag is "Blue Orb" or "Green Orb") _rigidbody.gravityScale = -_rigidbody.gravityScale;
            var velocity = _rigidbody.velocity;
            velocity.y = 0;
            _rigidbody.velocity = velocity;
            var force = new Vector2(0, Constants.JumpForce * modifier * Mathf.Sign(_rigidbody.gravityScale));
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
            Jumped?.Invoke();
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            TouchCatcher.PointerJustDown += _ =>
            {
                if (_grounded || _orbTag != "None") Jump();
                else _buffering = true;
            };
            TouchCatcher.PointerUp += _ => _buffering = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _grounded = true;
                _lastCollidedGroundPiece = other.gameObject;
                if (TouchCatcher.IsPointerDown) Jump();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground") && _lastCollidedGroundPiece == other.gameObject) 
                _grounded = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherTag = other.tag;
            if (otherTag.Contains("Orb"))
            {
                _orbTag = otherTag;
                _orb = other.gameObject;
                if (_buffering) Jump();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var otherTag = other.tag;
            if (otherTag == _orbTag && _orb == other.gameObject)
            {
                _orbTag = "None";
                _orb = null;
            }
        }
        
    }
}
