using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlowerPotController : MonoBehaviour
{
    public XRSocketInteractor xRSocketInteractor;
    public PointsController pointsController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enteredFlowerPot()
    {
        xRSocketInteractor.selectTarget.gameObject.GetComponent<InteractableObject>().AddPoints("Flower");
        xRSocketInteractor.selectTarget.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
