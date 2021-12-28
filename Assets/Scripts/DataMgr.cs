using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr
{
    private DataMgr() {}
    private static DataMgr m_pInstance = null;
    public static DataMgr Instance
    {
        get 
        {
            if(m_pInstance == null) 
            {
                m_pInstance = new DataMgr();
            }

            return m_pInstance;
        }
    }

    private GridMgr m_pGridMgr = null;
    public GridMgr GridMgr
    {
        get 
        {
            if(m_pGridMgr == null) 
            {
                var go = GameObject.Find("GridMgr");
                if(go == null) 
                {
                    go = new GameObject("GridMgr");
                }
                m_pGridMgr = go.GetComponent<GridMgr>();
                if(m_pGridMgr == null)
                {
                    m_pGridMgr = go.AddComponent<GridMgr>();
                }
            }

            return m_pGridMgr;
        }
    }
}
