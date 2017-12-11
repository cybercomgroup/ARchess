
using System;
using UnityEngine;

[Serializable]
public class PieceInfo
{
    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
        private set
        {
            name = value;
        }
    }
    
    [SerializeField]
    private int rot = 0;
    public int Rot
    {
        get
        {
            return rot;
        }
        private set
        {
            rot = value;
        }
    }
}