using Game.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.Elements
{
    public class LevelElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        public void Initialize()
        {
            _levelText.text = $"Level {PlayerData.Level}";
        }
    }
}