using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameController : MonoBehaviour
{
    //private static Game gameInstance;
    public static Game GameInstance { get; private set; }

    public const string SQUARE_CLICKED = "Square clicked";
    public const string OUTSIDE_CLICKED = "Outside clicked";
    public const string PIECE_SELECTED_IN_MENU = "Piece selected in menu";
    public const string PIECE_PICKED_FROM_MENU = "Piece picked from menu";
    public const string PIECE_PICKED_FROM_BOARD = "Piece picked from board";
    public const string PIECE_PUT = "Piece put";
    public const string HELD_PIECE_REMOVED = "Held piece removed"; 


    public const string PIECE_MOVED = "Piece moved";

    public const string PIECE_REMOVED = "Piece removed";
    public const string GAME_REQUESTED = "gameRequest";
    public const string GAME_CREATED = "Game created";
    public const string GAME_SETS_LOADED = "Game sets loaded";

    // NOTE: Do we need to keep this
    private SquarePos fromPos;

    private string heldPiece;


    void OnEnable()
    {
        //this.AddObserver(OnSquareRMBClicked, ViewController.SQUARE_RMB_CLICKED);
        this.AddObserver(OnSquareClicked, SQUARE_CLICKED);
        this.AddObserver(OnOutsideClicked, OUTSIDE_CLICKED);
        this.AddObserver(OnGameRequested, GAME_REQUESTED);
        this.AddObserver(OnPieceSelectedInMenu, PIECE_SELECTED_IN_MENU);
        this.AddObserver(OnBoardPlaced, ArViewController.BOARD_PLACED);

    }

    void OnDisable()
    {
        //this.RemoveObserver(OnSquareRMBClicked, ViewController.SQUARE_RMB_CLICKED);
        this.RemoveObserver(OnSquareClicked, SQUARE_CLICKED);
        this.RemoveObserver(OnOutsideClicked, OUTSIDE_CLICKED);
        this.RemoveObserver(OnGameRequested, GAME_REQUESTED);
        this.RemoveObserver(OnPieceSelectedInMenu, PIECE_SELECTED_IN_MENU);
        this.RemoveObserver(OnBoardPlaced, ArViewController.BOARD_PLACED);
    }


    // Use this for initialization
    void Start()
    {
        Screen.fullScreen = false;
        
        // Imports game sets from the resources dir
        Game.LoadGameSets();

        this.PostNotification(GAME_SETS_LOADED, new List<string>(Game.GameSets.Keys));
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>  
    ///  Handles the Square RMB clicked event.
    ///  ...
    /// </summary> 
    public void OnSquareRMBClicked(object sender, object args)
    {
        SquarePos squarePos = (SquarePos)args;

        string pieceType = GameInstance.PutSomePieceAt(squarePos.Col, squarePos.Row);

        this.PostNotification(PIECE_PUT, new PiecePut(pieceType, squarePos));
    }

    /// <summary>  
    ///  Handles the Square LMB clicked event.
    ///  ...
    /// </summary> 
    public void OnSquareClicked(object sender, object args)
    {
        SquarePos squarePos = (SquarePos)args;

        if (heldPiece != null)
        {
            GameInstance.PutPieceAt(heldPiece, squarePos);
            int rot = Game.GameSets[GameInstance.Name].PieceTypes.Find(info => info.Name.Equals(heldPiece)).Rot;
            squarePos.Rot = rot;
            this.PostNotification(PIECE_PUT, squarePos);

            heldPiece = null;
        }
        else if (GameInstance.PieceExistsAt(squarePos))
        {

            heldPiece = GameInstance.TakePieceAt(squarePos);

            this.PostNotification(PIECE_PICKED_FROM_BOARD, squarePos);
        }

        /*
        if (fromPos != null)
        {
            if (!fromPos.Equals(squarePos))
            {
                GameInstance.Board[squarePos.Col, squarePos.Row] = GameInstance.Board[fromPos.Col, fromPos.Row];
                GameInstance.Board[fromPos.Col, fromPos.Row] = null;
                this.PostNotification(PIECE_MOVED, new Move(fromPos, squarePos));
                Debug.Log("From pos (col, row): " + "(" + fromPos.Col + ", " + fromPos.Row + ")  " + "To pos (col, row): " + "(" + squarePos.Col + ", " + squarePos.Row + ")");
            }
            fromPos = null;
        }
        else if (GameInstance.Board[squarePos.Col, squarePos.Row] != null)
        {
            fromPos = squarePos;
        }
        */
    }

    /// <summary>  
    ///  Handles the Outside clicked event.
    ///  If a piece is Held piece is set to null. That the held piece has been 
    /// </summary>  
    public void OnOutsideClicked(object sender, object args)
    {
        if(heldPiece != null)
        {
            heldPiece = null;

            this.PostNotification(HELD_PIECE_REMOVED);
        }
        /*
        if (fromPos != null)
        {
            GameInstance.Board[fromPos.Col, fromPos.Row] = null;
            this.PostNotification(PIECE_REMOVED, fromPos);
            fromPos = null;
        }
        */
    }

    /// <summary>
    /// Adds the default pieces to the game
    /// </summary>
    public void OnBoardPlaced(object sender, object args)
    {
        foreach (DefaultPosition position in Game.GameSets[GameInstance.Name].DefaultPositions)
        {
            int rot = Game.GameSets[GameInstance.Name].PieceTypes.Find(info => info.Name.Equals(position.Name)).Rot;
            SquarePos squarePos = new SquarePos(position.Col, position.Row, rot);
            this.PostNotification(PIECE_PICKED_FROM_MENU, position.Name);
            GameInstance.PutPieceAt(position.Name, squarePos);
            this.PostNotification(PIECE_PUT, squarePos);
        }
    }
    
    /// <summary>  
    ///  Handles the Game requested event.
    ///  Starts a new game of the given game type and notifies observers that a game has been created.
    /// </summary>  
    public void OnGameRequested(object sender, object args)
    {
        string gameName = (string)args;

        // Creates the singular game instance
        GameInstance = Game.StartGame(gameName);

        fromPos = null;

        this.PostNotification(GAME_CREATED, new GameStarted(gameName, Game.GameSets[gameName]));
    }

    /// <summary>  
    ///  Handles the Piece selected in menu event.
    ///  Sets a piece of the given type as held and notifies this
    /// </summary>  
    public void OnPieceSelectedInMenu(object sender, object args)
    {
        string selectedPiece = (string)args;

        heldPiece = selectedPiece;

        this.PostNotification(PIECE_PICKED_FROM_MENU, heldPiece);
    }
}
