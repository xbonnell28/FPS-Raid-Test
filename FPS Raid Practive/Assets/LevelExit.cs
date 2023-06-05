using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : BaseMechanic
{
    public string targetScene;

    private void Start()
    {
        if (isActive)
        {
            Activate();
        }
    }
    public override void Activate()
    {
        base.Activate();
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = Color.yellow;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
