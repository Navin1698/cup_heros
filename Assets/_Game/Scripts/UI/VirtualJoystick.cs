using UnityEngine;
using UnityEngine.EventSystems;

namespace OrbRaiders.UI
{
    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform containerRect;
        [SerializeField] private RectTransform handleRect;
        [SerializeField] private float handleRange = 80.0f;

        public Vector2 InputDirection { get; private set; } = Vector2.zero;

        private void Awake()
        {
            if (containerRect == null) containerRect = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, containerRect.position);
            Vector2 radius = containerRect.sizeDelta / 2f;
            InputDirection = (eventData.position - position) / (radius * (handleRange / 100f));

            InputDirection = (InputDirection.magnitude > 1.0f) ? InputDirection.normalized : InputDirection;

            if (handleRect != null)
            {
                handleRect.anchoredPosition = new Vector2(InputDirection.x * (containerRect.sizeDelta.x / 2.5f), InputDirection.y * (containerRect.sizeDelta.y / 2.5f));
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            InputDirection = Vector2.zero;
            if (handleRect != null)
            {
                handleRect.anchoredPosition = Vector2.zero;
            }
        }
    }
}
