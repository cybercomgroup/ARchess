using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Message class for notifying the view relevant information about the started game
public class GameStarted
{
    public string Name { get; private set; }
    public GameSet GameSet { get; private set; }

    private GameStarted() { }

    public GameStarted(string name, GameSet gameSet)
    {
        Name = name;
        GameSet = gameSet;
    }
}