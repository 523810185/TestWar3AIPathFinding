﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float radius = 2f;
    // Start is called before the first frame update
    void Start()
    {
        DataMgr.Instance.GridMgr.agents.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
