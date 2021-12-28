using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder : IPathFinder
{
    private class Node
    {
        public int D; // Dis from st;
        public int H; // 启发因子
        public int F
        {
            get 
            {
                return D + H;
            }
        }

        public Node parent;
        public bool used = false;

        public int x, z;
        public Node(int x, int z) 
        {
            Init();
            this.x = x;
            this.z = z;
        }
        public Node(Vector2Int v) : this(v.x, v.y)
        {
        }

        public Node Init()
        {
            D = H = 0;
            x = z = 0;
            parent = null;
            used = false;
            return this;
        }

        public int GetHWithNode(Node node) 
        {
            return Mathf.Abs(node.x - x) + Mathf.Abs(node.z - z);
        }
    }

    private Node[,] m_arrNodeCache;
    private bool m_bHasInitNodeCache = false;
    private int m_iNodePointIndex = 0;
    private void InitNodeCache()
    {
        if(!m_bHasInitNodeCache) 
        {
            m_bHasInitNodeCache = true;
            m_arrNodeCache = new Node[Map.m_iLenX, Map.m_iLenZ];
            for(int i=0;i<Map.m_iLenX;i++) 
            {
                for(int j=0;j<Map.m_iLenZ;j++) 
                {
                    m_arrNodeCache[i, j] = new Node(0, 0);
                }
            }
        }
    }
    private void ClearNodeCache()
    {
        InitNodeCache();
        for(int i=0;i<Map.m_iLenX;i++) 
        {
            for(int j=0;j<Map.m_iLenZ;j++) 
            {
                m_arrNodeCache[i, j].Init();
            }
        }
    }
    private Node GetNewNode(int x, int z)
    {
        if(Map.IsValidIndex(x, z))
        {
            var ans = m_arrNodeCache[x, z];
            ans.x = x; ans.z = z;
            return ans;
        }

        return null;
    } 
    private Node GetNewNode(Vector2Int v) 
    {
        return GetNewNode(v.x, v.y);
    }

    private class NodeCmper : IComparer<Node>
    {
        public int Compare(Node a, Node b) 
        {
            return a.F - b.F;
        }
    }
    private PriorityQueue<Node> m_stPQ;
    private PriorityQueue<Node> NodeQueue
    {
        get 
        {
            if(m_stPQ == null) 
            {
                m_stPQ = new PriorityQueue<Node>.Builder()
                    .SetComparer(new NodeCmper())
                    .SetPopGreatPriorityValue(false)
                    .Build();
            }

            return m_stPQ;
        }
    }

    private GridMgr Map
    {
        get 
        {
            return DataMgr.Instance.GridMgr;
        }
    }

    private int[] DIR_X = {0, 0, 1, -1};
    private int[] DIR_Z = {1, -1, 0, 0};
    public bool FindPath(List<Vector2Int> path, Vector3 st, Vector3 ed)
    {
        if(path == null) 
        {
            return false;
        }

        path.Clear();
        NodeQueue.Clear();
        ClearNodeCache();

        var stNode = GetNewNode(Map.GetIndex(st.x, st.z));
        var edNode = GetNewNode(Map.GetIndex(ed.x, ed.z));
        stNode.D = 0;
        stNode.H = stNode.GetHWithNode(edNode);
        NodeQueue.Push(stNode);
        for(int _=0;_<1000000;_++)
        {
            if(NodeQueue.Count == 0) 
            {
                break;
            }

            Node top = NodeQueue.Top();
            int nowX = top.x;
            int nowZ = top.z;
            if(nowX == edNode.x && nowZ == edNode.z) 
            {
                var cur = top;
                for(int __=0;__<1000000;__++)
                {
                    path.Add(new Vector2Int(cur.x, cur.z));
                    if(cur.x == stNode.x && cur.z == stNode.x) 
                    {
                        break;
                    }

                    cur = cur.parent;
                }

                path.Reverse();
                return true;
            }

            for(int i=0;i<DIR_X.Length;i++) 
            {
                int dx = DIR_X[i];
                int dz = DIR_Z[i];
                int newX = nowX + dx;
                int newZ = nowZ + dz;
                var newNode = GetNewNode(newX, newZ);
                if(!Map.IsValidIndex(newX, newZ) || newNode == null || newNode.used || !Map.GetCanWalkOfIndex(newX, newZ)) 
                {
                    continue;
                }

                newNode.parent = top;
                newNode.D = newNode.D + 1;
                newNode.H = newNode.GetHWithNode(edNode);
                NodeQueue.Push(newNode);
            }
        }

        return false;
    }
}
