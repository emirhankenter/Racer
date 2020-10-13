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
        [SerializeField] Rigidbody _rigidBody;

        protected float Speed => 150f;

        private bool _holding;
        private bool _canMove;
        private bool _canRotate;

        protected PlayerInputs Input;
        private void Awake()
        {
            Input = new PlayerInputs();

            Input.Enable();

            RegisterEvents();

            _canMove = true;
            StartCoroutine(MoveRoutine());
        }

        private void OnDestroy()
        {
            Input.Disable();

            _canMove = false;
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

        private void FixedUpdate()
        {
        }

        private void OnPressPerformed(InputAction.CallbackContext obj)
        {
            _holding = true;

            Debug.Log("PressPerformed");

            StartCoroutine(Holding());
            StartCoroutine(MoveRoutine());
        }
        private void OnPressCanceled(InputAction.CallbackContext obj)
        {
            _holding = false;

            Debug.Log("PressCanceled");
        }

        private IEnumerator MoveRoutine()
        {
            while (_canMove)
            {
                _rigidBody.MovePosition(Vector3.Lerp(transform.position, transform.position + transform.forward, Time.fixedDeltaTime * Speed));
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator Holding()
        {
            while (_holding)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator Rotating()
        {
            while (_canRotate)
            {
                _rigidBody.MoveRotation(_rigidBody.rotation * Quaternion.Euler(new Vector3(0, 120, 0) * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Corner")) return;

            var corner = other.GetComponent<CornerBehaviour>();

            transform.SetParent(corner.AnchorPoint, true);

            _canRotate = true;
            StartCoroutine(Rotating());

            Debug.Log("Entered");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Corner")) return;
            _canRotate = false;
            transform.SetParent(null);

            Debug.Log("Exited");
        }
    }
}