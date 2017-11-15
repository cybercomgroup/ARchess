using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World
{
    private WorldTile[,] tiles;

    private Dictionary<WorldTile, Piece> tilePieceMap;

    public int Width { get; private set; }
    public int Height { get; private set; }

    Action<Piece> cbPieceCreated;

    public World(int width = 100, int height = 100)
    {
        this.Width = width;
        this.Height = height;

        tiles = new WorldTile[width, height];

        tilePieceMap = new Dictionary<WorldTile, Piece>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new WorldTile(this, x, y);
            }
        }

        Debug.Log("World created with " + (width * height) + " tiles");
    }

    public void RandomizeTiles()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (UnityEngine.Random.Range(0, 4) < 3)
                {
                    tiles[x, y].Type = WorldTile.TileType.Empty;
                }
                else
                {
                    tiles[x, y].Type = WorldTile.TileType.Floor;
                }

                // tiles[x, y] = new Tile(this, x, y);
            }
        }

    }

    public WorldTile GetTileAt(int x, int y)
    {
        if (x > Width || x < 0 || y > Height || y < 0)
        {
            Debug.LogError("Tile [" + x + ", " + y + "] is out of bounds");
            return null;
        }

        return tiles[x, y];
    }

    public void CreatePiece(WorldTile tile)
    {
        Piece piece = new Piece(tile);

        tilePieceMap.Add(tile, piece);

        if (cbPieceCreated != null)
        {
            cbPieceCreated(piece);
        }
    }

    // Don't need this one right?
    public Piece GetPieceAt(int x, int y)
    {
        WorldTile tile = GetTileAt(x, y);

        if (tile != null && tilePieceMap.ContainsKey(tile))
        {
            return tilePieceMap[tile];
        }
        else
        {
            return null;
        }
    }

    // Adjust logic
    public bool MovePieceFromTileToTile(WorldTile fromTile, WorldTile toTile)
    {
        Piece piece;

        if (tilePieceMap.ContainsKey(fromTile) && toTile != null)
        {
            piece = tilePieceMap[fromTile];
            tilePieceMap.Remove(fromTile);
        }
        else
        {
            return false;
        }

        tilePieceMap.Add(toTile, piece);
        piece.MoveToTile(toTile);
        return true;
    }

    public void RegisterPieceCreatedCallback(Action<Piece> callback)
    {
        cbPieceCreated += callback;
    }

    public void UnregisterPieceCreatedCallback(Action<Piece> callback)
    {
        cbPieceCreated -= callback;
    }
}
