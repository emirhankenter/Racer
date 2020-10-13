using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class LevelBehaviour : MonoBehaviour
    {
        public static event Action Started;

        public void Initialize()
        {
            Started?.Invoke();
        }

        public void Dispose()
        {

        }
    }
}