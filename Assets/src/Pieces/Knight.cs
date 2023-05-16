using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;

public class Knight : IPiece
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


    public Knight(char color)
    {
        this._color = color;
        this._value = 1;
        this.type = 'n';
    }

    public bool isLegal(Vector2 oldPosition, Vector2 newPosition)
    {
        if (GetLegalMoves(oldPosition).Contains(newPosition))
        {
            return true;
        } else
        {
            return false;
        }
    }

    public List<Vector2> GetLegalMoves(Vector2 position)
    {
        List<Vector2> legalMoves = new List<Vector2>();

        legalMoves.Add(position + new Vector2(2, 1));
        legalMoves.Add(position + new Vector2(2, -1));
        legalMoves.Add(position + new Vector2(-2, 1));
        legalMoves.Add(position + new Vector2(-2, -1));
        legalMoves.Add(position + new Vector2(1, 2));
        legalMoves.Add(position + new Vector2(1, -2));
        legalMoves.Add(position + new Vector2(-1, 2));
        legalMoves.Add(position + new Vector2(-1, -2));


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
