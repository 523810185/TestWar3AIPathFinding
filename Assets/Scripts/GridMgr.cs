using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridMgr : MonoBehaviour
{
    public int minX, minZ, maxX, maxZ;
    public float gridSize = 0.5f;

    // [NonSerialized]
    public List<Agent> agents = new List<Agent>();

    private List<bool> m_arrCanWalk;
    private int m_iLenX, m_iLenZ;

    // Start is called before the first frame update
    void Start()
    {
        m_iLenX = GetXByPosX(maxX);
        m_iLenZ = GetZByPosZ(maxZ);
        m_arrCanWalk = new List<bool>(m_iLenX * m_iLenZ);
        for(int i=0;i<m_iLenX * m_iLenZ;i++) 
        {
            m_arrCanWalk.Add(true);
        }

        // agents = new List<Agent>();
    }

    // Update is called once per frame
    void Update()
    {
        Clear();
        foreach (var item in agents)
        {
            Vector3 pos = item.transform.position;
            int r = Mathf.FloorToInt(item.radius / gridSize + 0.5f);
            int x = GetXByPosX(pos.x);
            int z = GetZByPosZ(pos.z);
            for(int i=x-r;i<=x+r;i++) 
            {
                for(int j=z-r;j<=z+r;j++) 
                {
                    var dtX = i - x;
                    var dtZ = j - z;
                    if(dtX * dtX + dtZ * dtZ <= r * r) 
                    {
                        m_arrCanWalk[Get2DBy1D(i, j)] = false;
                    }
                }
            }
        }
    }

    public bool GetCanWalk(float x, float z) 
    {
        int id = GetIDByPosXZ(x, z);
        if(id == -1) 
        {
            return false;
        }

        return m_arrCanWalk[id];
    }

    private void Clear()
    {
        for(int i=0;i<m_arrCanWalk.Count;i++) 
        {
            m_arrCanWalk[i] = true;
        }
    }

    private int GetIDByPosXZ(float x, float z) 
    {
        if(x < minX || x > maxX || z < minZ || z > maxZ) 
        {
            return -1;
        }

        int _x = GetXByPosX(x);
        int _z = GetZByPosZ(z);
        int id = Get2DBy1D(_x, _z);
        if(id < 0) id = 0;
        if(id >= m_arrCanWalk.Count) id = m_arrCanWalk.Count - 1;
        return id;
    }

    private int Get2DBy1D(int x, int z) 
    {
        return x * m_iLenZ + z;
    }

    private int GetXByPosX(float x) 
    {
        return Mathf.FloorToInt((x - minX) * 1f / gridSize);
    }

    private int GetZByPosZ(float z) 
    {
        return Mathf.FloorToInt((z - minZ) * 1f / gridSize);
    }
}
