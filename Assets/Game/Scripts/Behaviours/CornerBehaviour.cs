using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    [RequireComponent(typeof(BoxCollider))]
    public class CornerBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _anchorPoint;
        public Transform AnchorPoint => _anchorPoint;

    }
}