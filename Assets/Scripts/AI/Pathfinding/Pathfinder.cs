using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{
    public GridBehavior theGridMap;
    public Vector3 m_Destination;
    public bool b_PathFound = false;
    public bool b_CompletedPath = false;
    public double MapRefreshRate;

    List<List<Node>> NodeList;
    List<Node> OpenList = new List<Node>();
    List<Node> ClosedList = new List<Node>();

    Node n_CurrentNode = null;
    int i_CurrentIdx = 0;
    double d_Timer = 0;
    bool b_OnWaypoint = false;

    // Use this for initialization
    void Start()
    {
        NodeList = new List<List<Node>>();

        //Init list
        int SizeX = (int)theGridMap.GetGridSize().x;
        int SizeY = (int)theGridMap.GetGridSize().y;

        for (int i = 0; i < SizeX; i++)
        {
            NodeList.Add(new List<Node>());
        }

        // Fill up NodeList
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                Node toAdd = new Node();
                toAdd.Init((int)theGridMap.GetGridCost(i, j), theGridMap.GetVec3Pos(i, j));
                NodeList[i].Add(toAdd);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (d_Timer > MapRefreshRate)
        {
            RefreshNodeList();
        }
        else
        {
            d_Timer += Time.deltaTime;
        }
    }

    public bool FindPath(Vector3 dest)
    {
        // Reset variables
        OpenList.Clear();
        ClosedList.Clear();
        b_CompletedPath = false;

        int SizeX = (int)theGridMap.GetGridSize().x;
        int SizeY = (int)theGridMap.GetGridSize().y;
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                NodeList[i][j].ParentNode = null;
                NodeList[i][j].AccCost = 0;
            }

        }

        m_Destination = dest;
        n_CurrentNode = GetNode(this.gameObject.transform.position);
        Node TargetNode = GetNode(dest);
        List<Node> NeighbourList = new List<Node>();

        //Debug
        if (n_CurrentNode == null || TargetNode == null)
        {
            Debug.Log("error");
            n_CurrentNode = GetNode(this.gameObject.transform.position);
            TargetNode = GetNode(dest);
        }

        while (!b_PathFound)
        {
            // Add current node to closed list
            ClosedList.Add(n_CurrentNode);

            // Check if reached destination
            if (n_CurrentNode.Equals(TargetNode))
            {
                Debug.Log("Path found to target: " + TargetNode.m_pos.ToString());

                b_PathFound = true;
                b_OnWaypoint = false;

                return true;
            }

            int CurrentGridPosX = (int)theGridMap.GetGridPos(n_CurrentNode.m_pos).x;
            int CurrentGridPosY = (int)theGridMap.GetGridPos(n_CurrentNode.m_pos).y;

            // Get Neighbours of curr node, compute F-values and add to openlist
            int CheckX = CurrentGridPosX;
            int CheckY = CurrentGridPosY;

            // Top
            if (CurrentGridPosY != SizeY - 1)
            {
                CheckY = CurrentGridPosY + 1;

                // Check if reached destination
                if (NodeList[CheckX][CheckY].Equals(TargetNode))
                {
                    Debug.Log("Path found to target: " + TargetNode.m_pos.ToString());

                    b_PathFound = true;
                    b_OnWaypoint = false;

                    return true;
                }

                if (ValidateNode(NodeList[CheckX][CheckY]))
                {
                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }
            }

            // Bottom
            if (CurrentGridPosY != 0)
            {
                CheckY = CurrentGridPosY - 1;   
                
                // Check if reached destination
                if (NodeList[CheckX][CheckY].Equals(TargetNode))
                {
                    //Debug.Log("Path found to target: " + TargetNode.m_pos.ToString());

                    b_PathFound = true;
                    b_OnWaypoint = false;

                    return true;
                }

                if (ValidateNode(NodeList[CheckX][CheckY]))
                {
                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }
            }

            CheckY = CurrentGridPosY;

            // Left
            if (CurrentGridPosX != 0)
            {
                CheckX = CurrentGridPosX - 1;

                // Check if reached destination
                if (NodeList[CheckX][CheckY].Equals(TargetNode))
                {
                    Debug.Log("Path found to target: " + TargetNode.m_pos.ToString());

                    b_PathFound = true;
                    b_OnWaypoint = false;

                    return true;
                }

                if (ValidateNode(NodeList[CheckX][CheckY]))
                {
                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }
            }

            // Right
            if (CurrentGridPosX != SizeX - 1)
            {
                CheckX = CurrentGridPosX + 1;

                // Check if reached destination
                if (NodeList[CheckX][CheckY].Equals(TargetNode))
                {
                    Debug.Log("Path found to target: " + TargetNode.m_pos.ToString());

                    b_PathFound = true;
                    b_OnWaypoint = false;

                    return true;
                }

                if (ValidateNode(NodeList[CheckX][CheckY]))
                {
                    OpenList.Add(NodeList[CheckX][CheckY]);
                    NeighbourList.Add(NodeList[CheckX][CheckY]);
                }
            }

            // Set all neghbours parent to current node
            foreach (Node aNode in NeighbourList)
            {
                aNode.ParentNode = n_CurrentNode;
            }

            if (NeighbourList.Count <= 0)
                return false;

            // Get neighbour with lowest F value ()
            Node TempLowest = GetLowestF(OpenList);
            OpenList.Remove(TempLowest);
            n_CurrentNode = TempLowest;

            NeighbourList.Clear();
        }

        // Get that neighbour's neighbours, set that neighbour as the curr node

        // Repeat

        // Closed list will be the path to follow

        return false;
    }

    public void FollowPath()
    {
        if (!b_CompletedPath && b_PathFound)
        {
            Node endNode = ClosedList[ClosedList.Count - 1];

            List<Node> Path = new List<Node>();

            Path.Add(GetNode(m_Destination));
            Path.Add(endNode);

            while (endNode.ParentNode != null)
            {
                endNode = endNode.ParentNode;
                Path.Add(endNode);
            }

            Path.Reverse();

            if (i_CurrentIdx >= Path.Count)
            {
                Debug.Log("Path Completed. Current Pos: " + Path[Path.Count - 1].m_pos.ToString());
                Debug.Log("Waypoint status: " + b_OnWaypoint.ToString());

                b_PathFound = false;
                b_CompletedPath = true;
                b_OnWaypoint = false;

                i_CurrentIdx = 0;

                return;
            }

            // DEBUG
            for (int i = 0; i < Path.Count - 1; ++i)
            {
                Debug.DrawLine(Path[i].m_pos, Path[i + 1].m_pos, Random.ColorHSV());
            }

            Vector3 dir = (Path[i_CurrentIdx].m_pos - this.transform.position).normalized * Time.deltaTime * GetComponent<FSMBase>().GetMoveSpeed();

            float offset = 0.0009f;
            if (dir.x <= offset && dir.x >= -offset && dir.y <= offset && dir.y >= -offset && dir.z <= offset && dir.z >= -offset)
            {
                i_CurrentIdx++;
                return;
            }

            this.gameObject.transform.position += dir;

            Debug.DrawLine(this.gameObject.transform.position, Path[i_CurrentIdx].m_pos);

            // Rotate
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = lookRotation;
        }
    }

    public bool CheckIfInGrid()
    {
        return theGridMap.CheckIfInGridMap(this.gameObject.transform.position);
    }

    float GetManhattenDistance(Node aNode)
    {
        return (Mathf.Abs(m_Destination.x - aNode.m_pos.x) + Mathf.Abs(m_Destination.z - aNode.m_pos.z));
    }

    Node GetNode(Vector3 pos)
    {
        // Check using GridBehaviour
        Vector2 check = theGridMap.GetWithinGridPos(pos);

        int SizeX = (int)theGridMap.GetGridSize().x;
        int SizeY = (int)theGridMap.GetGridSize().y;
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                if (i == check.x && j == check.y)
                    return NodeList[i][j];
            }
        }

        return null;
    }

    bool ValidateNode(Node checkNode)
    {
        // Do various checks if node is valid
        if (checkNode == null)
        {
            return false;
        }

        if (checkNode.TileCost == -1)
        {
            return false;
        }

        if (CheckIfInClosedList(checkNode))
        {
            return false;
        }

        return true;
    }

    Node GetLowestF(List<Node> checkList)
    {
        if (checkList.Count <= 0)
            return null;

        int LowestF_Value = 99999;
        int LowestF_Idx = 0;
        for (int i = 0; i < checkList.Count; ++i)
        {
            if (checkList[i].CalculateAccCost() + GetManhattenDistance(checkList[i]) < LowestF_Value)
            {
                LowestF_Value = checkList[i].AccCost + (int)GetManhattenDistance(checkList[i]);
                LowestF_Idx = i;
            }
        }
        return checkList[LowestF_Idx];
    }

    bool CheckIfInClosedList(Node checkNode)
    {
        for (int i = 0; i < ClosedList.Count; ++i)
        {
            if (ClosedList[i].Equals(checkNode))
            {
                return true;
            }
        }

        return false;
    }

    bool CheckIfInOpenList(Node checkNode)
    {
        for (int i = 0; i < OpenList.Count; ++i)
        {
            if (OpenList[i].Equals(checkNode))
            {
                return true;
            }
        }

        return false;
    }

    void RefreshNodeList()
    {
        NodeList.Clear();

        //Init list
        int SizeX = (int)theGridMap.GetGridSize().x;
        int SizeY = (int)theGridMap.GetGridSize().y;

        for (int i = 0; i < SizeX; i++)
        {
            NodeList.Add(new List<Node>());
        }

        // Fill up NodeList
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                Node toAdd = new Node();
                toAdd.Init((int)theGridMap.GetGridCost(i, j), theGridMap.GetVec3Pos(i, j));
                NodeList[i].Add(toAdd);
            }
        }
    }

    public void IncrementIndex()
    {
        if (!b_CompletedPath && b_PathFound && b_OnWaypoint)
            i_CurrentIdx++;
    }

    public void SetOnWaypoint(bool status)
    {
        b_OnWaypoint = status;
    }

    public bool GetWaypointStatus()
    {
        return b_OnWaypoint;
    }

    public void Reset()
    {
        Debug.Log("Reseted");

        i_CurrentIdx = 0;

        b_PathFound = false;
        b_CompletedPath = false;
        b_OnWaypoint = false;
    }
}