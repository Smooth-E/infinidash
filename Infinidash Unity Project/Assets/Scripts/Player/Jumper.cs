using System;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Game;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jumper : MonoBehaviour
    {

        private const string TagGround = "Ground";
        private const string TagOrb = "Orb";

        private bool _grounded;
        private Orb _orb;
        private Rigidbody2D _rigidbody;
        private bool _buffering;
        private GameObject _lastCollidedGroundPiece = null;

        public event Action OnJump;
        
        private void Jump()
        {
            var modifier = Constants.JumpModifiersDictionary[_orb == null ? OrbType.Pink : _orb.Type];

            if (_orb != null) 
            {
                if (_orb.Type is OrbType.Blue or OrbType.Green) 
                    _rigidbody.gravityScale = -_rigidbody.gravityScale;
                
                _orb.Deactivate(); 
            }
            
            var velocity = _rigidbody.velocity;
            velocity.y = 0;
            _rigidbody.velocity = velocity;

            var force = Constants.CalculateJumpForce(modifier, _rigidbody.gravityScale);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);

            OnJump?.Invoke();
        }
        
        private void OnPointerJustDown(PointerEventData eventData)
        {
            if (_grounded || _orb != null) 
                Jump();
            else 
                _buffering = true;
        }

        private void StopBuffering(PointerEventData eventData) => _buffering = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            TouchCatcher.PointerJustDown += OnPointerJustDown;
            TouchCatcher.PointerUp += StopBuffering;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(TagGround))
            {
                _grounded = true;
                _lastCollidedGroundPiece = other.gameObject;
                if (TouchCatcher.IsPointerDown) Jump();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(TagGround) && _lastCollidedGroundPiece == other.gameObject) 
                _grounded = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagOrb))
            {
                _orb = other.GetComponent<Orb>();
                if (_buffering) Jump();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(TagOrb) && other.GetComponent<Orb>() == _orb)
                _orb = null;
        }

        private void OnDestroy()
        {
            TouchCatcher.PointerJustDown -= OnPointerJustDown;
            TouchCatcher.PointerUp -= StopBuffering;
        }
        
    }
}
