using Game.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mek.Controllers
{
    public static class CoroutineController
    {
        private static Dictionary<string, IEnumerator> _coroutineDictionary = new Dictionary<string, IEnumerator>();

        public static void StartCoroutine(string key, IEnumerator coroutine)
        {
            StartMyCoroutine(key, coroutine);
        }

        public static bool IsCoroutineRunning(string key)
        {
            return _coroutineDictionary.ContainsKey(key);
        }

        public static void StopCoroutine(string key)
        {
            if (_coroutineDictionary.TryGetValue(key, out IEnumerator value))
            {
                CoroutineHelper.Instance.StopCoroutine(value);
                _coroutineDictionary.Remove(key);
            }
        }

        private static Coroutine StartMyCoroutine(string key, IEnumerator coroutine)
        {
            return CoroutineHelper.Instance.StartCoroutine(GenericCoroutine(key, coroutine));
        }

        private static IEnumerator GenericCoroutine(string key, IEnumerator coroutine)
        {
            _coroutineDictionary.Add(key, coroutine);
            yield return CoroutineHelper.Instance.StartCoroutine(coroutine);
            _coroutineDictionary.Remove(key);
        }

        public static void ToggleRoutine(bool state, string routineKey, IEnumerator routine)
        {
            if (state && !IsCoroutineRunning(routineKey))
            {
                StartCoroutine(routineKey, routine);
            }

            if (!state && IsCoroutineRunning(routineKey))
            {
                StopCoroutine(routineKey);
            }
        }

        #region Helpers

        public static void DoAfterGivenTime(float time, Action onComplete)
        {
            DoAfterTime(time, onComplete);
        }

        public static void DoAfterFixedUpdate(Action onComplete)
        {
            CoroutineHelper.Instance.StartCoroutine(FixedUpdateRoutine());

            IEnumerator FixedUpdateRoutine()
            {
                yield return new WaitForFixedUpdate();
                onComplete?.Invoke();
            }
        }

        private static void DoAfterTime(float time, Action onComplete)
        {
            CoroutineHelper.Instance.StartCoroutine(TimeRoutine(time, () => { onComplete?.Invoke(); }));
        }

        private static IEnumerator TimeRoutine(float timer, Action onComplete)
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            onComplete?.Invoke();
        }

        #endregion
    }
}
