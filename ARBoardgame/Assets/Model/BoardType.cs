using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



/**
 * BoardType keeps track of board attributes.
 * Currently the only attribute is the size.
 * 
 */
[Serializable]
public class BoardType
{
    [SerializeField]
    private int numCols;
    public int NumCols
    {
        get
        {
            return numCols;
        }
        private set
        {
            numCols = value;
        }
    }

    [SerializeField]
    private int numRows;
    public int NumRows
    {
        get
        {
            return numRows;
        }
        private set
        {
            numRows = value;
        }
    }

    public BoardType(int numCols, int numRows)
    {
        NumCols = numCols;
        NumRows = numRows;
    }
}