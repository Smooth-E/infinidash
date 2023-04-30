using UnityEngine;
using Game;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PadJumper : MonoBehaviour
    {

        private const string TagPad = "Pad";

        private Rigidbody2D _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(TagPad)) return;

            var pad = other.GetComponent<Pad>();
            pad.Deactivate();

            var velocity = _rigidbody.velocity;
            velocity.y = 0;
            _rigidbody.velocity = velocity;

            if (pad.Type == PadType.Blue)
            {
                _rigidbody.gravityScale = -_rigidbody.gravityScale;
                return;
            }
            
            var modifier = Constants.JumpModifiersDictionary[(OrbType)pad.Type];

            var force = Constants.CalculateJumpForce(modifier, _rigidbody.gravityScale);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

    }
}
