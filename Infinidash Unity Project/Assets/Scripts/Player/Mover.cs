using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;
        [SerializeField] private float _defaultVelocity = 5;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

        private void Update() => _rigidbody.velocity = new Vector2(_defaultVelocity, _rigidbody.velocity.y);

        // TODO: Increase velocity when colliding with 'arrows'

    }
}
