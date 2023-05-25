using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : BaseMechanic
{
    public string targetScene;
    private void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
