using UnityEngine;
using Tools;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Padder : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var tag = other.gameObject.tag;
            var newTag = tag.Replace("Pad", "Orb");
            if (tag.Contains("Pad") && Constants.JumpModifiersDictionary.ContainsKey(newTag))
            {
                var modifier = Constants.JumpModifiersDictionary[newTag];
                var velocity = _rigidbody.velocity;
                velocity.y = 0;
                _rigidbody.velocity = velocity;
                if (tag.Contains("Blue")) _rigidbody.gravityScale = -_rigidbody.gravityScale;
                var force = new Vector2(0, modifier * Constants.JumpForce * Mathf.Sign(_rigidbody.gravityScale));
                _rigidbody.AddForce(force, ForceMode2D.Impulse);
            }
        }

    }
}
