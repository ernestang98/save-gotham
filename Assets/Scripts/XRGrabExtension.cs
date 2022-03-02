using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabExtension : XRGrabInteractable
{
    XRBaseInteractor m_HoveringInteractor;

    /// <summary>
    /// (Read Only) The current selecting interactor for this interactable.
    /// </summary>
    public XRBaseInteractor hoveringInteractor => m_HoveringInteractor;

    /// <inheritdoc />
    protected override void OnHoverEntered(XRBaseInteractor interactor)
    {
        if (interactor == null)
            return;
        m_HoveringInteractor = interactor;
        base.OnHoverEntered(interactor);
    }
}
