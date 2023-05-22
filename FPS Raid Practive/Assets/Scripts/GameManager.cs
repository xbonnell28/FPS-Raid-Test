using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager _instance;
    public GameManager instance {
        get {
            if(_instance == null)
            { 
                _instance = new GameManager();
            }
            return _instance;
        }
    }


}
    