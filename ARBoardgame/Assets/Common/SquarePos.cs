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

    public override bool Equals(object obj)
    {
        if (this == obj)
        {
            return true;
        }

        SquarePos squarePos = obj as SquarePos;
        if (squarePos != null)
        {
            return Col == squarePos.Col && Row == squarePos.Row;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        int hashCode = 17;

        hashCode = 31 * hashCode + Col;
        hashCode = 31 * hashCode + Row;

        return hashCode;
    }
}