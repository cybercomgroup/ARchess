using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //private static Game gameInstance;
    public static Game GameInstance { get; private set; }

    public const string PIECE_PUT = "Piece put";
    public const string PIECE_MOVED = "Piece moved";
    public const string PIECE_REMOVED = "Piece removed";
    public const string GAME_REQUESTED = "gameRequest";
    public const string GAME_CREATED = "Game created";
    private SquarePos fromPos;


    void OnEnable()
    {
        this.AddObserver(OnSquareRMBClicked, ViewController.SQUARE_RMB_CLICKED);
        this.AddObserver(OnSquareLMBClicked, ViewController.SQUARE_LMB_CLICKED);
        this.AddObserver(OnOutsideLMBClicked, ViewController.OUTSIDE_LMB_CLICKED);
        this.AddObserver(OnGameRequested, GAME_REQUESTED);

    }

    void OnDisable()
    {
        this.RemoveObserver(OnSquareRMBClicked, ViewController.SQUARE_RMB_CLICKED);
        this.RemoveObserver(OnSquareLMBClicked, ViewController.SQUARE_LMB_CLICKED);
        this.RemoveObserver(OnOutsideLMBClicked, ViewController.OUTSIDE_LMB_CLICKED);
        this.RemoveObserver(OnGameRequested, GAME_REQUESTED);
    }


    // Use this for initialization
    void Start()
    {
        // NOTE: Temp solution for creating and adding a game set
        Game.AddTestSet();

        // NOTE: Temp solution - this need to come as a notification from the menu
        // --> this.PostNotification(OnGameSelected, "Game name");
        // OnGameSelected(this, "test");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method commenting convention in C# :
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
    public void OnSquareLMBClicked(object sender, object args)
    {
        SquarePos squarePos = (SquarePos)args;

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
    }

    /// <summary>  
    ///  Handles the Outside LMB clicked event.
    ///  If a piece has been clicked previously that piece is removed, otherwise nothing happens.
    /// </summary>  
    public void OnOutsideLMBClicked(object sender, object args)
    {
        if (fromPos != null)
        {
            GameInstance.Board[fromPos.Col, fromPos.Row] = null;
            this.PostNotification(PIECE_REMOVED, fromPos);
            fromPos = null;
        }
    }

    /// <summary>  
    ///  Handles the Game requested event.
    ///  ...
    /// </summary>  
    public void OnGameRequested(object sender, object args)
    {
        string gameName = (string)args;

        // NOTE: temp - remove
        Debug.Log("OnGameRequested reached, game name: " + gameName);

        // Creates the singular game instance
        GameInstance = Game.StartGame(gameName);

        fromPos = null;

        this.PostNotification(GAME_CREATED, new GameStarted(gameName, Game.GameSets[gameName]) );
    }
}
