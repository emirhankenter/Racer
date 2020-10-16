using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Utilities
{
    public class ObjectPoolingManager : MonoBehaviour
    {
        private List<GameObject> _objects = new List<GameObject>();

        private Transform _pooledObjects;

        private void Awake()
        {
            var go = new GameObject("Pooled Objects");
            _pooledObjects = go.transform;
        }

        public T Spawn<T>(T target)
        {
            var selectedObj = default(GameObject);

            foreach (var obj in _objects)
            {
                if (!obj.gameObject.activeSelf && obj.GetType() == target.GetType())
                {
                    selectedObj = obj.gameObject;
                    break;
                }
            }

            if (selectedObj == null)
            {
                var obj = target as MonoBehaviour;
                var go = Instantiate(obj.gameObject, _pooledObjects);
                _objects.Add(go);

                selectedObj = go;
            }

            selectedObj.SetActive(true);
            //CoroutineController.DoAfterGivenTime(0.5f, () => selectedObj.SetActive(false));

            return selectedObj.GetComponent<T>();
        }

        private static ObjectPoolingManager _instance;
        public static ObjectPoolingManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ObjectPoolingManager>();

                    if (_instance == null)
                    {
                        var go = new GameObject(nameof(ObjectPoolingManager));
                        var instance = go.AddComponent<ObjectPoolingManager>();
                        _instance = instance;

                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }
    }
}