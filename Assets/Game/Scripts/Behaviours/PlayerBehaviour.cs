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

    public delegate void WinDelegate(bool isSuccess);
    public class PlayerBehaviour : MonoBehaviour
    {
        public static event WinDelegate Finish;

        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private LineBehaviour _linePrefab;
        [SerializeField] private List<TrailRenderer> _tireTracks;

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
        private bool _canDrift;
        private bool _isHit;

        private Vector3 _localRotationLimit = new Vector3(0, 45, 0);
        private Vector3 _localRotationDifference = new Vector3();

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private string MoveRoutineKey => $"MoveRoutine{GetInstanceID()}";
        private string RotateRoutineKey => $"RotateRoutine{GetInstanceID()}";

        protected PlayerInputs Input;
        private void Awake()
        {
            Input = new PlayerInputs();

            _initialPosition = transform.position;
            _initialRotation = transform.rotation;

            LevelBehaviour.Started += OnLevelStarted;
        }

        private void OnDestroy()
        {
            DeactivateInput();

            LevelBehaviour.Started -= OnLevelStarted;
        }

        private void ActivateInput()
        {
            Input.Enable();

            RegisterEvents();
        }

        private void DeactivateInput()
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

        public void Initialize()
        {
            _rigidBody.isKinematic = false;
            _isHit = false;
            _canDrift = false;
            ToggleMovement(true);
            ActivateInput();
        }

        private void OnLevelStarted()
        {
            ResetTransform();
        }

        private void ResetTransform()
        {
            transform.SetParent(null);
            transform.localScale = Vector3.one;
            transform.position = _initialPosition;
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnPressPerformed(InputAction.CallbackContext obj)
        {
            _holding = true;
        }
        private void OnPressCanceled(InputAction.CallbackContext obj)
        {
            _holding = false;
            if (_line)
            {
                _line.Dispose();
                Destroy(_line.gameObject);
            }

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

        private IEnumerator RotateRoutine(Direction? direction = null)
        {
            var initialRotationQuaternion = transform.localRotation;
            var targetRotation = transform.localRotation * Quaternion.Euler((direction == Direction.Right ? _localRotationLimit : -_localRotationLimit));

            _canDrift = true;

            while (_canRotate)
            {
                if (_holding)
                {
                    transform.parent.rotation = Quaternion.Euler(0, transform.parent.eulerAngles.y + (direction == Direction.Right ? 1 : -1) * AngularSpeed * Time.fixedDeltaTime, 0);

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
            if (_localRotationDifference != Vector3.zero && _canDrift && !_isHit)
            {
                _canDrift = false;
                _rigidBody.transform.DORotate(-_localRotationDifference * 1.2f, 0.25f, RotateMode.WorldAxisAdd).SetEase(Ease.OutSine).OnComplete(() => {});
            }
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Corner"))
            {
                var corner = other.GetComponent<CornerBehaviour>();

                var direction = transform.GetDirectionTo(corner.AnchorPoint);

                transform.SetParent(corner.AnchorPoint, true);

                //TODO: Object pooling is not working as it supposed to be
                //_line = _linePrefab.Spawn();  
                _line = Instantiate(_linePrefab);
                _line.Initialize(transform, corner.AnchorPoint);

                ToggleRotating(true, direction);
            }

            else if (other.gameObject.CompareTag("FinishLine"))
            {
                StopAll();
                Finish?.Invoke(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Corner"))
            {
                transform.SetParent(null, true);

                ToggleRotating(false);
                ToggleMovement(true);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("RoadBorder"))
            {
                StopAll();
                CoroutineController.DoAfterFixedUpdate(() => Finish?.Invoke(false));
                _rigidBody.transform.DOKill();
                ResetTransform();
            }
        }

        private void StopAll()
        {
            ToggleMovement(false);
            ToggleRotating(false);
            _rigidBody.isKinematic = true;
            _isHit = true;
            transform.SetParent(null, true);
            DeactivateInput();
        }

        private void ToggleTireTracks(bool state)
        {
            foreach (var track in _tireTracks)
            {
                track.enabled = state;
            }
        }

        private void ToggleMovement(bool state)
        {
            if (_isHit && state || _canMove == state) return;
            _canMove = state;
            CoroutineController.ToggleRoutine(state, MoveRoutineKey, MoveRoutine());
        }
        private void ToggleRotating(bool state, Direction? direction = null)
        {
            if (_isHit && state || _canRotate == state) return;
            _canRotate = state;
            ToggleTireTracks(state);
            CoroutineController.ToggleRoutine(state, RotateRoutineKey, RotateRoutine(direction));
        }
    }
}