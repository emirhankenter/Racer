using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.Elements
{
    public class CoinElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinAmountText;

        public void Initialize(int coinAmount)
        {
            _coinAmountText.text = coinAmount.ToString();
        }

        public void UpdateCoin(int to)
        {
            _coinAmountText.text = to.ToString();
        }
    }
}