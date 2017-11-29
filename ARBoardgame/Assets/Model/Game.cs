using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Game
{


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
        string gameName = "chess";
        BoardType testBoard = new BoardType(8, 8);

        List<string> testPieces = new List<string>();

        testPieces.Add("pawn");
        testPieces.Add("rook");

        GameSet test = new GameSet(testBoard, testPieces);

        AddGameSet(gameName, test);
    }

    /* Consider if this shouldn't be in GameController instead 
    public void MovePiece(int fromCol, int fromRow, int toCol, int toRow)
    {
        // Check arguments if necessary
        if(CheckMPArguments(fromCol, fromRow, toCol, toRow))
        {
            Board[toCol, toRow] = Board[fromCol, fromRow];
            Board[fromCol, fromRow] = null;

            this.PostNotification(PIECE_MOVED, new MoveMSG(fromCol, fromRow, toCol, toRow));
        }
    }
    */

    // Test version
    // Only execute move when there is a piece in the from position
    private bool CheckMPArguments(int fromCol, int fromRow, int toCol, int toRow)
    {
        return Board[fromCol, fromRow] != null;
    }

    // Test method
    public string PutSomePieceAt(int col, int row)
    {
        string pieceType = GameSets[Name].PieceTypes[0];
        Board[col, row] = pieceType;

        return pieceType;
    }

    /// <summary>
    /// Loads all GameSets in the Resources directory.
    /// </summary>
    public static void LoadGameSets()
    {
        List<string> gameSetNames = GetListOfGameSets();

        foreach(string gameSetName in gameSetNames)
        {
            BoardType boardType = ImportBoardType(gameSetName);
            List<string> pieceTypes = ImportPieceTypes(gameSetName);

            // Do not add a game set for which creating a boardType has filed (missing boardType.json)
            if (boardType != null)
            {
                GameSet gameSet = new GameSet(boardType, pieceTypes);
            
                AddGameSet(gameSetName, gameSet);
            }
        }
    }

    /// <summary>
    /// Get all GameSets in the Resources directory.
    /// </summary>
    public static List<string> GetListOfGameSets()
    {
        List<string> gamesList = new List<string>();

        foreach (string gamesSubDir in (Directory.GetDirectories(Application.dataPath + "/Resources/Games/")))
        {
            gamesList.Add(Path.GetFileName(gamesSubDir));
        }

        return gamesList;
    }

    /// <summary>
    /// Import board type data for a specific game set from json specification file.
    /// </summary>
    public static BoardType ImportBoardType(string gameSetName)
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/Games/" + gameSetName + "/boardType.json");

        BoardType boardType;

        if (File.Exists(filePath))
        {
            boardType = JsonUtility.FromJson<BoardType>(File.ReadAllText(filePath));
            return boardType;
        }
        else
        {
            // Consider what to do if expected game set resources do not exist.
            // Currently just refraining from adding the set.
            Debug.LogError("No boardType.json exists for game set " + gameSetName);
        }

        return null;
    }

    /// <summary>
    /// Import the pieces for the specified game set.
    /// </summary>
    public static List<string> ImportPieceTypes(string gameSetName)
    {
        string fileExt = "*.png";

        string piecesPath = Path.Combine(Application.dataPath, "Resources/Games/" + gameSetName + "/Pieces/");

        List<string> pieceTypes = new List<string>();

        foreach (string file in Directory.GetFiles(piecesPath, fileExt))
        {
            pieceTypes.Add(Path.GetFileNameWithoutExtension(file));
        }

        return pieceTypes;
    }
}
