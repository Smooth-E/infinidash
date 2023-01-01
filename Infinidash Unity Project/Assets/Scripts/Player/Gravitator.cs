using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Gravitator : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

        private void SwitchGravity() => _rigidbody.gravityScale = -_rigidbody.gravityScale;

        private void SwitchGravityFromPointerEvent(PointerEventData eventData) => SwitchGravity();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherGameObject = other.gameObject;
            if (otherGameObject.CompareTag("Blue Orb") || otherGameObject.CompareTag("Green Orb"))
                TouchCatcher.PointerJustDown += SwitchGravityFromPointerEvent; 
            else if (otherGameObject.CompareTag("Blue Pad"))
                SwitchGravity();
            else if (otherGameObject.CompareTag("Reverse Gravity Portal"))
                _rigidbody.gravityScale = -Mathf.Abs(_rigidbody.gravityScale);
            else if (otherGameObject.CompareTag("Normal Gravity Portal"))
                _rigidbody.gravityScale = Mathf.Abs(_rigidbody.gravityScale);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var otherGameObject = other.gameObject;
            if (otherGameObject.CompareTag("Blue Orb") || otherGameObject.CompareTag("Green Orb"))
                TouchCatcher.PointerJustDown -= SwitchGravityFromPointerEvent;
        }
        
    }
}
