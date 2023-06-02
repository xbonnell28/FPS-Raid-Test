using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechanic : MonoBehaviour
{
    public BaseMechanic linkedMechanic;
    //Determines whether or not this mechanic should be active
    [SerializeField] public bool isActive;

    public virtual void Activate()
    {
        isActive = true;
        enabled = isActive;
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
