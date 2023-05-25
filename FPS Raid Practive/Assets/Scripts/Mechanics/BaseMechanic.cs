using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechanic : MonoBehaviour
{
    public BaseMechanic linkedMechanic;
    //Determines whether or not this mechanic should be active
    public bool isActive { get; set; }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public void ActivateLinkedMechanic()
    {
        if(linkedMechanic != null)
        {
            linkedMechanic.Activate();
        }
    }
}
