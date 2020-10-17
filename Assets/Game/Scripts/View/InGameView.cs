using Assets.Game.Scripts.Behaviours;
using Game.Scripts.Behaviours;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.View
{
    public class InGameView : View
    {
        [SerializeField] private CoinElement _coinElement;
        [SerializeField] private LevelElement _levelElement;
        [SerializeField] private ParticleSystem _confetti;

        public override void Open(ViewParameters parameters)
        {
            base.Open();

            InitializeElements();

            RegisterEvents();
        }

        public override void Close()
        {
            base.Close();

            DisposeElements();

            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            GameController.PlayConfetti += PlayConfetti;
        }

        private void UnregisterEvents()
        {
            GameController.PlayConfetti -= PlayConfetti;
        }

        private void OnCoinCollected()
        {
            _coinElement.UpdateCoin(PlayerData.Coin);
        }

        private void OnFinishLinePassed()
        {
        }
        private void InitializeElements()
        {
            _coinElement.Initialize(PlayerData.Coin);
            _levelElement.Initialize();
            _confetti.gameObject.SetActive(false);
        }

        private void DisposeElements()
        {
        }

        public void PlayConfetti(Action onComplete = null)
        {
            _confetti.gameObject.SetActive(true);
            _confetti.Play();
            CoroutineController.DoAfterGivenTime(_confetti.main.duration, () =>
            {
                _confetti.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
    }

    public class InGameViewParameters : ViewParameters { }
}