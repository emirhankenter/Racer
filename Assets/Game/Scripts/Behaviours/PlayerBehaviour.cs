using Game.Scripts.Utilities;
using Game.Scripts.Models;
using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace Game.Scripts.Behaviours
{
    public enum Direction
    {
        Left,
        Right
    }
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private LineBehaviour _linePrefab;

        [SerializeField] private float _speed = 40f;
        [SerializeField] private float _angularSpeed = 100f;
        [SerializeField] private float _localAngularSpeed = 1f;

        protected float Speed => _speed;
        protected float AngularSpeed => _angularSpeed;
        protected float LocalAngularSpeed => _localAngularSpeed;

        private LineBehaviour _line;

        private bool _holding;
        private bool _canMove;
        private bool _canRotate;

        private Vector3 _localRotationLimit = new Vector3(0, 45, 0);
        private Vector3 _localRotationDifference = new Vector3();

        private string MoveRoutineKey => $"MoveRoutine{GetInstanceID()}";
        private string RotateRoutineKey => $"RotateRoutine{GetInstanceID()}";

        protected PlayerInputs Input;
        private void Awake()
        {
            Input = new PlayerInputs();

            Input.Enable();

            RegisterEvents();

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

            StartCoroutine(Holding());
        }
        private void OnPressCanceled(InputAction.CallbackContext obj)
        {
            _holding = false;
            _line.Dispose();

            ToggleMovement(true);
            ToggleRotating(false);
            Drift();
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
            var initialRotation = transform.localEulerAngles;
            var initialRotationQuaternion = transform.localRotation;
            var limit = transform.localEulerAngles + (direction == Direction.Right ? _localRotationLimit : -_localRotationLimit);
            var targetRotation = new Quaternion();

            while (_canRotate)
            {
                if (_holding)
                {
                    transform.parent.rotation = Quaternion.Euler(0, transform.parent.eulerAngles.y + (direction == Direction.Right ? 1 : -1) * AngularSpeed * Time.fixedDeltaTime, 0);

                    targetRotation = Quaternion.Euler(initialRotation + limit);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, LocalAngularSpeed * Time.fixedDeltaTime);

                    var difference = Quaternion.Angle(transform.localRotation, initialRotationQuaternion);
                    _localRotationDifference = new Vector3(0, (direction == Direction.Right ? difference : -difference), 0);

                    _line.UpdateLine();
                }
                yield return new WaitForFixedUpdate();
            }
        }

        private void Drift()
        {
            if (_localRotationDifference != Vector3.zero)
            {
                _rigidBody.transform.DORotate(-_localRotationDifference, 0.5f, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack).OnComplete(() => { });
            }
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Corner")) return;

            var corner = other.GetComponent<CornerBehaviour>();

            var direction = transform.GetDirectionTo(corner.AnchorPoint);

            transform.SetParent(corner.AnchorPoint, true);

            _line = _linePrefab.Spawn();
            _line.Initialize(transform, corner.AnchorPoint);

            ToggleRotating(true, direction);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Corner")) return;
            transform.SetParent(null);

            ToggleRotating(false);
            ToggleMovement(true);
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