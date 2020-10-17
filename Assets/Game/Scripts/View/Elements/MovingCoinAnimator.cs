using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.View.Elements
{
    public class MovingCoinAnimator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _coins;
        [SerializeField] private Transform _target;

        private float _duration;
        private readonly Vector3 _targetPositionOffset = new Vector3(0, 0, 0);

        private event Action _firstCoinCollected;
        private event Action _lastCoinCollected;

        private void Start()
        {
            foreach (var coin in _coins)
            {
                coin.gameObject.SetActive(false);
            }
        }

        public void Move(Action onFirstCoinCollected = null, Action onLastCoinCollected = null)
        {
            ResetCoins();

            _firstCoinCollected = OnFirstCoinCollected;
            _lastCoinCollected = OnLastCoinCollected;

            var delay = 0f;

            for (int i = 0; i < _coins.Count; i++)
            {
                var coin = _coins[i];
                delay += 0.05f;

                var randomVector = new Vector3(UnityEngine.Random.Range(-150f, 150f), UnityEngine.Random.Range(-150f, 150f), 0);
                var i2 = i;

                coin.gameObject.SetActive(true);
                coin.transform.DOLocalMove(randomVector, 0.5f).OnComplete(() =>
                {
                    var i1 = i2;
                    coin.transform.DOMove(_target.position + _targetPositionOffset, 1f)
                        .SetEase(Ease.InSine)
                        .SetDelay(0.2f)
                        .OnComplete(() =>
                        {
                            coin.gameObject.SetActive(false);
                            coin.transform.DOKill();

                            if (i1 == 0)
                            {
                                _firstCoinCollected?.Invoke();
                            }

                            if (i1 == _coins.Count - 1)
                            {
                                _lastCoinCollected?.Invoke();
                            }
                        });
                }).SetDelay(delay);
            }

            void OnFirstCoinCollected()
            {
                onFirstCoinCollected?.Invoke();
            }

            void OnLastCoinCollected()
            {
                onLastCoinCollected.Invoke();
            }
        }

        private void ResetCoins()
        {
            foreach (var coin in _coins)
            {
                coin.transform.localPosition = Vector3.zero;
                coin.transform.localScale = Vector3.one;
            }
        }
    }
}