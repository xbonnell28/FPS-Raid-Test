using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemCode : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        Vector3 playerVector = player.transform.position;
        playerVector.y = 0;
        gameObject.transform.LookAt(playerVector);
    }
}
