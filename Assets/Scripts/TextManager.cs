using System;
using TMPro;
using UnityEngine;

namespace OMG.Assets.Scripts
{
    public class TextManager : MonoBehaviour
    {
        /// <summary>
        /// How many seconds should text be display on screen
        /// </summary>
        public int TextVisibleForXSeconds = 2;
        /// <summary>
        /// If text should automatically hide
        /// </summary>
        public bool AutomaticHide = false;
        private TextMeshProUGUI LabelUserText;
        private bool ShouldTextBeVisible => _textTimeSet.AddSeconds(TextVisibleForXSeconds) > DateTime.Now;
        private DateTime _textTimeSet = DateTime.Now;
        private bool _visible = false;
        private GameObject[] _displayTextObjects;


        public bool Visible
        {
            get => _visible;
            set 
            { 
                _visible = value;
                SetVisibility(value);
            }

        }

        public void Start()
        {
            //all objects tagged as UIDisplayText
            _displayTextObjects = GameObject.FindGameObjectsWithTag("UIDisplayText");
            Debug.Log($"display objects found:{_displayTextObjects.Length}");
            LabelUserText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            Debug.Log($"tmp found = {LabelUserText != null}");
            HideText();
        }

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                SetVisibility(false);
            }
            LabelUserText.text = text;
            _textTimeSet = DateTime.Now;
            SetVisibility(true);
        }
        public void HideText()
        {
            Visible = false;
        }

        private void SetVisibility(bool visible)
        {
            foreach (var displayObject in _displayTextObjects)
            {
                displayObject.SetActive(visible);
            }
        }

        void Update()
        {
            if (AutomaticHide && _visible != ShouldTextBeVisible)
            {
                Visible = !_visible;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Visible = false;
            }
            
        }
    }
}
