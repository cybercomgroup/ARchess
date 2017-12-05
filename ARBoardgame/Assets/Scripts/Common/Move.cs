using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Move
{
    [SerializeField]
    private SquarePos fromPos;
    public SquarePos FromPos
    {
        get
        {
            return fromPos;
        }
        private set
        {
            fromPos = value;
        }
    }

    [SerializeField]
    private SquarePos toPos;
    public SquarePos ToPos
    {
        get
        {
            return toPos;
        }
        private set
        {
            toPos = value;
        }
    }

    private Move() { }

    public Move(SquarePos fromPos, SquarePos toPos)
    {
        FromPos = new SquarePos(fromPos.Col, fromPos.Row);
        ToPos = new SquarePos(toPos.Col, toPos.Row);
    }
}