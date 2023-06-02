using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargePanel : BaseMechanic
{
    public TextMeshPro ChargeLevelText;
    public float StartingCharge = 0;
    public float MaxCharge = 100;

    private float _chargeLevel = 0;

    private void Start()
    {
        UpdateText();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.gameObject.CompareTag("Player")) 
        {
            if(_chargeLevel < MaxCharge)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                float chargeToAdd = player.PullHeldCharge();
                _chargeLevel += chargeToAdd;
                UpdateText();
            }
            
            if(_chargeLevel >= MaxCharge)
            {
                _chargeLevel = MaxCharge;
                ChargeLevelText.text = "Full";
                ActivateLinkedMechanic();
                Deactivate();
            }
        }
    }

    private void UpdateText()
    {
        ChargeLevelText.text = _chargeLevel.ToString() + "/" + MaxCharge.ToString();
    }
}
