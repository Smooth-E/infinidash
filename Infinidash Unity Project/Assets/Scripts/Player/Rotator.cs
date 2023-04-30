using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rotator : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;
        private int _direction = -1;
        [SerializeField] private Jumper _jumper;
        [SerializeField] private float _angularVelocity = 120;

        private void OnJump()
        {
            _rigidbody.angularVelocity = _direction * _angularVelocity;
            _direction = -_direction;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _jumper.OnJump += OnJump;
        }

        private void OnDestroy() => _jumper.OnJump -= OnJump;
        
    }
}
