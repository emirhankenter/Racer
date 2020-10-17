using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Behaviours
{
    public enum RoadType
    {
        None = 0,

        RightTurn = 1,
        LeftTurn = 2,
        U_RightTurn = 3,
        U_LeftTurn = 4
    }
    public class RoadBehaviour : MonoBehaviour
    {
        [SerializeField] private RoadType _roadType;
        [SerializeField] private Transform _edge;

        public RoadType RoadType => _roadType;
        public Transform Edge => _edge;
    }
}