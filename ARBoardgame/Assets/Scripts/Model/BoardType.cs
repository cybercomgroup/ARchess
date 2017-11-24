using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * BoarType keeps track of board attributes.
 * Currently the only attribute is the size.
 * 
 */
public class BoardType
{
    public int NumCols { get; private set; }
    public int NumRows { get; private set; }

    public BoardType(int numCols, int numRows)
    {
        NumCols = numCols;
        NumRows = numRows;
    }
}
