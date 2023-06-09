using System.Collections.Generic;
using UnityEngine;

public class Pawn : IPiece
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
    private Board board;


    public Pawn(Board board, char color)
    {
        this.color = color;
        this.value = 1;
        this.type = 'p';
        this.board = board;
        this.boardArray = board.boardArray;
    }

    public bool IsLegal(Coord2 oldPosition, Coord2 newPosition)
    {
        return (GetLegalMoves(oldPosition).Contains(newPosition));
    }
    public bool IsAttackable(Coord2 oldPosition, Coord2 newPosition)
    {
        return (GetAttackMoves(oldPosition).Contains(newPosition));
    }
    public List<Coord2> GetLegalMoves(Coord2 position)
    {
        List<Coord2> moves = new List<Coord2>();
        if (color == 'l')
        {
            moves.Add(new Coord2(position.x, position.y + 1));

            if (position.y == 1)
            {
                moves.Add(new Coord2(position.x, position.y + 2));
            }
        }
        else
        {
            moves.Add(new Coord2(position.x, position.y - 1));

            if (position.y == 6)
            {
                moves.Add(new Coord2(position.x, position.y - 2));
            }

        }

        List <Coord2> legalMoves = board.CleanMoves(moves, position);
        legalMoves.RemoveAll(newPos => (
              boardArray[newPos.x, newPos.y] != null //Remove moves that collide 
        ));

        return legalMoves;

    }

    public List<Coord2> GetAttackMoves(Coord2 position)
    {
        List<Coord2> attackMoves = new List<Coord2>();

        if (color == 'l')
        {
            attackMoves.Add(new Coord2(position.x + 1, position.y + 1));
            attackMoves.Add(new Coord2(position.x - 1, position.y + 1));
        }
        else
        {
            attackMoves.Add(new Coord2(position.x + 1, position.y - 1));
            attackMoves.Add(new Coord2(position.x - 1, position.y - 1));
        }

        return board.CleanMoves(attackMoves, position);
    }

    public bool canPromote(Coord2 position)
    {
        return (color == 'l' && position.y == 7) || (color == 'd' && position.y == 0);
    }
}
