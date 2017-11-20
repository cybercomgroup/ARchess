using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static Game gameInstance;
    public static Game GameInstance { get; private set; }

    public const string PIECE_PUT = "Piece put";
    public const string PIECE_MOVED = "Piece moved";
    public const string PIECE_REMOVED = "Piece removed";
    private SquarePos fromPos;


    void OnEnable()
    {
        // For testing
        // this.AddObserver(OnLMBClick, InteractionController.LMB_CLICK);

        this.AddObserver(OnSquareRMBClicked, ViewController.SQUARE_RMB_CLICKED);
        this.AddObserver(OnSquareLMBClicked, ViewController.SQUARE_LMB_CLICKED);
        this.AddObserver(OnOutsideLMBClicked, ViewController.OUTSIDE_LMB_CLICKED);

    }

    void OnDisable()
    {
        // For testing
        // this.RemoveObserver(OnLMBClick, InteractionController.LMB_CLICK);

        this.AddObserver(OnSquareRMBClicked, ViewController.SQUARE_RMB_CLICKED);
        this.AddObserver(OnSquareLMBClicked, ViewController.SQUARE_LMB_CLICKED);
        this.AddObserver(OnOutsideLMBClicked, ViewController.OUTSIDE_LMB_CLICKED);
    }


    // Use this for initialization
    void Start()
    {
        // NOTE: Temp solution for creating and adding a game set
        Game.AddTestSet();

        // NOTE: Temp solution - this info needs to come from the menu
        string gameName = "test";
        
        // Creates the singular game instance
        GameInstance = Game.StartGame(gameName);

        fromPos = null;



        // Test
        // doAPutPieceTest();
        //doATestWithAPiece();
    }

    public void OnLMBClick(object sender, object args)
    {
        Debug.Log("Hallo! GameController has been notified that there has been a LMB click at position " + (Vector3)args);
    }



    // Update is called once per frame
    void Update()
    {

    }


    public void OnSquareRMBClicked(object sender, object args)
    {
        SquarePos squarePos = (SquarePos)args;

        string pieceType = GameInstance.PutSomePieceAt(squarePos.Col, squarePos.Row);

        this.PostNotification(PIECE_PUT, new PiecePutMSG(pieceType, squarePos) );
    }

    public void OnSquareLMBClicked(object sender, object args)
    {
        SquarePos squarePos = (SquarePos)args;

        if (fromPos != null)
        {
            GameInstance.Board[squarePos.Col, squarePos.Row] = GameInstance.Board[fromPos.Col, fromPos.Row];
            GameInstance.Board[fromPos.Col, fromPos.Row] = null;
            this.PostNotification(PIECE_MOVED, new Move(fromPos, squarePos));
            fromPos = null;
        }
        else if(GameInstance.Board[squarePos.Col, squarePos.Row] != null)
        {
            fromPos = squarePos;
        }
    }

    public void OnOutsideLMBClicked(object sender, object args)
    {
        if (fromPos != null)
        {
            GameInstance.Board[fromPos.Col, fromPos.Row] = null;
            this.PostNotification(PIECE_REMOVED, fromPos);
            fromPos = null;
        }
    }



    // Simple test - disabled
    public void doATestWithAPiece()
    {
        GameInstance.PutSomePieceAt(2, 2);

        // disabled
        // GameInstance.MovePiece(2, 2, 3, 3);

        Debug.Log("Piece at 3, 3:" + GameInstance.Board[3, 3]);

        Debug.Log("Piece at 2, 2:" + GameInstance.Board[2, 2]);
    }

    // Simple create and put piece test
    public void doAPutPieceTest()
    {
        int col = 2;
        int row = 2;
        string pieceType = GameInstance.PutSomePieceAt(col, row);

        this.PostNotification(PIECE_PUT, new PiecePutMSG(pieceType, new SquarePos(col, row)) );
    }
}
