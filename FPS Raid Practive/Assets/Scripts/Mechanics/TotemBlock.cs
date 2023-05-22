using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class TotemBlock : MonoBehaviour
{
    private GameObject player;
    public CodeTotem totem;
    public TextMeshPro numberText;
    public int currentNumber;

    private void Start()
    {       
        player = GameObject.FindGameObjectWithTag("Player");
        currentNumber = totem.LOWEST_POSSIBLE_DIGIT;
    }
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 lookAtVector = new(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(lookAtVector);
    }

    public void UpdateCurrentNumber()
    {
        numberText.text = currentNumber.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currentNumber++ == totem.HIGHEST_POSSIBLE_DIGIT)
        {
            currentNumber = totem.LOWEST_POSSIBLE_DIGIT;
        }
        UpdateCurrentNumber();
        totem.CheckForSolution();
    }
}
