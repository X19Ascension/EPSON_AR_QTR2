using UnityEngine;
using Vuforia;

public class CustomSmartTerrainTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    public bool b_TrackingFound;
    public ResourceCollectionManager theCollectionManager;

    private ImageTargetAbstractBehaviour m_ImageTarget;
    private TrackableBehaviour mTrackableBehaviour;

    private bool b_StartedRender = false;

    void Start()
    {
        m_ImageTarget = FindObjectOfType(typeof(ImageTargetAbstractBehaviour)) as ImageTargetAbstractBehaviour;

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        b_TrackingFound = false;
    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    // Function to start Rendering
    public void StartRender()
    {
        if (!b_StartedRender)
        {
            OnTrackingFound();
            b_StartedRender = true;
        }
    }

    private void OnTrackingFound()
    {
        b_TrackingFound = true;

        if (theCollectionManager.GetStage() == RESOURCE_COLLECTON_STAGE.SCANNING || theCollectionManager.GetStage() == RESOURCE_COLLECTON_STAGE.COLLECTING || theCollectionManager.GetStage() == RESOURCE_COLLECTON_STAGE.DONE)
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            WireframeBehaviour[] wireframeComponents = GetComponentsInChildren<WireframeBehaviour>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            // Enable wireframe rendering:
            foreach (WireframeBehaviour component in wireframeComponents)
            {
                component.enabled = true;
            }

            // Show the image target and its children
            if (m_ImageTarget != null)
            {
                Renderer[] rendererComponentsOfCylinder = m_ImageTarget.gameObject.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer component in rendererComponentsOfCylinder)
                {
                    component.enabled = true;
                }
            }
        }
    }


    private void OnTrackingLost()
    {
        b_TrackingFound = false;

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
        WireframeBehaviour[] wireframeComponents = GetComponentsInChildren<WireframeBehaviour>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }

        // Disable wireframe rendering:
        foreach (WireframeBehaviour component in wireframeComponents)
        {
            component.enabled = false;
        }
    }
    
}
