using Game.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Behaviours
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private bool _holding;

        protected PlayerInputs Input;
        private void Awake()
        {
            Input = new PlayerInputs();

            Input.Enable();

            RegisterEvents();
        }

        private void OnDestroy()
        {
            Input.Disable();

            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            Input.Player.Press.performed += OnPressPerformed;
            Input.Player.Press.canceled += OnPressCanceled;
        }

        private void UnregisterEvents()
        {
            Input.Player.Press.performed -= OnPressPerformed;
            Input.Player.Press.canceled -= OnPressCanceled;
        }

        private void OnPressPerformed(InputAction.CallbackContext obj)
        {
            _holding = true;

            StartCoroutine(Holding());
        }
        private void OnPressCanceled(InputAction.CallbackContext obj)
        {
            _holding = false;
        }

        private IEnumerator Holding()
        {
            while (_holding)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}