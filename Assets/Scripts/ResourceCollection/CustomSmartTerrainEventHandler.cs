using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Vuforia;

public class CustomSmartTerrainEventHandler : MonoBehaviour
{
    private ReconstructionBehaviour mReconstructionBehaviour;
    private SurfaceBehaviour mSurfaceBehavour;

    private bool b_PropsReplaced = false;

    public PropBehaviour PropTemplate;
    public SurfaceBehaviour SurfaceTemplate;

    public GameObject TreePrefab;
    public GameObject RockPrefab;

    void Start()
    {
        mReconstructionBehaviour = GetComponent<ReconstructionBehaviour>();
        if (mReconstructionBehaviour)
        {
            mReconstructionBehaviour.RegisterInitializedCallback(OnInitialized);
            mReconstructionBehaviour.RegisterPropCreatedCallback(OnPropCreated);
            mReconstructionBehaviour.RegisterSurfaceCreatedCallback(OnSurfaceCreated);
        }
    }

    void OnDestroy()
    {
        if (mReconstructionBehaviour)
        {
            mReconstructionBehaviour.UnregisterInitializedCallback(OnInitialized);
            mReconstructionBehaviour.UnregisterPropCreatedCallback(OnPropCreated);
            mReconstructionBehaviour.UnregisterSurfaceCreatedCallback(OnSurfaceCreated);
        }
    }

    public void OnInitialized(SmartTerrainInitializationInfo initializationInfo)
    {
    }

    public void OnPropCreated(Prop prop)
    {
        //shows an example of how you could get a handle on the prop game objects to perform different game logic
        if (mReconstructionBehaviour)
        {
            mReconstructionBehaviour.AssociateProp(PropTemplate, prop);
            PropAbstractBehaviour behaviour;
            if (mReconstructionBehaviour.TryGetPropBehaviour(prop, out behaviour))
            {
                behaviour.gameObject.name = "Prop " + prop.ID;
            }
        }
    }

    public void OnSurfaceCreated(Surface surface)
    {
        //shows an example of how you could get a handle on the surface game objects to perform different game logic
        if (mReconstructionBehaviour)
        {
            mReconstructionBehaviour.AssociateSurface(SurfaceTemplate, surface);
            SurfaceAbstractBehaviour behaviour;
            if (mReconstructionBehaviour.TryGetSurfaceBehaviour(surface, out behaviour))
            {
                behaviour.gameObject.name = "Surface " + surface.ID;
            }
        }
    }

    public void StartTerrainGeneration()
    {
        mSurfaceBehavour = FindObjectOfType<SurfaceBehaviour>();
        mSurfaceBehavour.GetComponent<CustomSmartTerrainTrackableEventHandler>().StartRender();
    }

    public void StopTerrainGeneration()
    {
        mReconstructionBehaviour.Reconstruction.Stop();
    }

    public void ResetTerrainGeneration()
    {
        TrackerManager.Instance.GetTracker<SmartTerrainTracker>().Stop();
        mReconstructionBehaviour.Reconstruction.Reset();
        TrackerManager.Instance.GetTracker<SmartTerrainTracker>().Start();

        b_PropsReplaced = false;
        ClearObjects();
    }

    // Function to replace the prop templates with proper models
    public void ReplaceTemplates()
    {
        if (b_PropsReplaced)
            return;

        // Find all props
        PropAbstractBehaviour[] props = GameObject.FindObjectsOfType(typeof(PropAbstractBehaviour)) as PropAbstractBehaviour[];

        // Delete props and replace them with proper models
        foreach (PropAbstractBehaviour prop in props)
        {
            Transform BoundingBox = prop.transform.FindChild("BoundingBoxCollider");
            BoxCollider collider = BoundingBox.GetComponent<BoxCollider>();
            collider.isTrigger = false;

            // Check which model to replace it with
            if (collider.bounds.extents.y > collider.bounds.extents.x)
            {
                // Tree
                GameObject effect = Instantiate(TreePrefab) as GameObject;
                effect.SetActive(true);
                effect.name = "Resource_Credit";
                effect.transform.parent = BoundingBox;
                effect.transform.localPosition = new Vector3(0f, 0.032f, 0f);
                effect.transform.localRotation = Quaternion.identity;

                effect.GetComponent<Resource>().b_IsOriginal = false;
            }
            else
            {
                // Rock
                GameObject effect = Instantiate(RockPrefab) as GameObject;
                effect.SetActive(true);
                effect.name = "Resource_Data";
                effect.transform.parent = BoundingBox;
                effect.transform.localPosition = new Vector3(0f, 0.032f, 0f);
                effect.transform.localRotation = Quaternion.identity;

                effect.GetComponent<Resource>().b_IsOriginal = false;
            }

            prop.SetAutomaticUpdatesDisabled(true);
            Renderer propRenderer = prop.GetComponent<MeshRenderer>();
            if (propRenderer != null)
            {
                Destroy(propRenderer);
            }

        }

        b_PropsReplaced = true;
    }

    public void ClearObjects()
    {
        // Find all Resource Objects
        GameObject[] ToDelete = GameObject.FindGameObjectsWithTag("Resources");

        // Delete them
        foreach (GameObject go in ToDelete)
        {
            Destroy(go);
        }

        // Find all props
        PropAbstractBehaviour[] props = GameObject.FindObjectsOfType(typeof(PropAbstractBehaviour)) as PropAbstractBehaviour[];

        // Delete props and replace them with proper models
        foreach (PropAbstractBehaviour prop in props)
        {
            Destroy(prop);
        }

        // Find all Resource Objects
        GameObject[] PropsToDelete = GameObject.FindGameObjectsWithTag("Prop");

        // Delete them
        foreach (GameObject go in PropsToDelete)
        {
            Destroy(go);
        }
    }
}




