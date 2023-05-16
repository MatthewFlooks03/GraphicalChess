using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using System.Collections.Generic;

public class EnPassantPiece : IPiece
{
    private char _color;
    public char color
    {
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
            if (color == 'l')
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


    public EnPassantPiece()
    {
        this._color = 'd';
        this._value = 1;
        this.type = 'e';
    }

    public bool isLegal(Vector2 oldPosition, Vector2 newPosition)
    {
        return false;
    }

    public List<Vector2> GetLegalMoves(Vector2 position)
    {
        return new List<Vector2>(new Vector2[0]);
    }
}
