using Game.Scripts.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<LevelBehaviour> _levels;

        public LevelBehaviour CurrentLevel { get; private set; }

        private void Awake()
        {
            PrepareLevel();
        }
        private void PrepareLevel()
        {
            CurrentLevel = Instantiate(_levels[(PlayerData.Level - 1) & _levels.Count]);
            CurrentLevel.Initialize();
        }

        private void DisposeLevel()
        {

        }
    }
}