using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Piece
{
    public WorldTile Tile { get; private set; }

    Action<Piece> cbOnPieceChanged;

    public Piece(WorldTile tile)
    {
        Tile = tile;
    }

    /*
    public void PlacePiece()
    {
        
    }
    */

    public void MoveToTile(WorldTile tile)
    {
        Tile = tile;
        if (cbOnPieceChanged != null)
        {
            cbOnPieceChanged(this);
        }
    }

    public void RegisterOnPieceChangedCallback(Action<Piece> callback)
    {
        cbOnPieceChanged += callback;
    }

    public void UnregisterOnPieceChangedCallback(Action<Piece> callback)
    {
        cbOnPieceChanged -= callback;
    }
}