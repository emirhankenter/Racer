using Assets.Game.Scripts.Behaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Utilities
{
    [Serializable]
    public struct RoadDictionary
    {
        public RoadBehaviour RightTurn;
        public RoadBehaviour LeftTurn;
        public RoadBehaviour URightTurn;
        public RoadBehaviour ULeftTurn;

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
        private Queue<RoadBehaviour> _lastRoads = new Queue<RoadBehaviour>();

        private Transform _parent;

        public void Generate(Transform parent,int roadCount = 10)
        {
            _parent = parent;
            for (int i = 0; i < roadCount; i++)
            {
                //var last2Roads = new List<RoadBehaviour>();

                //if (i >= 2)
                //{
                //    foreach (var road in _lastRoads)
                //    {
                //        last2Roads.Add(road);
                //    }
                //    _lastRoads.Dequeue();
                //}
                if (i == 0)
                {
                    _lastRoad = Instantiate(RoadDictionary.GetRandomRegular(), parent);
                    continue;
                }

                if (_lastRoad.RoadType == RoadType.RightTurn)
                {
                    _lastRoad = InstantiateRoadAtPosition(RoadDictionary.ULeftTurn);
                    continue;
                }

                if (_lastRoad.RoadType == RoadType.LeftTurn)
                {
                    _lastRoad = InstantiateRoadAtPosition(RoadDictionary.URightTurn);
                    continue;
                }

                if (_lastRoad.RoadType == RoadType.U_RightTurn)
                {
                    _lastRoad = InstantiateRoadAtPosition(RoadDictionary.ULeftTurn);
                    continue;
                }

                if (_lastRoad.RoadType == RoadType.U_LeftTurn)
                {
                    _lastRoad = InstantiateRoadAtPosition(RoadDictionary.URightTurn);
                    continue;
                }
            }
        }

        private RoadBehaviour InstantiateRoadAtPosition(RoadBehaviour newRoad)
        {
            var road = Instantiate(newRoad, _lastRoad.Edge);

            road.transform.localPosition = Vector3.zero;
            road.transform.localRotation = Quaternion.EulerAngles(Vector3.zero);

            road.transform.SetParent(_parent, true);

            return road;
        }
    }
}