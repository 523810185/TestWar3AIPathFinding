using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAgent : MonoBehaviour
{
    public bool inAgent = false;
    public GridMgr mgr = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IPathFinder finder = new AStarPathFinder();
    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        inAgent = !mgr.GetCanWalkOfPos(pos.x, pos.z);

        if(_1 != _2) 
        {
            _1 = _2;
            var path = new List<Vector2Int>();
            finder.FindPath(path, transform.position, edPos);
            foreach (var item in path)
            {
                Debug.Log(" -- " + item);
            }
        }
    }

    public int _1, _2;
    public Vector3 edPos;
}
