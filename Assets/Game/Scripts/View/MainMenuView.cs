using Assets.Game.Scripts.Behaviours;
using DG.Tweening;
using Game.Scripts.Behaviours;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class MainMenuView : View
    {
        [SerializeField] private RectTransform _handImage;

        private MainMenuViewParameters _params;

        public override void Open(ViewParameters parameters)
        {
            base.Open();

            _params = parameters as MainMenuViewParameters;
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
            _handImage.DOScale(_handImage.localScale * 1.3f, 0.5f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Yoyo);
        }

        private void DisposeElements()
        {
            _handImage.DOKill();
            _handImage.localScale = Vector3.one;
        }

        public void OnTap()
        {
            _params.OnTap?.Invoke();
        }
    }

    public class MainMenuViewParameters : ViewParameters 
    {
        public Action OnTap;

        public MainMenuViewParameters(Action onTap)
        {
            OnTap = onTap;
        }
    }
}