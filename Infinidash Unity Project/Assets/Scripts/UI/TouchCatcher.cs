using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TouchCatcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {

        public static bool IsPointerDown { private set; get; }

        private bool _wasDownLastFrame;

        public static event Action<PointerEventData> PointerJustDown; 
        public static event Action<PointerEventData> PointerDown;
        public static event Action<PointerEventData> PointerUp;
        public static event Action<PointerEventData> PointerClick;

        private void Update() => _wasDownLastFrame = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_wasDownLastFrame) PointerJustDown?.Invoke(eventData);
            Debug.Log("Pointer down!");
            PointerDown?.Invoke(eventData);
            IsPointerDown = true;
            _wasDownLastFrame = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Pointer up!");
            PointerUp?.Invoke(eventData);
            IsPointerDown = false;
        }

        public void OnPointerClick(PointerEventData eventData) => PointerClick?.Invoke(eventData);
        
    }
}
