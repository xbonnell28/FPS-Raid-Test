using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CodeTotem : MonoBehaviour
{
    public int LOWEST_POSSIBLE_DIGIT;
    public int HIGHEST_POSSIBLE_DIGIT;

    public TotemBlock[] totemBlocks;

    private int[] code;
    private int[] currentGuess;
    private void Start()
    {
        GenerateCode();
    }

    private void GenerateCode()
    {
        code = new int[totemBlocks.Length];
        code = new int[totemBlocks.Length];
        for (int i = 0; i < code.Length; i++)
        {
            code[i] = UnityEngine.Random.Range(LOWEST_POSSIBLE_DIGIT, HIGHEST_POSSIBLE_DIGIT);
        }
        Debug.Log("Current Code: \n");
        for(int i = 0; i < code.Length; i++)
        {
            Debug.Log(code[i]);
        }
    }

    internal void CheckForSolution()
    {
        bool solutionFound = true;
        for(int i = 0; i < totemBlocks.Length; i++)
        {
            if (code[i] != totemBlocks[i].currentNumber)
            {
                solutionFound = false;
                break;
            }
        }
        if(solutionFound)
        {
            Debug.Log("Solution Found");
        }
        else
        {
            Debug.Log("Wrong Solution");
        }
        
    }
}
