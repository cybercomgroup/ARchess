using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePutMSG
{
    public string PieceType { get; private set; }
    public SquarePos SquarePos { get; private set; }

    private PiecePutMSG() { }

    public PiecePutMSG(string pieceType, SquarePos squarePos)
    {
        PieceType = pieceType;
        SquarePos = squarePos;
    }
}
