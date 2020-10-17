using Game.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public delegate void WinDelegate(bool isSuccess);
    public class LevelBehaviour : MonoBehaviour
    {
        public static event Action Started;
        public WinDelegate Completed;

        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private int _roadCount = 10;

        public void Initialize()
        {
            _mapGenerator.Generate(transform, _roadCount);
            PlayerBehaviour.Hit += OnPlayerHit;

            Started?.Invoke();
        }

        public void Dispose()
        {
            PlayerBehaviour.Hit -= OnPlayerHit;
        }

        private void OnPlayerHit()
        {
            Completed?.Invoke(false);
        }
    }
}