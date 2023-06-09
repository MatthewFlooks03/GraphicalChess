using System.Collections.Generic;
using UnityEngine;

public class Bishop : IPiece
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

    public Bishop(Board board, char color)
    {
        this.color = color;
        this.value = 1;
        this.type = 'b';
        this.board = board;
        this.boardArray = board.boardArray;
    }

    /// <summary>
    /// Returns true if the move is legal, false otherwise.
    /// </summary>
    /// <param name="oldPosition"></param>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    public bool IsLegal(Coord2 oldPosition, Coord2 newPosition)
    {
        return GetLegalMoves(oldPosition).Contains(newPosition);
    }
    public bool IsAttackable(Coord2 oldPosition, Coord2 newPosition)
    {
        return (GetAttackMoves(oldPosition).Contains(newPosition));
    }

    /// <summary>
    /// Gets legal moves of the bishop at the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public List<Coord2> GetLegalMoves(Coord2 position)
    {
        List<Coord2> moves = new List<Coord2>();


        int y = position.y;
        int x = position.x;

        do
        {
            y++;
            x++;
            moves.Add(new Coord2(x, y));
        }
        while (y <= 7 && x <= 7 && boardArray[x, y] == null);

        y = position.y;
        x = position.x;

        do
        {
            y--;
            x++;
            moves.Add(new Coord2(x, y));
        }
        while (y >= 0 && x <= 7 && boardArray[x, y] == null);

        y = position.y;
        x = position.x;

        do
        {
            y++;
            x--;
            moves.Add(new Coord2(x, y));
        }
        while (y <= 7 && x >= 0 && boardArray[x, y] == null);

        y = position.y;
        x = position.x;

        do
        {
            y--;
            x--;
            moves.Add(new Coord2(x, y));
        }
        while (y >= 0 && x >= 0 && boardArray[x, y] == null);

        return board.CleanMoves(moves, position);
    }

    public List<Coord2> GetAttackMoves(Coord2 position)
    {
        return GetLegalMoves(position);
    }
}
