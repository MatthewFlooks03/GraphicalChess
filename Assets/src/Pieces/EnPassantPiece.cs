using System.Collections.Generic;
using UnityEngine;

public class EnPassantPiece : IPiece
{
    public char color { get; }

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

    public int value { get; }

    public bool canMove { get; set; }

    public GameObject gameObject { get; set; }

    private IPiece[,] boardArray;
    public EnPassantPiece(Board board, char color)
    {
        this.color = color;
        this.value = 0;
        this.type = 'e';
        this.boardArray = board.boardArray;
    }

    public bool IsLegal(Coord2 oldPosition, Coord2 newPosition)
    {
        return false;
    }
    public bool IsAttackable(Coord2 oldPosition, Coord2 newPosition)
    {
        return false;
    }

    public List<Coord2> GetLegalMoves(Coord2 position)
    {
        return new List<Coord2>(new Coord2[0]);
    }

    public List<Coord2> GetAttackMoves(Coord2 position)
    {
        return new List<Coord2>(new Coord2[0]);
    }
}
