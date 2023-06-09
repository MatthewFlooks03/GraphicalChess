using System.Collections.Generic;
using UnityEngine;

public class Rook : IPiece
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


    public Rook(Board board, char color)
    {
        this.color = color;
        this.value = 1;
        this.type = 'r';
        this.board = board;
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
        List<Coord2> moves = new List<Coord2>();

        int x = position.x;
        int y = position.y;

        do
        {
            y++;
            moves.Add(new Coord2(x, y));
        }
        while (y <= 7 && Main.gameBoard.boardArray[x, y] == null);

        y = position.y;

        do
        {
            y--;
            moves.Add(new Coord2(x, y));
        }
        while (y >= 0 && Main.gameBoard.boardArray[x, y] == null);

        y = position.y;

        do
        {
            x++;
            moves.Add(new Coord2(x, y));
        }
        while (x <= 7 && Main.gameBoard.boardArray[x, y] == null);

        x = position.x;

        do
        {
            x--;
            moves.Add(new Coord2(x, y));
        }
        while (x >= 0 && Main.gameBoard.boardArray[x, y] == null);

        return board.CleanMoves(moves, position);
    }

    public List<Coord2> GetAttackMoves(Coord2 position)
    {
        return GetLegalMoves(position);
    }
}
