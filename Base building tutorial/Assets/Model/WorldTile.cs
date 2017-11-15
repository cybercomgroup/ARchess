using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldTile
{
    public enum TileType { Empty, Floor };

    private GameObject item;

    Action<WorldTile> cbTileTypeChanged;

    private TileType _type = TileType.Empty;
    public TileType Type
    {
        get
        {
            return _type;
        }

        set
        {
            // Call the callback if there's been a change
            if (_type != value)
            {
                _type = value;

                if (cbTileTypeChanged != null)
                {
                    cbTileTypeChanged(this);
                }
            }
        }
    }

    LooseObject looseObj;
    InstalledObject installedObj;

    private World world;
    public int X { get; private set; }
    public int Y { get; private set; }

    public WorldTile(World world, int x, int y)
    {
        this.world = world;
        this.X = x;
        this.Y = y;
    }

    public void RegisterTileTypeChangedCallback(Action<WorldTile> callback)
    {
        cbTileTypeChanged += callback;
    }

    public bool placeObject(GameObject itemInstance)
    {
        if (itemInstance == null)
        {
            item = null;
            return true;
        }

        item = itemInstance;
        return true;
    }
}
