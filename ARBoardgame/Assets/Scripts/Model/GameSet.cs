using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSet
{
    public BoardType BoardType { get; private set; }
    public List<PieceInfo> PieceTypes { get; private set; }
    public List<DefaultPosition> DefaultPositions { get; private set; }

    public GameSet(BoardType boardType, List<PieceInfo> pieceTypes, List<DefaultPosition> defaultPositions)
    {
        BoardType = boardType;
        PieceTypes = pieceTypes;
        DefaultPositions = defaultPositions;
    }
}
