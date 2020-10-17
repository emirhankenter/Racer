using DG.Tweening;
using Mek.Controllers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.Elements
{
    public class CoinElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinAmountText;

        private int _currentCoin;

        public void Initialize(int coinAmount)
        {
            _currentCoin = coinAmount;
            _coinAmountText.text = coinAmount.ToString();
        }

        public void UpdateCoin(int to)
        {
            var counter = _currentCoin;

            DOTween.To(
                getter: () => counter,
                setter: i => counter = i,
                endValue: PlayerData.Coin,
                duration: 1f)
                .onUpdate = () =>
                {
                    _coinAmountText.text = counter.ToString();
                };

            CoroutineController.DoAfterGivenTime(1f, () => _currentCoin = PlayerData.Coin);
        }
    }
}