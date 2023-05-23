using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargePanel : MonoBehaviour
{
    public TextMeshPro ChargeLevelText;
    public float StartingCharge = 0;
    public float MaxCharge = 100;

    private float _chargeLevel = 0;

    private void Start()
    {
        ChargeLevelText.text = StartingCharge.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            if(_chargeLevel < MaxCharge)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                float chargeToAdd = player.PullHeldCharge();
                _chargeLevel += chargeToAdd;
                ChargeLevelText.text = _chargeLevel.ToString();
            } else
            {
                _chargeLevel = MaxCharge;
                ChargeLevelText.text = "Full Charge";
            }
        }
    }
}
