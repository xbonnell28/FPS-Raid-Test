using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargePanel : MonoBehaviour
{
    public TextMeshPro ChargeLevelText;
    public int StartingCharge = 0;

    private int _chargeLevel = 0;

    private void Start()
    {
        ChargeLevelText.text = StartingCharge.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            PlayerController player = other.GetComponent<PlayerController>();
            int chargeToAdd = player.PullHeldCharge();
            _chargeLevel += chargeToAdd;
            ChargeLevelText.text = _chargeLevel.ToString();
        }
    }
}
