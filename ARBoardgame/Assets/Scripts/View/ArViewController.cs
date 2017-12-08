using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.HelloAR;
using UnityEngine;

public class ArViewController : MonoBehaviour
{
    private string gameName;
    private GameStarted gameStarted;

    public GameObject planePrefab;

    private GameObject boardGO;
    private GameObject[,] boardTiles;

    private float tileWidth;
    private float tileHeight;

    private List<TrackedPlane> m_newPlanes = new List<TrackedPlane>();

    private Dictionary<string, Object> pieceTypeToModelMap;

    private bool BoardPositioned;
    private GameObject heldPiece;

    private Color[] m_planeColors = new Color[] {
        new Color(1.0f, 1.0f, 1.0f),
        new Color(0.956f, 0.262f, 0.211f),
        new Color(0.913f, 0.117f, 0.388f),
        new Color(0.611f, 0.152f, 0.654f),
        new Color(0.403f, 0.227f, 0.717f),
        new Color(0.247f, 0.317f, 0.709f),
        new Color(0.129f, 0.588f, 0.952f),
        new Color(0.011f, 0.662f, 0.956f),
        new Color(0f, 0.737f, 0.831f),
        new Color(0f, 0.588f, 0.533f),
        new Color(0.298f, 0.686f, 0.313f),
        new Color(0.545f, 0.764f, 0.290f),
        new Color(0.803f, 0.862f, 0.223f),
        new Color(1.0f, 0.921f, 0.231f),
        new Color(1.0f, 0.756f, 0.027f)
    };

    // NOTE: Needed?
    private static readonly Vector3 SCALE = new Vector3(0.8F, 5, 0.8F);

    public const string HIGHLIGHTABLE = "Highlightable";
    public const string PICKUPABLE = "Pickup";
    public const string HIGHLIGHPICKUP = "HighlightPickup";

    // Handling subscribing and unsubscribing to the notifications in OnEnable and OnDisable is a "better practice" method to prevent
    // mem leakage, subscribed no longer existing objects
    void OnEnable()
    {
        this.AddObserver(OnLMBClick, ArInteractionController.AR_CLICK);
        this.AddObserver(OnCameraUpdate, ArInteractionController.ARCAMERA_UPDATE);
        this.AddObserver(OnGameCreated, GameController.GAME_CREATED);
        this.AddObserver(OnPiecePickedFromMenu, GameController.PIECE_PICKED_FROM_MENU);
        this.AddObserver(OnPiecePickedFromBoard, GameController.PIECE_PICKED_FROM_BOARD);
        this.AddObserver(OnPiecePut, GameController.PIECE_PUT);
    }


    void OnDisable()
    {
        this.RemoveObserver(OnLMBClick, ArInteractionController.AR_CLICK);
        this.RemoveObserver(OnCameraUpdate, ArInteractionController.ARCAMERA_UPDATE);
        this.RemoveObserver(OnGameCreated, GameController.GAME_CREATED);
        this.RemoveObserver(OnPiecePickedFromMenu, GameController.PIECE_PICKED_FROM_MENU);
        this.RemoveObserver(OnPiecePickedFromBoard, GameController.PIECE_PICKED_FROM_BOARD);
        this.RemoveObserver(OnPiecePut, GameController.PIECE_PUT);
    }

