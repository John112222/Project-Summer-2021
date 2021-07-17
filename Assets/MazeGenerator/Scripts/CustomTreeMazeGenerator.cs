using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTreeMazeGenerator : TreeMazeGenerator
{
    public CustomTreeMazeGenerator(int row, int column) : base(row, column)
    {

    }

    private int counter = 0;

    protected override int GetCellInRange(int max)
    {
        Debug.Log(max);
        if (max == 0)
        {
            return 0;
        }
        if (max % 2 == 0)
        {

            return max*4/5;
        }
        else
        {
            return max*5/6;
        }
        //return (max-counter) % max;
    }
}
