using UnityEngine;
using Game;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

        private void Update() => _rigidbody.velocity = new Vector2(Constants.MoveVelocity, _rigidbody.velocity.y);

        // TODO: Increase velocity when colliding with 'arrows'

    }
}
