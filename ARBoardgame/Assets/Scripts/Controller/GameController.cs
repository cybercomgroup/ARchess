using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameController : MonoBehaviour
{
    // For handling match with remote player
    public MatchController matchController;

    //private static Game gameInstance;
    public static Game GameInstance { get; private set; }

    public const string PIECE_PUT = "Piece put";
    public const string PIECE_PUT_MULTI = "Piece put multi";
    public const string PIECE_MOVED = "Piece moved";
    public const string PIECE_MOVED_MULTI = "Piece moved multi";
    public const string PIECE_REMOVED = "Piece removed";
    public const string GAME_REQUESTED = "gameRequest";
    public const string GAME_CREATED = "Game created";
    public const string GAME_SETS_LOADED = "Game sets loaded";
    private SquarePos fromPos;


    void OnEnable()
    {
        this.AddObserver(OnSquareRMBClicked, ViewController.SQUARE_RMB_CLICKED);
        this.AddObserver(OnSquareLMBClicked, ViewController.SQUARE_LMB_CLICKED);
        this.AddObserver(OnOutsideLMBClicked, ViewController.OUTSIDE_LMB_CLICKED);
        this.AddObserver(OnGameRequested, GAME_REQUESTED);

        this.AddObserver(OnMatchReady, MatchController.MATCH_READY);
        this.AddObserver(OnDoSomething, PlayerController.DO_SOMETHING);
        this.AddObserver(OnPiecePutMulti, PIECE_PUT_MULTI);
        this.AddObserver(OnPieceMovedMulti, PIECE_MOVED_MULTI);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnSquareRMBClicked, ViewController.SQUARE_RMB_CLICKED);
        this.RemoveObserver(OnSquareLMBClicked, ViewController.SQUARE_LMB_CLICKED);
        this.RemoveObserver(OnOutsideLMBClicked, ViewController.OUTSIDE_LMB_CLICKED);
        this.RemoveObserver(OnGameRequested, GAME_REQUESTED);

        this.RemoveObserver(OnMatchReady, MatchController.MATCH_READY);
        this.RemoveObserver(OnDoSomething, PlayerController.DO_SOMETHING);
        this.RemoveObserver(OnPiecePutMulti, PIECE_PUT_MULTI);
        this.RemoveObserver(OnPieceMovedMulti, PIECE_MOVED_MULTI);
    }


    // Use this for initialization
    void Start()
    {

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

        // NOTE: Which type of piece to put should be selected already or come with the notification. 
        string pieceType = "pawn";

        if (matchController.IsReady)
        {                                    
            string piecePutJSON = JsonUtility.ToJson(new PiecePut(pieceType, squarePos));

            matchController.localPlayer.CmdPiecePut(piecePutJSON);
        }
        else
        {
            GameInstance.PutPieceAt(pieceType, squarePos.Col, squarePos.Row);

            this.PostNotification(PIECE_PUT, new PiecePut(pieceType, squarePos));
        }
    }


    /// <summary>  
    ///  Handles the piece put multi event.
    ///  ...
    /// </summary> 
    public void OnPiecePutMulti(object sender, object args)
    {
        string piecePutJSON = (string)args;

        PiecePut piecePut = JsonUtility.FromJson<PiecePut>(piecePutJSON);

        GameInstance.PutPieceAt(piecePut.PieceType, piecePut.SquarePos.Col, piecePut.SquarePos.Row);

        this.PostNotification(PIECE_PUT, piecePut);
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
                if (matchController.IsReady)
                {
                    string moveJSON = JsonUtility.ToJson(new Move(fromPos, squarePos));

                    matchController.localPlayer.CmdPieceMoved(moveJSON);
                }
                else
                {
                    GameInstance.Board[squarePos.Col, squarePos.Row] = GameInstance.Board[fromPos.Col, fromPos.Row];
                    GameInstance.Board[fromPos.Col, fromPos.Row] = null;
                    this.PostNotification(PIECE_MOVED, new Move(fromPos, squarePos));
                    fromPos = null;
                }



                // Debug.Log("From pos (col, row): " + "(" + fromPos.Col + ", " + fromPos.Row + ")  " + "To pos (col, row): " + "(" + squarePos.Col + ", " + squarePos.Row + ")");
            }
            // fromPos = null;
        }
        else if (GameInstance.Board[squarePos.Col, squarePos.Row] != null)
        {
            fromPos = squarePos;
        }
    }

    /// <summary>  
    ///  Handles the piece moved multi event.
    ///  ...
    /// </summary> 
    public void OnPieceMovedMulti(object sender, object args)
    {
        string pieceMovedJSON = (string)args;

        Debug.Log("OnPieceMovedMulti: " + pieceMovedJSON);

        Move move = JsonUtility.FromJson<Move>(pieceMovedJSON);

        GameInstance.Board[move.ToPos.Col, move.ToPos.Row] = GameInstance.Board[move.FromPos.Col, move.FromPos.Row];
        GameInstance.Board[move.FromPos.Col, move.FromPos.Row] = null;

        this.PostNotification(PIECE_MOVED, move);

        fromPos = null;

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

    public void OnMatchReady(object sender, object args)
    {
        Debug.Log("Match is ready");
    }

    public void OnDoSomething(object sender, object args)
    {
        Debug.Log("I am doing something. I am saying this: " + (string)args);
    }
}
