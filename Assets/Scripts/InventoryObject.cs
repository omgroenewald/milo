using UnityEngine;
using UnityEngine.EventSystems;

namespace OMG.Assets.Scripts
{
    public class InventoryObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
    {

        SpriteRenderer sprite;
        Color target = Color.red;
        private InteractionObject _iaObject;
        private UnityEngine.UI.Image _image;
        private bool _loaded = false;
        private LevelManager _levelManager;
        private Vector3 _startPosition;

        public void Start()
        {
            
            _image = GetComponentInParent<UnityEngine.UI.Image>();
            _startPosition = _image.rectTransform.position;
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            if (_iaObject != null)
                _image.sprite = _iaObject.Sprite;
            _loaded = true;
        }

        public InteractionObject InteractionObject
        {
            set
            {
                _iaObject = value;
                if (_loaded)
                {
                    _image.sprite = _iaObject.Sprite;
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
            _image.rectTransform.position *= eventData.delta;
            
        }
    }
}
