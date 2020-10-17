using Assets.Game.Scripts.Behaviours;
using Game.Scripts.Behaviours;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Game.Scripts.View.Elements;
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
        }

        private void OnCoinCollected()
        {
            _coinElement.UpdateCoin(PlayerData.Coin);
        }

        private void OnFinishLinePassed()
        {
        }

        private void UnregisterEvents()
        {
        }
        private void InitializeElements()
        {
            _coinElement.Initialize(PlayerData.Coin);
            _levelElement.Initialize();
        }

        private void DisposeElements()
        {
        }
    }

    public class InGameViewParameters : ViewParameters { }
}