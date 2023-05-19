using System.Collections.Generic;
using UnityEngine;

public class Knight : IPiece
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

    public GameObject gameObject { get; set; }

    private IPiece[,] boardArray;


    public Knight(Board board, char color)
    {
        this.color = color;
        this.value = 1;
        this.type = 'n';
        this.boardArray = board.boardArray;
    }

    public bool IsLegal(Coord2 oldPosition, Coord2 newPosition)
    {
        return GetLegalMoves(oldPosition).Contains(newPosition);
    }
    public bool IsAttackable(Coord2 oldPosition, Coord2 newPosition)
    {
        return (GetAttackMoves(oldPosition).Contains(newPosition));
    }



    public List<Coord2> GetLegalMoves(Coord2 position)
    {
        List<Coord2> legalMoves = new List<Coord2>();

        legalMoves.Add(position + new Coord2(2, 1));
        legalMoves.Add(position + new Coord2(2, -1));
        legalMoves.Add(position + new Coord2(-2, 1));
        legalMoves.Add(position + new Coord2(-2, -1));
        legalMoves.Add(position + new Coord2(1, 2));
        legalMoves.Add(position + new Coord2(1, -2));
        legalMoves.Add(position + new Coord2(-1, 2));
        legalMoves.Add(position + new Coord2(-1, -2));

        // Remove illegal moves
        legalMoves.RemoveAll(move => (
                !move.IsOnBoard() || 
                (boardArray[move.x, move.y] != null) && (boardArray[move.x, move.y].color == this.color)
            ));

        return legalMoves;
    }

    public List<Coord2> GetAttackMoves(Coord2 position)
    {
        return GetLegalMoves(position);
    }
}
