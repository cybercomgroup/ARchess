using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public int FromCol { get; private set; }
    public int FromRow { get; private set; }
    public int ToCol { get; private set; }
    public int ToRow { get; private set; }

    public Move(int fromCol, int fromRow, int toCol, int toRow)
    {
        FromCol = fromCol;
        FromRow = fromRow;
        ToCol = toCol;
        ToRow = toRow;
    }
}
