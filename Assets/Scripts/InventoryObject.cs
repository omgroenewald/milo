using UnityEngine;
using UnityEngine.EventSystems;

namespace OMG.Assets.Scripts
{
    public class InventoryObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler, IEndDragHandler, IBeginDragHandler
    {

        SpriteRenderer sprite;
        Color target = Color.red;
        private InteractionObject _iaObject;
        private UnityEngine.UI.Image _image;
        private bool _loaded = false;
        private LevelManager _levelManager;
        private Vector2 _startPosition;
        private CanvasGroup _canvasGroup;

        public void Awake()
        {
            
            _image = GetComponentInParent<UnityEngine.UI.Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _startPosition = _image.rectTransform.anchoredPosition;
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            if (_iaObject != null)
                _image.sprite = _iaObject.Sprite;
            _loaded = true;
        }

        public InteractionObject InteractionObject
        {
            get => _iaObject;
            set
            {
                _iaObject = value;
                if (_loaded)
                {
                    _image.sprite = _iaObject?.Sprite;
                }
            }
        }

        public void Clear()
        {
            if (!_loaded)
                return;
            _image.sprite = null;
            _iaObject = null;
        }
        public int Order;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_iaObject is null) return;
            _levelManager.EnableRotate(_iaObject.gameObject);
            Debug.Log("Rotate me", _iaObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_iaObject is null) return;
            Debug.Log("On me", _iaObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_iaObject is null) return;
            Debug.Log("Gone from me", _iaObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("Dragging me", _iaObject);
            _image.rectTransform.anchoredPosition += eventData.delta;
            
        }

        public void OnDrop(PointerEventData eventData)  
        {
            if (_iaObject == null) return;
            var droppedObject = eventData.pointerDrag.GetComponent<InventoryObject>();
            if (droppedObject != null)
            {
                Debug.Log("dropping on me", _iaObject);
                _iaObject = _iaObject.Craft(droppedObject.InteractionObject);
                _image.sprite = _iaObject.Sprite;
                if (!droppedObject.InteractionObject.Owned)
                {
                    droppedObject.InteractionObject = null;
                }
            }
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("end drag", _iaObject);
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _image.rectTransform.anchoredPosition = _startPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("begin drag", _iaObject);
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = .6f;
        }
    }
}
