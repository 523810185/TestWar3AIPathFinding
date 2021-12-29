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

    public bool[,] m_arrCanWalk;
    public int m_iLenX, m_iLenZ;

    // Start is called before the first frame update
    void Start()
    {
        m_iLenX = GetIndexXByPosX(maxX);
        m_iLenZ = GetIndexZByPosZ(maxZ);
        m_arrCanWalk = new bool[m_iLenX, m_iLenZ];
        Clear();

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
            int x = GetIndexXByPosX(pos.x);
            int z = GetIndexZByPosZ(pos.z);
            for(int i=x-r;i<=x+r;i++) 
            {
                for(int j=z-r;j<=z+r;j++) 
                {
                    if(!IsValidIndex(i, j)) 
                    {
                        continue;
                    }
                    var dtX = i - x;
                    var dtZ = j - z;
                    if(dtX * dtX + dtZ * dtZ <= r * r) 
                    {
                        m_arrCanWalk[i, j] = false;
                    }
                }
            }
        }
    }

    public bool GetCanWalkOfPos(float x, float z) 
    {
        int indexX = GetIndexXByPosX(x);
        int indexZ = GetIndexZByPosZ(z);
        return GetCanWalkOfIndex(indexX, indexZ);
    }

    public bool GetCanWalkOfIndex(int indexX, int indexZ) 
    {
        if(!IsValidIndex(indexX, indexZ))
        {
            return false;
        }

        return m_arrCanWalk[indexX, indexZ];
    }

    public Vector2Int GetIndex(float x, float z) 
    {
        return new Vector2Int(GetIndexXByPosX(x), GetIndexZByPosZ(z));
    }

    public Vector3 GetPos(int indexX, int indexZ) 
    {
        return new Vector3(minX + gridSize * indexX, 0, minZ + gridSize * indexZ);
    }

    private void Clear()
    {
        for(int i=0;i<m_iLenX;i++) 
        {
            for(int j=0;j<m_iLenZ;j++) 
            {
                m_arrCanWalk[i, j] = true;
            }
        }
    }

    private int GetIndexXByPosX(float x) 
    {
        return Mathf.FloorToInt((x - minX) * 1f / gridSize);
    }

    private int GetIndexZByPosZ(float z) 
    {
        return Mathf.FloorToInt((z - minZ) * 1f / gridSize);
    }

    public bool IsValidIndex(int x, int z) 
    {
        return 0 <= x && x < m_iLenX && 0 <= z && z < m_iLenZ;
    }
}
