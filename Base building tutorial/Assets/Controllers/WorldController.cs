using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    private static WorldController _instance;
    public static WorldController Instance { get; private set; }

    public Sprite floorSprite;

    public Sprite pieceSprite;

    public World World { get; private set; }

    private Dictionary<WorldTile, GameObject> tileGameObjectMap;
    private Dictionary<Piece, GameObject> pieceGameObjectMap;

    private int numPieces;


    // Use this for initialization
    void Start()
    {
        Instance = this;

        // Create a world with empty tiles
        World = new World();

        numPieces = 0;

        World.RegisterPieceCreatedCallback(OnPieceCreated);

        tileGameObjectMap = new Dictionary<WorldTile, GameObject>();
        pieceGameObjectMap = new Dictionary<Piece, GameObject>();

        // Create a GameObject for each of our tiles so they can be displayed
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                WorldTile tileData = World.GetTileAt(x, y);

                GameObject tileGo = new GameObject();
                tileGo.name = "Tile_" + x + "_" + y;
                tileGo.transform.position = new Vector3(x, y, 0);
                tileGo.transform.SetParent(this.transform, true);

                // Add but don't use SpriteRenderer for every tile as they're all empty
                tileGo.AddComponent<SpriteRenderer>();

                tileData.RegisterTileTypeChangedCallback( (tile) => { OnTileTypeChanged(tile, tileGo);  } );
                
                
                
                /*WorldTile tileData = world.GetTileAt(x, y);

                if (tileData.Type == WorldTile.TileType.Floor)
                {
                    tileSr.sprite = floorSprite;
                }*/
            }
        }

        World.RandomizeTiles();
    }

    void OnTileTypeChanged(WorldTile tileData, GameObject tileGo)
    {
        switch (tileData.Type)
        {
            case WorldTile.TileType.Floor:
                tileGo.GetComponent<SpriteRenderer>().sprite = floorSprite;
                break;

            case WorldTile.TileType.Empty:
                tileGo.GetComponent<SpriteRenderer>().sprite = null;
                break;
            default:
                Debug.LogError("OnTileTypeChanged: Unrecognized TileType");
                break;
        }
        /*if (tileData == WorldTile.TileType.Floor)
        {
            tileGo.GetComponent<SpriteRenderer>().sprite == floorSprite;
        }
        else if(tileData == WorldTile.TileType.Floor)*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    public WorldTile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.RoundToInt(coord.x);
        int y = Mathf.RoundToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }

    // Create visuals for the piece  
    public void OnPieceCreated(Piece piece)
    {
        GameObject pieceGO = new GameObject();

        pieceGameObjectMap.Add(piece, pieceGO);

        pieceGO.name = "Piece_" + numPieces;
        numPieces++;
        pieceGO.transform.position = new Vector3(piece.Tile.X, piece.Tile.Y, 0);
        pieceGO.transform.SetParent(this.transform, true);

        pieceGO.AddComponent<SpriteRenderer>().sprite = pieceSprite;

        piece.RegisterOnPieceChangedCallback(OnPieceChanged);
    }

    private void OnPieceChanged(Piece piece)
    {
        GameObject pieceGO = pieceGameObjectMap[piece];

        pieceGO.transform.position = new Vector3(piece.Tile.X, piece.Tile.Y, 0);
    }
}