using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public PointsController pointsController;
    public abstract void AddPoints(string tag);
}
