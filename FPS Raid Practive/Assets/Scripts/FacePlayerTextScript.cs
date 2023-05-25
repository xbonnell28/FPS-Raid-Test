using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FacePlayerTextScript : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 lookAtVector = new(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(lookAtVector);
    }
}
