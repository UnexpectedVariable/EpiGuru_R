using Assets.Scripts.Util;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    internal class Player : MonoBehaviour, IPauseable, IResumable
    {
        [SerializeField]
        private InputAction _leftStrafeAction = null;
        [SerializeField]
        private InputAction _rightStrafeAction = null;
        [SerializeField]
        private HoldableButton _leftStrafeButton = null;
        [SerializeField]
        private HoldableButton _rightStrafeButton = null;
        [SerializeField]
        private Rigidbody _body = null;

        [SerializeField]
        private Vector3 _movementVector = Vector3.zero;
        [SerializeField]
        private float _speedMultiplier = 0f;

        public float MaximumX = 0f;

        private void Start()
        {
            InitializeInputActions();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            _leftStrafeButton.OnButtonHeld += () =>
            {
                MoveTransform(_movementVector * -1 * Time.deltaTime * _speedMultiplier);
            };
            _rightStrafeButton.OnButtonHeld += () =>
            {
                MoveTransform(_movementVector * Time.deltaTime * _speedMultiplier);
            };
        }

        private void InitializeInputActions()
        {
            _leftStrafeAction.Enable();
            _rightStrafeAction.Enable();
        }

        private void Update()
        {
            if (_leftStrafeAction.IsPressed())
            {
                MoveTransform(_movementVector * -1 * Time.deltaTime * _speedMultiplier);
            }
            if (_rightStrafeAction.IsPressed())
            {
                MoveTransform(_movementVector * Time.deltaTime * _speedMultiplier);
            }
        }

        private void MoveTransform(Vector3 movementVector)
        {
            Vector3 position = transform.position;
            position += movementVector;
            position.x = Mathf.Clamp(position.x, MaximumX * -1, MaximumX);
            transform.position = position;
        }

        public void Pause()
        {
            _leftStrafeAction.Disable();
            _rightStrafeAction.Disable();

            _leftStrafeButton.Active = false;
            _rightStrafeButton.Active = false;
        }
        public void Resume()
        {
            _leftStrafeAction.Enable();
            _rightStrafeAction.Enable();

            _leftStrafeButton.Active = true;
            _rightStrafeButton.Active = true;
        }
    }
}