    public void Update()
    {
        // The tracking state must be FrameTrackingState.Tracking in order to access the Frame.
        if (Frame.TrackingState != FrameTrackingState.Tracking)
        {
            const int LOST_TRACKING_SLEEP_TIMEOUT = 15;
            Screen.sleepTimeout = LOST_TRACKING_SLEEP_TIMEOUT;
            return;
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Frame.GetNewPlanes(ref m_newPlanes);

        // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
        for (int i = 0; i < m_newPlanes.Count; i++)
        {
            // Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
            // the origin with an identity rotation since the mesh for our prefab is updated in Unity World
            // coordinates.
            GameObject planeObject = Instantiate(planePrefab, Vector3.zero, Quaternion.identity,
                transform);
            planeObject.GetComponent<TrackedPlaneVisualizer>().SetTrackedPlane(m_newPlanes[i]);
            foreach (var collider in planeObject.GetComponentsInChildren<Collider>())
                collider.gameObject.layer = 8;

            // Apply a random color and grid rotation.
            planeObject.GetComponent<Renderer>().material.SetColor("_GridColor", m_planeColors[Random.Range(0,
                m_planeColors.Length - 1)]);
            planeObject.GetComponent<Renderer>().material.SetFloat("_UvRotation", Random.Range(0.0f, 360.0f));
        }
    }

    public void OnGameCreated(object sender, object args)
    {
        pieceTypeToModelMap = new Dictionary<string, Object>();
        gameStarted = (GameStarted)args;
        gameName = gameStarted.Name;

        // Set up array for board tile references
        boardTiles = new GameObject[gameStarted.GameSet.BoardType.NumCols, gameStarted.GameSet.BoardType.NumRows];

        // NOTE: How this is handled could probably be improved

        foreach (string pieceType in gameStarted.GameSet.PieceTypes)
        {

            pieceTypeToModelMap[pieceType] = Resources.Load("Games/" + gameName + "/Pieces/" + pieceType);

        }
    }

    public void OnCameraUpdate(object sender, object args)
    {
        Transform transform = (Transform)args;
        Ray ray = new Ray(transform.position, transform.forward);
        int layerMask = 1 << LayerMask.NameToLayer("Default");
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
        {
            if (hit.collider.gameObject.CompareTag(HIGHLIGHTABLE) || hit.collider.gameObject.CompareTag(HIGHLIGHPICKUP))
            {
                hit.collider.gameObject.GetComponent<HighlightView>().Highlighted = true;
            }
        }
    }

    public void OnLMBClick(object sender, object args)
    {
        Transform transform = (Transform)args;
        Ray ray = new Ray(transform.position, transform.forward);

        if (!BoardPositioned)
        {
#if UNITY_EDITOR
            int layerMask = 1 << LayerMask.NameToLayer("ARGameObject");
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
            {
                Vector3 pos = hit.point;
#else
			TrackableHitFlag raycastFilter = TrackableHitFlag.PlaneWithinBounds | TrackableHitFlag.PlaneWithinPolygon;
			TrackableHit trackHit;
			if (Session.Raycast(ray, raycastFilter, out trackHit))
			{
				Vector3 pos = trackHit.Point;
#endif
                BoardPositioned = true;
                boardGO = new GameObject();

                boardGO = Instantiate(Resources.Load("Board", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
                boardGO.name = "Board";
                boardGO.transform.SetParent(this.transform, true);
                GameObject playFieldGO = boardGO.transform.GetChild(0).gameObject;

                //boardGO.transform.GetChild(0).GetComponent<Renderer>().material = Resources.Load<Material>("Games/" + gameName + "/board");
                playFieldGO.GetComponent<Renderer>().material = Resources.Load<Material>("Games/" + gameName + "/board");
                playFieldGO.name = "PlayField";
                //boardGO.GetComponent<Renderer>().material = Resources.Load<Material>("Games/" + gameName + "/board");
                // boardGO.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Games/" + gameName + "/board");


                int cols = gameStarted.GameSet.BoardType.NumCols;
                int rows = gameStarted.GameSet.BoardType.NumRows;

                // tileWidth = playFieldGO.GetComponent<Renderer>().bounds.size.x / cols;
                // tileHeight = playFieldGO.GetComponent<Renderer>().bounds.size.z / rows;

                GameObject tileGO;

                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        tileGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
                        boardTiles[i, j] = tileGO;
                        tileGO.AddComponent<HighlightView>();
                        tileGO.tag = HIGHLIGHTABLE;
                        tileGO.name = i + "," + j;
                        tileGO.layer = LayerMask.NameToLayer("TilesAndPieces");

                        tileGO.transform.SetParent(playFieldGO.transform, true);
                        //tileGO.transform.localPosition = Vector3.zero;
                        tileGO.transform.localPosition = new Vector3((i - 4 + 0.5f) / (float)cols, (j - 4 + 0.5f) / (float)rows, 0);
                        tileGO.transform.localScale = new Vector3(1 / (float)cols, 1 / (float)rows, 1);

                        tileGO.transform.Rotate(new Vector3(90, 0, 0));

                        // NOTE: For showing the positions of the tiles
                        // tileGO.GetComponent<Renderer>().material = (i + j) % 2 == 0 ? Resources.Load<Material>("BlueMaterial") : Resources.Load<Material>("RedMaterial");
                        // Hide the tiles
                        tileGO.GetComponent<Renderer>().enabled = false;

                        //go.tag = "Tile"; // Sätt tag så att man kan interagera med rutorna.


                        //tileGO.transform.SetParent(boardGO.transform);
                    }
                }


                // GameObject board = Instantiate(boardPrefab, pos, Quaternion.identity);
                Vector3 lookPos = new Vector3(transform.position.x, pos.y, transform.position.z);
                boardGO.transform.LookAt(lookPos);
                boardGO.transform.Rotate(0, 180, 0, Space.World);
            }
            return;
        }
        else
        {
            // We are only interested in collisions with tiles and pieces
            int layerMask = 1 << LayerMask.NameToLayer("TilesAndPieces");
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
            {
                GameObject hitObj = hit.collider.gameObject;

                // Using the names of the objects is one way to handle position info...
                string[] pos = hitObj.name.Split(',');
                int colPos = System.Convert.ToInt32(pos[0]);
                int rowPos = System.Convert.ToInt32(pos[1]);

                this.PostNotification(GameController.SQUARE_CLICKED, new SquarePos(colPos, rowPos));
                #region Hide for now 
                /*
                if (holdingObject == null && (hitObj.CompareTag(PICKUPABLE) || hitObj.CompareTag(HIGHLIGHPICKUP)))
                {
                    holdingObject = hitObj;
                    holdingObject.transform.parent = transform;
                    holdingObject.transform.localPosition = new Vector3(0, 0.09999F, .2F);
                    holdingObject.transform.localEulerAngles = new Vector3(-20, 0, 0);
                }
                else if (holdingObject != null)
                {	
                    if (hitObj.CompareTag(PICKUPABLE) || hitObj.CompareTag(HIGHLIGHPICKUP))
                    {
                        //Placed on another piece
                    } 
                    else 
                    {
                        //Verify that it's a square and send info of placement
                        //this.PostNotification(SQUARE_LMB_CLICKED, pos);
                        holdingObject.transform.parent = hitObj.transform;
                        holdingObject.transform.localScale = SCALE;
                        holdingObject.transform.localPosition = new Vector3(0, 0, -2.4F);
                        holdingObject.transform.localEulerAngles = new Vector3(-90, 0, 0);
                    }					

                    holdingObject = null;
                }

                this.PostNotification(SQUARE_LMB_CLICKED);
                return;
                */
                #endregion
            }
        }
        this.PostNotification(GameController.OUTSIDE_CLICKED);
    }

    /// <summary>  
    ///  Handles the Piece put event.
    ///  Moves the held game object, the piece, to the specified tile
    /// </summary>
    public void OnPiecePickedFromMenu(object sender, object args)
    {
        heldPiece = Instantiate(pieceTypeToModelMap[(string)args]) as GameObject;
        heldPiece.AddComponent<HighlightView>();
        heldPiece.tag = HIGHLIGHPICKUP;
        // NOTE: At least temp solution for size - scaling
        heldPiece.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        heldPiece.transform.parent = Camera.main.transform;
        heldPiece.transform.localPosition = new Vector3(0.3f, 0.1F, 0.5F);
        heldPiece.transform.localEulerAngles = new Vector3(-120, 0, 0);
    }

    /// <summary>  
    ///  Handles the Piece put event.
    ///  Moves the held game object, the piece, to the specified tile
    /// </summary>
    public void OnPiecePut(object sender, object args)
    {
        SquarePos squarePos = (SquarePos)args;

        if (boardTiles[squarePos.Col, squarePos.Row].transform.GetChildCount() > 0)
        {
            Destroy(boardTiles[squarePos.Col, squarePos.Row].transform.GetChild(0).gameObject, 0);
        }

        GameObject pieceToPut = heldPiece;
        heldPiece = null;

        pieceToPut.transform.parent = boardTiles[squarePos.Col, squarePos.Row].transform;
        pieceToPut.transform.localRotation = pieceToPut.transform.parent.localRotation;
        pieceToPut.transform.localRotation = Quaternion.Euler(180, 0, 0);
        // pieceToPut.transform.localPosition = pieceToPut.transform.parent.transform.localPosition;

        // NOTE: This could probably be improved
        pieceToPut.transform.localPosition = new Vector3(0, 0, -0.4f);


    }

    /// <summary>  
    ///  Handles the Piece put event.
    ///  Moves the held game object, the piece, to the specified tile
    /// </summary>
    public void OnPiecePickedFromBoard(object sender, object args)
    {
        SquarePos squarePos = (SquarePos)args;

        heldPiece = boardTiles[squarePos.Col, squarePos.Row].transform.GetChild(0).gameObject;

        heldPiece.transform.parent = Camera.main.transform;
        heldPiece.transform.localPosition = new Vector3(0.3f, 0.1F, 0.5F);
        //heldPiece.transform.localPosition = heldPiece.transform.parent.localPosition;
    }
}
