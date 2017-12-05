using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SquarePos
{
    [SerializeField]
    private int col;
    public int Col {
        get
        {
            return col;
        }
        private set
        {
            col = value;
        }
    }

    [SerializeField]
    private int row;
    public int Row {
        get
        {
            return row;   
        }
        private set
        {
            row = value;
        }
    }

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