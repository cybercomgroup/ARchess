using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSet
{
    public BoardType BoardType { get; private set; }
    public List<string> PieceTypes { get; private set; }

    public GameSet(BoardType boardType, List<string> pieceTypes)
    {
        BoardType = boardType;
        PieceTypes = pieceTypes;
    }
}
