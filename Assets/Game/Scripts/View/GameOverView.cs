using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class GameOverView : View
    {
        [SerializeField] private CoinElement _coinElement;
        [SerializeField] private TextMeshProUGUI _earnAmountText;

        [SerializeField] private GameObject _claimButtonBlacken;

        private GameOverViewParameters _params;

        public override void Open(ViewParameters parameters)
        {
            base.Open();

            _params = parameters as GameOverViewParameters;
            if (_params == null) return; 

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

        private void UnregisterEvents()
        {
        }
        private void InitializeElements()
        {
            _coinElement.Initialize(PlayerData.Coin);
            _earnAmountText.text = _params.EarnAmount.ToString();
            _claimButtonBlacken.gameObject.SetActive(false);
        }

        private void DisposeElements()
        {
        }

        public void OnClaimButtonClicked()
        {
            PlayerData.Coin += _params.EarnAmount;
            _coinElement.UpdateCoin(PlayerData.Coin);
            _claimButtonBlacken.gameObject.SetActive(true);
            _params.OnComplete?.Invoke();
        }
    }

    public class GameOverViewParameters : ViewParameters 
    {
        public int EarnAmount;
        public Action OnComplete;

        public GameOverViewParameters(int earnAmount, Action onComplete)
        {
            EarnAmount = earnAmount;
            OnComplete = onComplete;
        }
    }
}