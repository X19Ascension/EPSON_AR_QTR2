using UnityEngine;
using System.Collections;

public class Node {

    public int TileCost;
    public int AccCost;
    public Vector3 m_pos;

    public Node ParentNode = null;

    public void Init(int cost, Vector3 pos)
    {
        ParentNode = null;
        TileCost = cost;
        m_pos = pos;
    }

    public int CalculateAccCost()
    {
        if (ParentNode != null)
        {
            AccCost = this.TileCost + this.ParentNode.CalculateAccCost();
            return AccCost;
        }
        else
        {
            AccCost = this.TileCost;
            return AccCost;
        }
    }

}
