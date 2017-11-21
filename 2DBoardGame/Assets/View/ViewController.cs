using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    // NOTE: temp solution for handling graphical resources - consider how to import
    public Sprite chessBoardSprite;
    public Sprite pawn;

    private int numPiecesCreated;

    public const string SQUARE_RMB_CLICKED = "Square RMB clicked";
    public const string SQUARE_LMB_CLICKED = "Square LMB clicked";
    public const string OUTSIDE_LMB_CLICKED = "Outside board LMB clicked";

    private Dictionary<string, Sprite> gamePieceTypeToSpriteMap;
    //private Dictionary<string, GameObject> gamePieceToGameObjectMap;
    private GameObject[,] board;


    private GameObject boardGO;
    // Not really required, just for quicker access
    private Vector3 boardPos;


    // Layers
    private const string PIECE_LAYER = "PieceLayer";
    private const string BOARD_LAYER = "BoardLayer";



    // NOTE: Should each class handle its notification subcsriptions? Handle subscriptions in GameController maybe?

    // Handling subscribing and unsubscribing to the notifications in OnEnable and OnDisable is a "better practice" method to prevent
    // mem leakage, subscribed no longer existing objects
    void OnEnable()
    {
        this.AddObserver(OnBoardPostioned, InteractionController.BOARD_POSITIONED);
        this.AddObserver(OnLMBClick, InteractionController.LMB_CLICK);
        this.AddObserver(OnRMBClick, InteractionController.RMB_CLICK);
        this.AddObserver(OnPiecePut, GameController.PIECE_PUT);
        this.AddObserver(OnPieceMoved, GameController.PIECE_MOVED);
        this.AddObserver(OnPieceRemoved, GameController.PIECE_REMOVED);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnBoardPostioned, InteractionController.BOARD_POSITIONED);
        this.RemoveObserver(OnLMBClick, InteractionController.LMB_CLICK);
        this.RemoveObserver(OnRMBClick, InteractionController.RMB_CLICK);
        this.RemoveObserver(OnPiecePut, GameController.PIECE_PUT);
        this.RemoveObserver(OnPieceMoved, GameController.PIECE_MOVED);
        this.RemoveObserver(OnPieceRemoved, GameController.PIECE_REMOVED);
    }

    // Use this for initialization
    void Start()
    {
        // Move to and implement in OnGameCreated
        board = new GameObject[8, 8];
        //

        gamePieceTypeToSpriteMap = new Dictionary<string, Sprite>();
        // gamePieceToGameObjectMap = new Dictionary<string, GameObject>();

        gamePieceTypeToSpriteMap["pawn"] = pawn;
    }

    // Not implemented
    // Should contain info about which game that has been started: game name, list of piece names and board size
    public void OnGameCreated(object sender, object args)
    {
    }

    public void OnBoardPostioned(object sender, object args)
    {
        Vector3 pos = (Vector3)args;
        // The currently used solution for determining cursor postion results in a z axis pos on the "camera cutoff plane"
        // Set z axis pos to 0 instead
        pos.z = 0;
        // Not really required, just for quicker access
        boardPos = pos;

        // Debug.Log("Board postion selected: " + pos);

        boardGO = new GameObject();
        boardGO.name = "Board";

        boardGO.transform.position = pos;
        boardGO.transform.SetParent(this.transform, true);        
        boardGO.AddComponent<SpriteRenderer>().sprite = chessBoardSprite;
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
        PiecePutMSG piecePutMSG = (PiecePutMSG)args;
        string pieceType = piecePutMSG.PieceType;

        Vector3 pos = GetVectPosOfSquarePos( piecePutMSG.SquarePos );

        GameObject pieceGO = new GameObject();
        pieceGO.name = "Piece_" + numPiecesCreated;
        numPiecesCreated++;

        pieceGO.transform.position = pos;
        pieceGO.transform.SetParent(this.transform, true);
        pieceGO.AddComponent<SpriteRenderer>().sprite =  gamePieceTypeToSpriteMap[pieceType];

        // Setting layer for sorting order. Probably not very important for our future real 3D view
        pieceGO.GetComponent<Renderer>().sortingLayerName = PIECE_LAYER;


        int col = piecePutMSG.SquarePos.Col;
        int row = piecePutMSG.SquarePos.Row;

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
        float squareWidth = boardGO.GetComponent<Renderer>().bounds.size.x / 8;
        float squareHeight = boardGO.GetComponent<Renderer>().bounds.size.y / 8;



        // temp
        // NOTE: Update source of size info. Should be info in class members, info supplied by game created notification
        // (0, 0) lower left currently
        int col = (int)Mathf.Floor(2 * (pos.x - boardPos.x + 4 * squareWidth));
        int row = (int)Mathf.Floor(2 * (pos.y - boardPos.y + 4 * squareHeight));
        
        if (col >= 0 && row >= 0 && col <= 7 && row <= 7)
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
        float squareWidth = boardGO.GetComponent<Renderer>().bounds.size.x / 8;
        float squareHeight = boardGO.GetComponent<Renderer>().bounds.size.y / 8;

        float x = ((float)pos.Col) * squareWidth + boardPos.x - 3.5f * squareWidth;
        float y = ((float)pos.Row) * squareHeight + boardPos.y - 3.5f * squareHeight;

        return new Vector3(x, y, 0);
    }
}