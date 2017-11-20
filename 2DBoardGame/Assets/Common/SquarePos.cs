using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePos
{
    public int Col { get; private set; }
    public int Row { get; private set; }

    private SquarePos() { }

    public SquarePos(int col, int row)
    {
        Col = col;
        Row = row;
    }
}