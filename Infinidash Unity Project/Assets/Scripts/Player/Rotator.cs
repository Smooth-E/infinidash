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
        [SerializeField] private float _AngularVelocity = 120;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _jumper.OnJump += () =>
            {
                _rigidbody.angularVelocity = _direction * _AngularVelocity;
                _direction = -_direction;
            };
        }
        
    }
}
