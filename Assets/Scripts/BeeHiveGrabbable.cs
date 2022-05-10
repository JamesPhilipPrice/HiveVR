using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BeeHiveGrabbable : XRGrabInteractable
{
    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotation = Quaternion.identity;

    public InteractionLayerSwitch[] layerSwitchers;

    public bool blockingInteractibles = false;

    public bool stuck = true;

    private void Start()
    {
        if (stuck) this.enabled = false;
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        if(interactor is XRDirectInteractor)
        {
            StoreInteractor(interactor);
            MatchAttachPoints(interactor);

            if (blockingInteractibles)
            {
                foreach (InteractionLayerSwitch switcher in layerSwitchers)
                {
                    switcher.EnableLayers();
                }
            }
        }
        else if (interactor is XRSocketInteractor)
        {
            if (blockingInteractibles)
            {
                foreach (InteractionLayerSwitch switcher in layerSwitchers)
                {
                    switcher.DisableLayers();
                }
            }
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        if (interactor is XRDirectInteractor)
        {
            ResetAttachPoints(interactor);
            ClearInteractor(interactor);
        }
    }

    //Custom stuff
    private void MatchAttachPoints(XRBaseInteractor interactor)
    {
        bool hasAttach = attachTransform != null;

        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    private void ResetAttachPoints(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localPosition = interactorPosition;
        interactor.attachTransform.localRotation = interactorRotation;
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = interactor.attachTransform.localPosition;
        interactorRotation = interactor.attachTransform.localRotation;
    }

    private void ClearInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = Vector3.zero;
        interactorRotation = Quaternion.identity;
    }
}
