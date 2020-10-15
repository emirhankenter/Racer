using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBehaviour : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;

    private Transform _from;
    private Transform _to;

    private Vector3[] _positions;
    public void Initialize(Transform from, Transform to)
    {
        _from = from;
        _to = to;
        _positions = new Vector3[2] { from.position, to.position };
        gameObject.SetActive(true);
    }

    public void UpdateLine()
    {
        _positions[0] = _from.position;
        _positions[1] = _to.position;

        _line.SetPositions(_positions);
    }

    public void Dispose()
    {
        _line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
        gameObject.SetActive(false);
    }
}
