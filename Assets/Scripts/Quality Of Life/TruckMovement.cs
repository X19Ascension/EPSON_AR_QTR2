using UnityEngine;
using System.Collections;

public class TruckMovement : MonoBehaviour {

    public enum TRUCK_STAGES
    {
        IDLE,
        LIFT_UP,
        MOVING,
        TOUCHDOWN,
    }

    public TRUCK_STAGES CurrentStage;
    public GameObject TownHall;
    public GameObject Target;
    public float offset = 0.5f;
     
    float timeLeft = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        GameObject[] Buildings = GameObject.FindGameObjectsWithTag("Buildings");
        GameObject[] Cube = GameObject.FindGameObjectsWithTag("Cube");  
        for (int i = 0; i < Buildings.Length; ++i)
        {
            Target = Buildings[i];
        }


            timeLeft -= Time.deltaTime;  
                        
        switch (CurrentStage)
        {
            case TRUCK_STAGES.IDLE:
                // Check if have building to fly to (except TownHall)
                Buildings = GameObject.FindGameObjectsWithTag("Buildings");
                
                // if no building
                //if (Buildings.Length == 1)
                //{
                    // only townhall exist
                //}
                Cube = GameObject.FindGameObjectsWithTag("Cube");
                // Loop thru the array
                for (int i = 0; i < Buildings.Length; ++i)
                {
                    // If have any other building 
                    if (!Buildings[i].name.Contains("TownHall"))                                                                                                                                                                                                                                                       
                    {
                        // Loop thru all cubes
                        for (int j = 0; j < Cube.Length; ++j)
                        {
                            
                            //if (Cube[i].GetComponent<TruckMovement>().Target == Buildings[i])
                            //{
                            //If no cube already going there  
                            //cube position != target.position
                            if (this.gameObject.transform.position != Target.transform.position)
                            {
                                Target.transform.position = GameObject.FindGameObjectWithTag("Buildings").transform.position;//set building as target
                                CurrentStage = TRUCK_STAGES.LIFT_UP;//go to lift up state
      
                            }
                           

                        }

                    }
                                
                }
                break;

            case TRUCK_STAGES.LIFT_UP:
                if (timeLeft < 0)//wait for 5 secs
                {
                    this.gameObject.transform.Translate(new Vector3(0, 1 * Time.deltaTime, 0));//move up
                        if (this.gameObject.transform.position.y > 1)//reach a certain height
                        {
                            CurrentStage = TRUCK_STAGES.MOVING;//go to moving state

                        }
                }                               
                break;

            case TRUCK_STAGES.MOVING:
                Vector3 dir = (Target.transform.position - this.gameObject.transform.position).normalized;//direction to move
                dir.y = 0;
                this.gameObject.transform.Translate(dir * Time.deltaTime);//move towards target position

                 
                //this.gameObject.transform.Translate(new Vector3(1 * Time.deltaTime, 0, 0));

                if (Target.transform.position.x >= gameObject.transform.position.x - offset && Target.transform.position.z >= gameObject.transform.position.z - offset && Target.transform.position.x <= gameObject.transform.position.x + offset && Target.transform.position.z <= gameObject.transform.position.z + offset)  //if reach above target
                    {
                        CurrentStage = TRUCK_STAGES.TOUCHDOWN;//go to touchdown state 

                    }
                break;

            case TRUCK_STAGES.TOUCHDOWN:
                this.gameObject.transform.Translate(new Vector3(0, -1 * Time.deltaTime, 0));//move down
                if (this.gameObject.transform.position.y < Target.transform.position.y)//lower than target 
                {
                    CurrentStage = TRUCK_STAGES.IDLE;//go to idle state

                }
                break;
        }

	}
}
