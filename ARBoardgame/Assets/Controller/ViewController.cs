using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    private string gameName;

    private int numPiecesCreated;

    // Notification names
    public const string SQUARE_RMB_CLICKED = "Square RMB clicked";
    public const string SQUARE_LMB_CLICKED = "Square LMB clicked";
    public const string OUTSIDE_LMB_CLICKED = "Outside board LMB clicked";

    private Dictionary<string, Sprite> gamePieceTypeToSpriteMap;

    private GameObject[,] board;
    private GameObject boardGO;
    // Not really required, just for quicker access
    private Vector3 boardPos;


    // Sorting Layers - used by the sprites and won't be required once models are used
    private const string PIECE_LAYER = "PieceLayer";
    private const string BOARD_LAYER = "BoardLayer";


    // NOTE: Should each class handle its notification subcsriptions? Handle subscriptions in GameController maybe?

    // Handling subscribing and unsubscribing to the notifications in OnEnable and OnDisable is a "better practice" method to prevent
    // mem leakage, subscribed no longer existing objects
    // NOTE: OnEnable happens before Start
    void OnEnable()
    {
        this.AddObserver(OnBoardPostioned, InteractionController.BOARD_POSITIONED);
        this.AddObserver(OnLMBClick, InteractionController.LMB_CLICK);
        this.AddObserver(OnRMBClick, InteractionController.RMB_CLICK);
        this.AddObserver(OnPiecePut, GameController.PIECE_PUT);
        this.AddObserver(OnPieceMoved, GameController.PIECE_MOVED);
        this.AddObserver(OnPieceRemoved, GameController.PIECE_REMOVED);
        this.AddObserver(OnGameCreated, GameController.GAME_CREATED);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnBoardPostioned, InteractionController.BOARD_POSITIONED);
        this.RemoveObserver(OnLMBClick, InteractionController.LMB_CLICK);
        this.RemoveObserver(OnRMBClick, InteractionController.RMB_CLICK);
        this.RemoveObserver(OnPiecePut, GameController.PIECE_PUT);
        this.RemoveObserver(OnPieceMoved, GameController.PIECE_MOVED);
        this.RemoveObserver(OnPieceRemoved, GameController.PIECE_REMOVED);
        this.RemoveObserver(OnGameCreated, GameController.GAME_CREATED);
    }

    // Use this for initialization
    // NOTE: OnEnable happens before Start
    void Start()
    {
        // gamePieceTypeToSpriteMap = new Dictionary<string, Sprite>();
        // gamePieceToGameObjectMap = new Dictionary<string, GameObject>();


        // gamePieceTypeToSpriteMap["pawn"] = pawn;
    }

    // OnGameCreated is responsible for setting 
    // Should contain info about which game that has been started: game name, list of piece names and board size
    // Is game name really needed though?
    public void OnGameCreated(object sender, object args)
    {
        gamePieceTypeToSpriteMap = new Dictionary<string, Sprite>();

        GameStarted gameStarted = (GameStarted)args;
        gameName = gameStarted.Name;
        // Create the board 
        board = new GameObject[gameStarted.GameSet.BoardType.NumCols, gameStarted.GameSet.BoardType.NumRows];

        // Map every available pieceTypes to their respective sprite, later model
        foreach (string pieceType in gameStarted.GameSet.PieceTypes)
        {
            // Use the selected game set's sprite resources for the pieces
            gamePieceTypeToSpriteMap[pieceType] = Resources.Load<Sprite>("Games/" + gameName + "/Pieces/" + pieceType);
        }
    }

    public void OnBoardPostioned(object sender, object args)
    {
        Vector3 pos = (Vector3)args;
        // The currently used solution for determining cursor postion results in a z axis pos on the "camera cutoff plane"
        // Set z axis pos to 0 instead
        pos.z = 0;
        // Not really required, just for quicker access
        boardPos = pos;

        boardGO = new GameObject();
        boardGO.name = "Board";

        boardGO.transform.position = pos;
        boardGO.transform.SetParent(this.transform, true);
        // Use the selected game set's board sprite resource. This method means that the board sprite needs to be in Resources/Games/<gameName>/ and have the name board
        boardGO.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Games/" + gameName + "/board");
        // Setting layer for sorting order. Probably not very important for our future real 3D view
        boardGO.GetComponent<Renderer>().sortingLayerName = BOARD_LAYER;
    }

    public void OnLMBClick(object sender, object args)
    {
        Vector3 pos = (Vector3)args;

        // Debug.Log("ViewController - position left clicked: " + pos + "\n");

        SquarePos squarePos = GetSquarePosOfVectPos(pos);

        if (squarePos != null)
        {
            // Debug.Log("ViewController - square (" + squarePos.Col + "," + squarePos.Row + ") left clicked");

            this.PostNotification(SQUARE_LMB_CLICKED, squarePos);
        }
        else
        {
            this.PostNotification(OUTSIDE_LMB_CLICKED);
        }
    }



    //temp for testing. count is for test message logging; message isn't printed every time otherwise
    int count;


    // Notify observers which square has been RMB clicked
    //
    public void OnRMBClick(object sender, object args)
    {
        Vector3 pos = (Vector3)args;

        SquarePos squarePos = GetSquarePosOfVectPos(pos);


        if (squarePos != null)
        {
            this.PostNotification(SQUARE_RMB_CLICKED, squarePos);
        }
        else
        {
            Debug.Log(count + ": Outside board");
        }

        // count is for test message logging; message isn't printed every time otherwise
        count++;
    }

    // When notified that piece has been put
    //
    public void OnPiecePut(object sender, object args)
    {
        PiecePut piecePut = (PiecePut)args;
        string pieceType = piecePut.PieceType;

        Vector3 pos = GetVectPosOfSquarePos( piecePut.SquarePos );

        GameObject pieceGO = new GameObject();
        pieceGO.name = "Piece_" + numPiecesCreated;
        numPiecesCreated++;

        pieceGO.transform.position = pos;
        pieceGO.transform.SetParent(this.transform, true);

        pieceGO.AddComponent<SpriteRenderer>().sprite =  gamePieceTypeToSpriteMap[pieceType];

        // Setting layer for sorting order. Probably not very important for our future real 3D view
        pieceGO.GetComponent<Renderer>().sortingLayerName = PIECE_LAYER;


        int col = piecePut.SquarePos.Col;
        int row = piecePut.SquarePos.Row;

        if (board[col, row] != null)
        {
            Destroy(board[col, row]);
        }

        board[col, row] = pieceGO;
    }

    // When notified that piece has been moved
    //
    public void OnPieceMoved(object sender, object args)
    {
        Move move = (Move)args;

        GameObject pieceGOToMove = board[move.FromPos.Col, move.FromPos.Row];

        // Stop if from and to pos are the same
        if (pieceGOToMove == board[move.ToPos.Col, move.ToPos.Row])
        {
            return;
        }

        // Remove GameObject at to position if it exists
        if (board[move.ToPos.Col, move.ToPos.Row] != null)
        {
            Destroy(board[move.ToPos.Col, move.ToPos.Row]);
        }

        // Add reference to the piece at the to pos in the board array
        board[move.ToPos.Col, move.ToPos.Row] = pieceGOToMove;
        // Remove reference from the from pos in the array
        board[move.FromPos.Col, move.FromPos.Row] = null;

        // Update position of the sprite
        pieceGOToMove.transform.position = GetVectPosOfSquarePos(move.ToPos);
    }

    // When notified that piece has been removed
    //
    public void OnPieceRemoved(object sender, object args)
    {
        SquarePos squarePos = (SquarePos)args;

        // Destroy piece GameObject
        Destroy(board[squarePos.Col, squarePos.Row]);
        // Remove piece reference from board array
        board[squarePos.Col, squarePos.Row] = null;
    }

    // Takes any Vector3 position
    // Returns square position on the board, SquarePos consisting of col and row, corresponding
    // to the Vector3 x and y positions if there is one, otherwise null
    private SquarePos GetSquarePosOfVectPos(Vector3 pos)
    {
        float squareWidth = boardGO.GetComponent<Renderer>().bounds.size.x / (board.GetUpperBound(0) + 1);
        float squareHeight = boardGO.GetComponent<Renderer>().bounds.size.y / (board.GetUpperBound(1) + 1);

        // (0, 0) lower left currently
        int col = (int)Mathf.Floor(2 * (pos.x - boardPos.x + (board.GetUpperBound(0) + 1)/2 * squareWidth));
        int row = (int)Mathf.Floor(2 * (pos.y - boardPos.y + (board.GetUpperBound(1) + 1)/2 * squareHeight));
        
        if (col >= 0 && row >= 0 && col <= board.GetUpperBound(0) && row <= board.GetUpperBound(1))
        {
            return new SquarePos(col, row);
        }
        else
        {
            return null;
        }
    }


    // Takes any Square position
    // Returns a Vector3 postion corresponding
    // to the Square positions
    private Vector3 GetVectPosOfSquarePos(SquarePos pos)
    {
        float squareWidth = boardGO.GetComponent<Renderer>().bounds.size.x / (board.GetUpperBound(0) + 1);
        float squareHeight = boardGO.GetComponent<Renderer>().bounds.size.y / (board.GetUpperBound(1) + 1);

        float x = ((float)pos.Col) * squareWidth + boardPos.x - 3.5f * squareWidth;
        float y = ((float)pos.Row) * squareHeight + boardPos.y - 3.5f * squareHeight;

        return new Vector3(x, y, 0);
    }
}