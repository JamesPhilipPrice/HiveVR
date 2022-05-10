using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionLayerSwitch : MonoBehaviour
{
    public XRDirectInteractor interactor;

    public InteractionLayerMask whenDisabled;
    public InteractionLayerMask whenEnabled;

    public bool enabledOnStart = false;

    public void Start()
    {
        if (enabledOnStart)
        {
            EnableLayers();
        }
        else
        {
            DisableLayers();
        }
    }

    public void EnableLayers()
    {
        interactor.interactionLayers = whenEnabled;
    }

    public void DisableLayers()
    {
        interactor.interactionLayers = whenDisabled;
    }
}
