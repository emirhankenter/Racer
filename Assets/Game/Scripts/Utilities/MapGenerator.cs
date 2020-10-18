using Assets.Game.Scripts.Behaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Utilities
{
    public enum WorldDirection
    {
        Forward,
        Backward,
        Right,
        Left
    }
    [Serializable]
    public struct RoadDictionary
    {
        public RoadBehaviour RightTurn;
        public RoadBehaviour LeftTurn;
        public RoadBehaviour URightTurn;
        public RoadBehaviour ULeftTurn;
        public RoadBehaviour Finish;

        private RoadBehaviour[] RegularRoads;
        private RoadBehaviour[] URoads;

        private RoadBehaviour[] RightTurns;
        private RoadBehaviour[] LeftTurns;

        public RoadBehaviour GetRandomRegular()
        {
            if (RegularRoads == null)
            {
                RegularRoads = new RoadBehaviour[] { RightTurn, LeftTurn };
            }
            return RegularRoads.GetRandomElement();
        }

        public RoadBehaviour GetRandomU()
        {
            if (URoads == null)
            {
                URoads = new RoadBehaviour[] { URightTurn, ULeftTurn };
            }
            return URoads.GetRandomElement();
        }

        public RoadBehaviour GetRandomLeft()
        {
            if (LeftTurns == null)
            {
                LeftTurns = new RoadBehaviour[] { LeftTurn, ULeftTurn };
            }
            return LeftTurns.GetRandomElement();
        }

        public RoadBehaviour GetRandomRight()
        {
            if (RightTurns == null)
            {
                RightTurns = new RoadBehaviour[] { RightTurn, URightTurn };
            }
            return RightTurns.GetRandomElement();
        }
    }
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private RoadDictionary RoadDictionary;

        private RoadBehaviour _lastRoad;
        private Transform _parent;
        private Vector3 _forward = Vector3.forward;

        public void Generate(Transform parent,int roadCount = 10)
        {
            _parent = parent;
            for (int i = 0; i < roadCount; i++)
            {
                if (i == 0)
                {
                    _lastRoad = Instantiate(RoadDictionary.GetRandomRegular(), parent);
                    continue;
                }

                if (i == roadCount - 1)
                {
                    _lastRoad = InstantiateRoadAtPosition(RoadDictionary.Finish);
                    _forward = _lastRoad.Edge.forward;
                    continue;
                }

                if (_lastRoad.RoadType == RoadType.RightTurn)
                {
                    if (_forward == Vector3.right)
                    {
                        _lastRoad = InstantiateRoadAtPosition(RoadDictionary.GetRandomLeft());
                        continue;
                    }
                    else if (_forward == Vector3.forward)
                    {
                        _lastRoad = InstantiateRoadAtPosition(RoadDictionary.LeftTurn);
                        continue;
                    }
                }

                if (_lastRoad.RoadType == RoadType.LeftTurn)
                {
                    if (_forward == Vector3.left)
                    {
                        _lastRoad = InstantiateRoadAtPosition(RoadDictionary.GetRandomRight());
                        continue;
                    }
                    else if (_forward == Vector3.forward)
                    {
                        _lastRoad = InstantiateRoadAtPosition(RoadDictionary.RightTurn);
                        continue;
                    }
                }

                if (_lastRoad.RoadType == RoadType.U_RightTurn)
                {
                    if (_forward == Vector3.right)
                    {
                        _lastRoad = InstantiateRoadAtPosition(RoadDictionary.GetRandomLeft());
                        continue;
                    }
                }

                if (_lastRoad.RoadType == RoadType.U_LeftTurn)
                {
                    if (_forward == Vector3.left)
                    {
                        _lastRoad = InstantiateRoadAtPosition(RoadDictionary.GetRandomRight());
                        continue;
                    }
                }
            }
        }

        private RoadBehaviour InstantiateRoadAtPosition(RoadBehaviour newRoad)
        {
            var road = Instantiate(newRoad, _lastRoad.Edge);

            road.transform.localPosition = Vector3.zero;
            road.transform.localRotation = Quaternion.EulerAngles(Vector3.zero);

            road.transform.SetParent(_parent, true);

            _forward = road.Edge.forward;

            return road;
        }
    }
}