using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Test version
    // Only execute move when there is a piece in the from position
    private bool CheckMPArguments(int fromCol, int fromRow, int toCol, int toRow)
    {
        return Board[fromCol, fromRow] != null;
    }

    // Test method
    public string PutSomePieceAt(int col, int row)
    {
        PieceInfo pieceType = GameSets[Name].PieceTypes[0];
        Board[col, row] = pieceType.Name;

        return pieceType.Name;
    }

    public void PutPieceAt(string pieceType, SquarePos squarePos)
    {
        Board[squarePos.Col, squarePos.Row] = pieceType;
    }

    public bool PieceExistsAt(SquarePos squarePos)
    {
        return Board[squarePos.Col, squarePos.Row] == null ? false : true;
    }

    public string TakePieceAt(SquarePos squarePos)
    {
        string piece = Board[squarePos.Col, squarePos.Row];

        Board[squarePos.Col, squarePos.Row] = null;

        return piece;
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
            List<PieceInfo> pieceTypes = ImportPieceTypes(gameSetName);
            List<DefaultPosition> defaultPositions = ImportDefaultPositions(gameSetName);

            // Do not add a game set for which creating a boardType has failed (missing boardType.json)
            if (boardType != null)
            {
                GameSet gameSet = new GameSet(boardType, pieceTypes, defaultPositions);
            
                AddGameSet(gameSetName, gameSet);
            }
        }
    }


    // Refactor: Combine with ImportPieceTypes to avoid code duplication 
    /// <summary>
    /// Get all GameSets in the Resources directory.
    /// </summary>
    private static List<string> GetListOfGameSets()
    {
        return ImportStringListFromJSON("Games/gameSets");
    }

    /// <summary>
    /// Import board type data for a specific game set from json specification file.
    /// </summary>
    private static BoardType ImportBoardType(string gameSetName)
    {
        TextAsset file = Resources.Load("Games/" + gameSetName + "/boardType") as TextAsset;
        BoardType boardType = JsonUtility.FromJson<BoardType>(file.ToString());

        return boardType;
    }

    /// <summary>
    /// Import the pieces for the specified game set.
    /// </summary>
    private static List<PieceInfo> ImportPieceTypes(string gameSetName)
    {
        TextAsset file = Resources.Load("Games/" + gameSetName + "/pieceTypes") as TextAsset;
        PieceInfo[] infos = JsonHelper.FromJson<PieceInfo>(file.ToString());
        return new List<PieceInfo>(infos);
    }

    private static List<DefaultPosition> ImportDefaultPositions(string gameSetName)
    {
        TextAsset file = Resources.Load("Games/" + gameSetName + "/defaultSetup") as TextAsset;
        if (file != null)
        {
            DefaultPosition[] defaults = JsonHelper.FromJson<DefaultPosition>(file.ToString());
            return new List<DefaultPosition>(defaults);
        }
        return new List<DefaultPosition>();
    }

    /// <summary>
    /// Import the pieces for the specified game set.
    /// </summary>
    private static List<string> ImportStringListFromJSON(string jsonFilePath)
    {
        TextAsset file = Resources.Load(jsonFilePath) as TextAsset;
        string[] strings = JsonHelper.FromJson<string>(file.ToString());

        return new List<string>(strings);
    }
}
