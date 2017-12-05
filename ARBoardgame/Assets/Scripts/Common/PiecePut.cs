using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PiecePut
{
    [SerializeField]
    private string pieceType;
    public string PieceType
    {
        get
        {
            return pieceType;
        }
        private set
        {
            pieceType = value;
        }
    }

    [SerializeField]
    private SquarePos squarePos;
    public SquarePos SquarePos
    {
        get
        {
            return squarePos;
        }
        private set
        {
            squarePos = value;
        }
    }

    private PiecePut() { }

    public PiecePut(string pieceType, SquarePos squarePos)
    {
        PieceType = pieceType;
        SquarePos = squarePos;
    }
}
