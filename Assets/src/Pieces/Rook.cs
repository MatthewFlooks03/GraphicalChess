using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Rook : IPiece
{
    private char _color;
    public char color {
        get 
        {
            return _color;
        }
    }

    private char _type;
    public char type
    {
        get
        {
            if(color == 'l')
            {
                return char.ToUpper(_type);
            }
            return _type;
        }
        set
        {
            _type = char.ToLower(value);
        }
    }

    private int _value;
    public int value
    {
        get
        {
            return _value;
        }
    }

    private GameObject _gameObject;
    public GameObject gameObject
    {
        get
        {
            return _gameObject;
        }
        set
        {
            _gameObject = value;
        }
    }


    public Rook(char color)
    {
        this._color = color;
        this._value = 1;
        this.type = 'r';
    }

    public bool isLegal(Vector2 oldPosition, Vector2 newPosition)
    {
        if (GetLegalMoves(oldPosition).Contains(newPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public List<Vector2> GetLegalMoves(Vector2 position)
    {
        List<Vector2> legalMoves = new List<Vector2>();

        int x = (int)position.x;
        int y = (int)position.y;

        do
        {
            y++;
            legalMoves.Add(new Vector2(x,y));
        } 
        while (y <= 7 && Main.gameBoard.board[(int)x, (int)y] == null);

        y = (int)position.y;

        do
        {
            y--;
            legalMoves.Add(new Vector2(x,y));
        } 
        while (y >= 0 && Main.gameBoard.board[(int)x, (int)y] == null);

        y = (int)position.y;

        do
        {
            x++;
            legalMoves.Add(new Vector2(x,y));
        } 
        while (x <= 7 && Main.gameBoard.board[(int)x, (int)y] == null);

        x = (int)position.x;

        do
        {
            x--;
            legalMoves.Add(new Vector2(x,y));
        } 
        while (x >= 0 && Main.gameBoard.board[(int)x, (int)y] == null);

        x = (int)position.x;



        legalMoves.RemoveAll(move => (
                (move.x < 0 || move.x > 7 || move.y < 0 || move.y > 7)
            ||
                (
                    (Main.gameBoard.board[(int)move.x, (int)move.y] != null)
                &&
                    (Main.gameBoard.board[(int)move.x, (int)move.y].color == this.color)
                    )
            ));

        return legalMoves;
    }
}
