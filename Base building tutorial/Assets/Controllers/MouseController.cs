using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public GameObject CursorPrefab;

    private Vector3 lastFramePos;

    private Vector3 currFramePos;

    private Vector3 LMBDragStartPos;

    private List<GameObject> dragPreviewCursors;

    private WorldTile.TileType activeTileType = WorldTile.TileType.Floor;

    private enum Mode { Tile, Item, Move };

    Mode activeMode = Mode.Tile;

    // Use this for initialization
    void Start()
    {
        dragPreviewCursors = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        currFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePos.z = 0;

        // Handle cursor positioning
        // UpdateCursorPos();
        // Handle left mouse button clicks
        UpdateLMBAction();
        // Handle screen dragging
        UpdateCamera();
    }

    //Handles cursor positioning
    //private void UpdateCursorPos()
    //{
    //    WorldTile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(currFramePos);
    //    if (tileUnderMouse != null)
    //    {
    //        Cursor.SetActive(true);
    //        Vector3 cursorPos = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
    //        Cursor.transform.position = cursorPos;
    //    }
    //    else
    //    {
    //        Cursor.SetActive(false);
    //    }
    //}

    // Handles left mouse button interaction
    private void UpdateLMBAction()
    {
        // Return if over UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        WorldTile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(currFramePos);

        // Start LMB drag
        if (Input.GetMouseButtonDown(0))
        {
            LMBDragStartPos = currFramePos;
        }

        // End LMB drag
        if (Input.GetMouseButtonUp(0))
        {
            int startX = Mathf.RoundToInt(LMBDragStartPos.x);
            int startY = Mathf.RoundToInt(LMBDragStartPos.y);

            int endX = Mathf.RoundToInt(currFramePos.x);
            int endY = Mathf.RoundToInt(currFramePos.y);

            if (activeMode == Mode.Move)
            {
                WorldTile fromTile = WorldController.Instance.World.GetTileAt(startX, startY);
                WorldTile toTile = WorldController.Instance.World.GetTileAt(endX, endY);

                WorldController.Instance.World.MovePieceFromTileToTile(fromTile, toTile);

                // Implement move piece
                Debug.Log("Not implemented");
            }
            else
            {
                if (startX > endX)
                {
                    Swap(ref startX, ref endX);
                }

                if (startY > endY)
                {
                    Swap(ref startY, ref endY);
                }

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        WorldTile tile = WorldController.Instance.World.GetTileAt(x, y);
                        if (tile != null)
                        {
                            if (activeMode == Mode.Tile)
                            {
                                tile.Type = activeTileType;
                            }
                            else if (activeMode == Mode.Item)
                            {
                                WorldController.Instance.World.CreatePiece(tile);
                            }

                        }
                    }
                }
            }
        }


        // Clear preview
        foreach (GameObject dragPreviewCursor in dragPreviewCursors)
        {
            Destroy(dragPreviewCursor);
        }
        dragPreviewCursors.Clear();

        // Display preview
        if (Input.GetMouseButton(0) && activeMode != Mode.Move)
        {
            int startX = Mathf.RoundToInt(LMBDragStartPos.x);
            int startY = Mathf.RoundToInt(LMBDragStartPos.y);

            int endX = Mathf.RoundToInt(currFramePos.x);
            int endY = Mathf.RoundToInt(currFramePos.y);

            if (startX > endX)
            {
                Swap(ref startX, ref endX);
            }

            if (startY > endY)
            {
                Swap(ref startY, ref endY);
            }

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    // Display cursor
                    GameObject dragPreviewCursor = (GameObject)Instantiate(CursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    dragPreviewCursor.transform.SetParent(this.transform, true);
                    dragPreviewCursors.Add(dragPreviewCursor);

                }
            }

        }
    }

    private static void Swap<T>(ref T left, ref T right)
    {
        T temp = left;
        left = right;
        right = temp;
    }


    // Handles camera dragging
    private void UpdateCamera()
    {
        // Right or middle mouse button
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Vector3 diff = lastFramePos - currFramePos;
            Camera.main.transform.Translate(diff);
        }

        // Camera.main may have to be updated - see ep 4 17:50
        // Raycasting is likely what we're going to use
        lastFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePos.z = 0;
    }

    public void setModeMovePiece()
    {
        activeMode = Mode.Move;
    }

    public void setModePlacePiece()
    {
        activeMode = Mode.Item;
    }

    public void setModeBuildFloor()
    {
        activeMode = Mode.Tile;
        activeTileType = WorldTile.TileType.Floor;
    }

    public void setModeRemoveTile()
    {
        activeMode = Mode.Tile;
        activeTileType = WorldTile.TileType.Empty;
    }
}
