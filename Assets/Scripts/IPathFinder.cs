using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder
{
    bool FindPath(List<Vector2Int> path, Vector3 st, Vector3 ed);
}
