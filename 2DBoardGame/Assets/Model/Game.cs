using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public const string PIECE_MOVED = "Piece moved";

    public string Name { get; private set; }
    private static Dictionary<string, GameSet> gameSets = new Dictionary<string, GameSet>();
    public static Dictionary<string, GameSet> GameSets
    {
        get
        {
            return gameSets;
        }
    }
    // null == no piece at position
    public string[,] Board { get; private set; }

    private Game() {}

    public static void AddGameSet(string setName, GameSet gameSet)
    {
        gameSets[setName] = gameSet;
    }

    public static Game StartGame(string gameName)
    {
        Game game = new Game();

        game.Name = gameName;
        BoardType boardType = gameSets[gameName].BoardType;

        game.Board = new string[boardType.NumCols, boardType.NumRows];

        return game;
    }


    // NOTE: Temp solution for creating and adding a game set
    public static void AddTestSet()
    {
        string gameName = "test";
        BoardType testBoard = new BoardType(8, 8);

        List<string> testPieces = new List<string>();

        testPieces.Add("pawn");
        testPieces.Add("rook");

        GameSet test = new GameSet(testBoard, testPieces);

        AddGameSet(gameName, test);
    }

    public void MovePiece(int fromCol, int fromRow, int toCol, int toRow)
    {
        // Check arguments if necessary
        if(CheckMPArguments(fromCol, fromRow, toCol, toRow))
        {
            Board[toCol, toRow] = Board[fromCol, fromRow];

            Board[fromCol, fromRow] = null;
            // TODO: Notify
        }
    }

    // Test version
    // Only execute move when there is a piece in the from position
    private bool CheckMPArguments(int fromCol, int fromRow, int toCol, int toRow)
    {
        return Board[fromCol, fromRow] != null;
    }

    // Test method
    public void PlaceSomePieceAt(int col, int row)
    {
        Board[col, row] = GameSets[Name].PieceTypes[0];
    }
}