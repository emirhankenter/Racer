using Game.Scripts.Models;
using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Behaviours
{
    public enum Direction
    {
        Left,
        Right
    }
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] Rigidbody _rigidBody;
        [SerializeField] LineBehaviour _line;

        protected float Speed => 150f;

        private bool _holding;
        private bool _canMove;
        private bool _canRotate;

        private string MoveRoutineKey => $"MoveRoutine{GetInstanceID()}";
        private string RotateRoutineKey => $"RotateRoutine{GetInstanceID()}";

        protected PlayerInputs Input;
        private void Awake()
        {
            Input = new PlayerInputs();

            Input.Enable();

            RegisterEvents();

            //_canMove = true;
            //CoroutineController.StartCoroutine(MoveRoutineKey, MoveRoutine());

            ToggleMovement(true);
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
        }
        private void OnPressCanceled(InputAction.CallbackContext obj)
        {
            _holding = false;
            _line.Dispose();

            Debug.Log("PressCanceled");

            ToggleMovement(true);
        }

        #region Routines

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
                if (_canRotate)
                {
                    ToggleMovement(false);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator RotateRoutine(Direction? direction = null)
        {
            var parent = transform.parent;

            while (_canRotate)
            {
                if (_holding)
                {
                    transform.parent.rotation = Quaternion.Euler(0, transform.parent.eulerAngles.y + (direction == Direction.Right ? 120f : -120f) * Time.fixedDeltaTime, 0);

                    //transform.rotation = Quaternion.Euler(0, transform.parent.localEulerAngles.y + (direction == Direction.Right ? 50f : -50f) * Time.fixedDeltaTime, 0);

                    transform.Rotate(new Vector3(0, (direction == Direction.Right ? 45f : -45f), 0) * Time.fixedDeltaTime, Space.Self);

                    _line.UpdateLine();
                    Debug.Log($"Line: {_line.name}");
                }
                yield return new WaitForFixedUpdate();
            }
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Corner")) return;

            var corner = other.GetComponent<CornerBehaviour>();

            var projectionOnRight = Vector3.Dot(corner.AnchorPoint.transform.position - transform.position, transform.right);

            //var direction = projectionOnRight < 0 ? Direction.Left : Direction.Right;
            var direction = transform.GetDirectionTo(corner.AnchorPoint);

            transform.SetParent(corner.AnchorPoint, true);

            _line.Initialize(transform, corner.AnchorPoint);

            ToggleRotating(true, direction);

            Debug.Log("Entered");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Corner")) return;
            transform.SetParent(null);

            ToggleRotating(false);
            ToggleMovement(true);

            Debug.Log("Exited");
        }

        private void ToggleMovement(bool state)
        {
            _canMove = state;
            CoroutineController.ToggleRoutine(state, MoveRoutineKey, MoveRoutine());
        }
        private void ToggleRotating(bool state, Direction? direction = null)
        {
            _canRotate = state;
            CoroutineController.ToggleRoutine(state, RotateRoutineKey, RotateRoutine(direction));
        }
    }
}