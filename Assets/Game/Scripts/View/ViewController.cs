using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.View
{
    public class ViewController : MonoBehaviour
    {
        [SerializeField] private View _inGameView;
        public View InGameView => _inGameView;

        [SerializeField] private View _gameOverView;
        public View GameOverView => _gameOverView;

        #region Singleton

        private static ViewController _instance;

        public static ViewController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ViewController>();

                    if (_instance == null)
                    {
                        Debug.LogError($"{typeof(ViewController)} is needed in the scene but it does not exist!");
                    }
                }
                return _instance;
            }
        }

        #endregion
    }
}