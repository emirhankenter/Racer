﻿using Game.Scripts.Behaviours;
using Game.Scripts.View;
using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour _player;
        [SerializeField] private ParticleSystem _confetti;

        [SerializeField] private List<LevelBehaviour> _levels;

        public LevelBehaviour CurrentLevel { get; private set; }

        private void Awake()
        {
            PrepareLevel();
        }
        private void PrepareLevel()
        {
            CurrentLevel = Instantiate(_levels[(PlayerData.Level - 1) % _levels.Count]);
            CurrentLevel.Initialize();

            CurrentLevel.Completed += OnLevelCompleted;

            _player.Initialize();

            ViewController.Instance.InGameView.Open(new InGameViewParameters());
        }

        private void DisposeLevel()
        {
            CurrentLevel.Dispose();

            Destroy(CurrentLevel.gameObject);

            PrepareLevel();
        }

        private void NextLevel()
        {
            DisposeLevel();
        }

        private void OnLevelCompleted(bool isSuccess)
        {
            CurrentLevel.Completed -= OnLevelCompleted;

            ViewController.Instance.InGameView.Close();

            if (isSuccess)
            {
                var earnAmount = PlayerData.Level * 50;
                PlayConfetti(() => ViewController.Instance.GameOverView.Open(new GameOverViewParameters(earnAmount, OnRewardClaimed)));
                PlayerData.Level++;
                PlayerData.Coin += earnAmount;
            }
            else
            {
                DisposeLevel();
            }
        }

        private void OnRewardClaimed()
        {
            CoroutineController.DoAfterGivenTime(2f, () =>
            {
                ViewController.Instance.GameOverView.Close();
                NextLevel();
            });
        }

        private void PlayConfetti(Action onComplete = null)
        {
            _confetti.transform.position = _player.transform.position + Vector3.up * 1f;
            _confetti.gameObject.SetActive(true);
            _confetti.Play();
            CoroutineController.DoAfterGivenTime(_confetti.main.duration, () => 
            {
                _confetti.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
    }
}