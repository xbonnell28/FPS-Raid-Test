using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class HiddenCodeBlock : MonoBehaviour
{
    private GameObject player;
    public TextMeshPro correctNumber;

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

    public void MakeVisible(bool makeVisible)
    {
        correctNumber.gameObject.SetActive(makeVisible);
    }
}
