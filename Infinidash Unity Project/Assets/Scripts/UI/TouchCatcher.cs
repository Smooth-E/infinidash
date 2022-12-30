using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TouchCatcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {

        public static bool IsPointerDown { private set; get; }

        public static event Action<PointerEventData> PointerDown;
        public static event Action<PointerEventData> PointerUp;
        public static event Action<PointerEventData> PointerClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer down!");
            PointerDown?.Invoke(eventData);
            IsPointerDown = true;
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
