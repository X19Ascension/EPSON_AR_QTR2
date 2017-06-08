using UnityEngine;
using System.Collections;

public class Wall : BuildingAbstractBase {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    void OnDisable()
    {
        if (!IsPreview)
        {
            base.Shutdown();
        }
    }

    public override void ProduceResource()
    {

    }

    public override int GetPopulationCount()
    {
        return 0;
    }

    public override int GetPopulationCap()
    {
        return 0;
    }

    public override int GetCreditsStorageCap()
    {
        return 0;
    }

    public override int GetDataStorageCap()
    {
        return 0;
    }

    public override int GetPowerStorageCap()
    {
        return 0;
    }

    public override int GetFoodStorageCap()
    {
        return 0;
    }

    public override void Reset()
    {
        this.gameObject.GetComponent<Health>().Reset();
    }
}
