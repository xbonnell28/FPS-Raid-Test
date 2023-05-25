using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CodeTotem : BaseMechanic
{
    public int LOWEST_POSSIBLE_DIGIT;
    public int HIGHEST_POSSIBLE_DIGIT;

    public TotemBlock[] totemBlocks;
    public HiddenCodeBlock[] hiddenCodeBlock;
    public Collider blocker;

    private int[] code;
    private void Start()
    {
        GenerateCode();
        UpdateHiddenCodeBlocks();
        // Add error check to make sure there are and equal number of hidden blocks to totem blocks
    }

    private void Update()
    {
        blocker.enabled = !isActive;
    }

    private void UpdateHiddenCodeBlocks()
    {
        for(int i = 0; i < hiddenCodeBlock.Length; i++)
        {
            hiddenCodeBlock[i].correctNumber.text = code[i].ToString();
        }
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
            ActivateLinkedMechanic();
        }
        else
        {
            Debug.Log("Wrong Solution");
        }
        
    }
}
