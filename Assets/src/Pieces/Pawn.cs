using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Pawn : IPiece
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


    public Pawn(char color)
    {
        this._color = color;
        this._value = 1;
        this.type = 'p';
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
        };
    }
    public List<Vector2> GetLegalMoves(Vector2 position)
    {
        List<Vector2> legalMoves = new List<Vector2>();

        if (color == 'l')
        {
            if (position.y == 1)
            {
                if (Main.gameBoard.board[(int)position.x, (int)position.y + 1] == null)
                {
                    legalMoves.Add(new Vector2(position.x, position.y + 1));
                    if (Main.gameBoard.board[(int)position.x, (int)position.y + 2] == null)
                    {
                        legalMoves.Add(new Vector2(position.x, position.y + 2));
                    }
                }
            }
            else
            {
                if (Main.gameBoard.board[(int)position.x, (int)position.y + 1] == null)
                {
                    legalMoves.Add(new Vector2(position.x, position.y + 1));
                }
            }
            if (position.x + 1 <= 7 && position.y + 1 <= 7)
            {
                if (Main.gameBoard.board[(int)position.x + 1, (int)position.y + 1] != null)
                {
                    legalMoves.Add(new Vector2(position.x + 1, position.y + 1));
                }
            }
            if (position.x - 1 >= 0 && position.y + 1 <= 7)
            {
                if (Main.gameBoard.board[(int)position.x - 1, (int)position.y + 1] != null)
                {
                    legalMoves.Add(new Vector2(position.x - 1, position.y + 1));
                }
            }
        }
        else
        {
            if (position.y == 6)
            {
                if (Main.gameBoard.board[(int)position.x, (int)position.y - 1] == null)
                {
                    legalMoves.Add(new Vector2(position.x, position.y - 1));
                    if (Main.gameBoard.board[(int)position.x, (int)position.y - 2] == null)
                    {
                        legalMoves.Add(new Vector2(position.x, position.y - 2));
                    }
                }
            }
            else
            {
                if (Main.gameBoard.board[(int)position.x, (int)position.y - 1] == null)
                {
                    legalMoves.Add(new Vector2(position.x, position.y - 1));
                }
            }

            if (position.x + 1 <= 7 && position.y - 1 >= 0)
            {
                if (Main.gameBoard.board[(int)position.x + 1, (int)position.y - 1] != null)
                {
                    legalMoves.Add(new Vector2(position.x + 1, position.y - 1));
                }
            }
            if (position.x - 1 >= 0 && position.y - 1 >= 0)
                {
                if (Main.gameBoard.board[(int)position.x - 1, (int)position.y - 1] != null)
                {
                    legalMoves.Add(new Vector2(position.x - 1, position.y - 1));
                }
            }
        }

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

    public bool canPromote(Vector2 position)
    {
        if (color == 'l')
        {
            if (position.y == 7)
            {
                return true;
            }
        }
        else
        {
            if (position.y == 0)
            {
                return true;
            }
        }
        return false;
    }
}           
