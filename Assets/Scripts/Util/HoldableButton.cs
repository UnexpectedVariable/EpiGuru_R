using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Util
{
    internal class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isPressed = false;
        public Action OnButtonHeld = null;
        public bool Active = true;

        private void Update()
        {
            if (!_isPressed) return;
            if (!Active) return;
            OnButtonHeld?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
        }
    }
}
