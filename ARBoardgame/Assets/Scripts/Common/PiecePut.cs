using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePut
{
    public string PieceType { get; private set; }
    public SquarePos SquarePos { get; private set; }

    private PiecePut() { }

    public PiecePut(string pieceType, SquarePos squarePos)
    {
        PieceType = pieceType;
        SquarePos = squarePos;
    }
}
