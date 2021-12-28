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

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        inAgent = !mgr.GetCanWalk(pos.x, pos.z);
    }
}
