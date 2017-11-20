using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public SquarePos FromPos { get; private set; }
    public SquarePos ToPos { get; private set; }

    private Move() { }

    public Move(SquarePos fromPos, SquarePos toPos)
    {
        FromPos = new SquarePos(fromPos.Col, fromPos.Row);
        ToPos = new SquarePos(toPos.Col, toPos.Row);
    }
}