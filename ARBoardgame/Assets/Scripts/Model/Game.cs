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

            // Do not add a game set for which creating a boardType has failed (missing boardType.json)
            if (boardType != null)
            {
                GameSet gameSet = new GameSet(boardType, pieceTypes);
            
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
    private static List<string> ImportPieceTypes(string gameSetName)
    {
        return ImportStringListFromJSON("Games/" + gameSetName + "/pieceTypes");
    }

    /// <summary>
    /// Import the pieces for the specified game set.
    /// </summary>
    private static List<string> ImportStringListFromJSON(string jsonFilePath)
    {
        TextAsset file = Resources.Load(jsonFilePath) as TextAsset;
        StringsJSON stringJSON = JsonUtility.FromJson<StringsJSON>(file.ToString());

        return new List<string>(stringJSON.strings);
    }
}
