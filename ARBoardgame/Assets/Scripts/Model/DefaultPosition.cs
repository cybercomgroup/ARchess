
using System;
using UnityEngine;

[Serializable]
public class DefaultPosition
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
    private int row;
    public int Row
    {
        get
        {
            return row;
        }
        private set
        {
            row = value;
        }
    }

    [SerializeField]
    private int col;
    public int Col
    {
        get
        {
            return col;
        }
        private set
        {
            col = value;
        }
    }
    
    public DefaultPosition(string name, int row, int col)
    {
        Name = name;
        Row = row;
        Col = col;
    }
}