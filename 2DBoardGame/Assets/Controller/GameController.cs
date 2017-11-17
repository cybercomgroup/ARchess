using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static Game gameInstance;
    public static Game GameInstance { get; private set; }

    void OnEnable()
    {
        // For testing
        this.AddObserver(OnPieceMoved, Game.PIECE_MOVED );
        this.AddObserver(OnClick, InteractionController.CLICK);
    }

    void OnDisable()
    {
        // For testing
        this.RemoveObserver(OnPieceMoved, Game.PIECE_MOVED);
        this.RemoveObserver(OnClick, InteractionController.CLICK);
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





        // Test
        //doATestWithAPiece();
    }

    public void OnClick(object sender, object args)
    {
        Debug.Log("Hallo!! The value is " + (int)args);
    }



    // Update is called once per frame
    void Update()
    {

    }


    public void OnPieceMoved(object sender, object args)
    {
        Move move = (Move)args;

        Debug.Log("OnPieceMoved reached\n");
    }

    // Simple test
    public void doATestWithAPiece()
    {
        GameInstance.PlaceSomePieceAt(2, 2);

        GameInstance.MovePiece(2, 2, 3, 3);

        Debug.Log("Piece at 3, 3:" + GameInstance.Board[3, 3]);

        Debug.Log("Piece at 2, 2:" + GameInstance.Board[2, 2]);
    }
}
