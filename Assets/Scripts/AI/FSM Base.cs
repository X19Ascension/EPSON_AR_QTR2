using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public abstract class FSMBase : MonoBehaviour {

    [Header("MessageBoard Variables")]
    public MessageBoard theBoard;
    public Message CurrentMessage = null;     // Handle to message

    [Header("Enemy Stats")]
    public float AttackRange = 1;
    [Tooltip("Amount of delay between attacks in secs")]
    public float AttackSpeed = 1;
    public int AttackDamage = 1;
    public Vector2 GridPos;

    protected GameObject m_TargetedEnemy = null;

    protected Animator theAnimator;
    protected bool b_InGrid = false;

    double d_TImer; // DEBUG

    // Use this for initialization
    public virtual void Start()
    {
        theAnimator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
            RunFSM();
	}

    public void RunFSM()
    {
        Sense();

        int actValue = Think();
        if (actValue != -1)
        {
            Act(actValue);
        }
    }

    public abstract void Sense();           // get/receive updates from the world
    public abstract int Think();            // process the updates
    public abstract void Act(int value);    // act upon any change in behaviour
    public abstract void ProcessMessage();  // process message received
    public abstract float GetMoveSpeed();

    public Message ReadFromMessageBoard()
    {
        if (theBoard != null)
            return theBoard.GetMessage(this.gameObject.GetInstanceID());
        else
            return null;
    }

    public GameObject GetTarget()
    {
        return m_TargetedEnemy;
    }

    protected bool GetAnimatorIsPlaying()
    {
        return (theAnimator.GetCurrentAnimatorStateInfo(0).length > theAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    List<Vector2> GetGridsInRange(int range)
    {
        List<Vector2> returnList = new List<Vector2>();
        List<Vector2> checkedList = new List<Vector2>();

        returnList = RecursiveFindGrids(GetComponent<Pathfinder>().theGridMap.GetWithinGridPos(gameObject.transform.position), range, checkedList, true);

        return returnList;
    }

    List<Vector2> RecursiveFindGrids(Vector2 checkPos, int tilesLeft, List<Vector2> checkedPos, bool firstRun = false)
    {
        List<Vector2> returnList = new List<Vector2>();

        if (!firstRun)
            returnList.Add(checkPos);

        checkedPos.Add(checkPos);

        if (tilesLeft > 0)
        {
            Vector2 newPos = checkPos + new Vector2(0, 1);
            List<Vector2> tempList1 = new List<Vector2>();
            if (!checkedPos.Contains(newPos) && newPos.y < GetComponent<Pathfinder>().theGridMap.GetGridSize().y && GetComponent<Pathfinder>().theGridMap.GetGridCost((int)newPos.x, (int)newPos.y) != -1)
            {
                tempList1 = RecursiveFindGrids(newPos, tilesLeft - 1, checkedPos);
            }

            newPos = checkPos + new Vector2(0, -1);
            List<Vector2> tempList2 = new List<Vector2>();
            if (!checkedPos.Contains(newPos) && newPos.y >= 0 && GetComponent<Pathfinder>().theGridMap.GetGridCost((int)newPos.x, (int)newPos.y) != -1)
            {
                tempList2 = RecursiveFindGrids(newPos, tilesLeft - 1, checkedPos);
            }

            newPos = checkPos + new Vector2(1, 0);
            List<Vector2> tempList3 = new List<Vector2>();
            if (!checkedPos.Contains(newPos) && newPos.x < GetComponent<Pathfinder>().theGridMap.GetGridSize().x && GetComponent<Pathfinder>().theGridMap.GetGridCost((int)newPos.x, (int)newPos.y) != -1)
            {
                tempList3 = RecursiveFindGrids(newPos, tilesLeft - 1, checkedPos);
            }

            newPos = checkPos + new Vector2(-1, 0);
            List<Vector2> tempList4 = new List<Vector2>();
            if (!checkedPos.Contains(newPos) && newPos.x >= 0 && GetComponent<Pathfinder>().theGridMap.GetGridCost((int)newPos.x, (int)newPos.y) != -1)
            {
                tempList4 = RecursiveFindGrids(newPos, tilesLeft - 1, checkedPos);
            }

            foreach (Vector2 aVec2 in tempList1)
            {
                returnList.Add(aVec2);
            }

            foreach (Vector2 aVec2 in tempList2)
            {
                returnList.Add(aVec2);
            }

            foreach (Vector2 aVec2 in tempList3)
            {
                returnList.Add(aVec2);
            }

            foreach (Vector2 aVec2 in tempList4)
            {
                returnList.Add(aVec2);
            }
        }

        return returnList;
    }

    protected Vector3 GetClosestDestination(int range, GameObject target)
    {
        List<Vector2> AllGrids = GetGridsInRange(range);

        float closestDist = 999;
        int closestItr = 0;
        for(int i = 0; i < AllGrids.Count - 1; ++i)
        {
            Vector3 checkPos = GetComponent<Pathfinder>().theGridMap.GetVec3Pos((int)AllGrids[i].x, (int)AllGrids[i].y);
            if ((target.transform.position - checkPos).magnitude < closestDist)
            {
                closestDist = (target.transform.position - checkPos).magnitude;
                closestItr = i;
            }
        }

        return GetComponent<Pathfinder>().theGridMap.GetVec3Pos((int)AllGrids[closestItr].x, (int)AllGrids[closestItr].y);
    }
}
