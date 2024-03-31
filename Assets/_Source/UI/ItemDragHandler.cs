using UnityEngine;

namespace UI
{
    public class ItemDragHandler : MonoBehaviour
    {
        private Canvas _canvas;
        private Camera _camera;
        private InventoryItem _item;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _camera = Camera.main;
            _item = GetComponentInChildren<InventoryItem>();
        }

        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.worldCamera,
                out Vector2 position
            );
            transform.position = _canvas.transform.TransformPoint(position);
        }

        public void EnterFollower() => gameObject.SetActive(true);
        public void ExitFollower() => gameObject.SetActive(false);
    
        public void SetData(Sprite sprite, int quantity) => _item.SetData(sprite, quantity);
    }
}
