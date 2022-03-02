using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleBinController : MonoBehaviour
{
    public GameObject gameplayCanvas;

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        interactableObject.AddPoints(this.tag);
    }
}
