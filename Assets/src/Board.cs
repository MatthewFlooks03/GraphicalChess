using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;

public class Board
{
    public IPiece[,] board;

    public Board()
    {
        board = new IPiece[8, 8];
    }

    public bool MovePiece(Vector2 oldPosition, Vector2 newPosition)
    /* Moves a piece from oldPosition to newPosition if the move is legal. Returns true if the move was successful, false otherwise. */
    {
        if (newPosition.x > 7 || newPosition.y > 7 || newPosition.x < 0 || newPosition.y < 0)
        {
            return false;
        }

        if (oldPosition == newPosition)
        {
            //When piece not moved
            return false;
        }

        if (CheckCheck(oldPosition, newPosition)) 
        { 
            return false;
        }


        if (board[(int)oldPosition.x, (int)oldPosition.y].isLegal(oldPosition, newPosition))
        {
            if (board[(int)newPosition.x, (int)newPosition.y] != null)
            {
                Capture(newPosition);
            }
            board[(int)newPosition.x, (int)newPosition.y] = board[(int)oldPosition.x, (int)oldPosition.y];
            RemovePiece(oldPosition);

            if (board[(int)newPosition.x, (int)newPosition.y] is Pawn)
            {
                PromotePiece(newPosition, 'q');
            }

            return true;
        }
        else
        {
            Debug.Log("Illegal move");
            return false;
        }
    }

    public void AddPiece(IPiece piece, Vector2 position)
    {
        board[(int)position.x, (int)position.y] = piece;
        Graphics.DrawPiece(position);
    }

    public void RemovePiece(Vector2 position)
    {
        board[(int)position.x, (int)position.y] = null;
    }

    public void PromotePiece(Vector2 position, char promotion)
    {
        Pawn pawn;

        pawn = (Pawn)board[(int)position.x, (int)position.y];

        if (!pawn.canPromote(position))
        {
            return;
        }

        RemovePiece(position);

        switch (promotion)
        {
            case 'q':
                AddPiece(new Queen(pawn.color), position);
                break;
            case 'r':
                AddPiece(new Rook(pawn.color), position);
                break;
            case 'b':
                AddPiece(new Bishop(pawn.color), position);
                break;
            case 'n':
                AddPiece(new Knight(pawn.color), position);
                break;
            default:
                Debug.Log("Invalid promotion");
                break;
        }

        Graphics.DeletePiece(pawn);
    }

    public void Castle(Vector2 king, Vector2 rook)
    {
        if(!(board[(int)king.x, (int)king.y] is King)){
            Debug.Log("Not a king");
            return;
        }
        if(!(board[(int)rook.x, (int)rook.y] is Rook)){
            Debug.Log("Not a rook");
            return;
        }

        int direction = (int)Mathf.Sign(rook.x - king.x);
        int x = (int)king.x;
        int y = (int)king.y;
        while (x != rook.x)
        {
            x += direction;

            if (board[x, y] != null)
            {
                Debug.Log("Pieces in the way");
                return;
            }
        }

        char castleKey;
        
        switch (rook.x)
        {
            case 0:
                castleKey = 'q';
                break;
            case 7:
                castleKey = 'k';
                break;
            default:
                Debug.Log("Not a rook");
                return;
        }

        if(rook.y == 0)
        {
            castleKey = char.ToUpper(castleKey);
        }

        if (Main.game.castling.Contains(castleKey))
        {
            King kingPiece = (King)board[(int)king.x, (int)king.y];
            board[(int)king.x, (int)king.y] = board[(int)rook.x, (int)rook.y];
            board[(int)rook.x, (int)rook.y] = kingPiece;
        }
        else
        {
            Debug.Log("Illegal move");
        }
    }

    public void SetEnPassant(Vector2 position)
    {
        board[(int)position.x, (int)position.y] = new EnPassantPiece();
    }

    public Vector2 GetEnPassant()
    {
        for (int y = 7; y >= 0; y--)
        {
            for (int x = 0; x < 8; x++)
            {
                if (board[x, y] is EnPassantPiece)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return new Vector2(-1, -1);
    }

    public void Capture(Vector2 position)
    {
        Debug.Log("Capture");
        Graphics.DeletePiece(board[(int)position.x, (int)position.y]);
    }

    public static char Color(char c)
    {
        if (char.IsUpper(c))
        {
            return 'l';
        }
        else
        {
            return 'd';
        }
    }

    public void Print() { 
        string boardString = "";
        for (int y = 7; y >= 0; y--)
        {
            for (int x = 0; x < 8; x++)
            {
                if (board[x, y] == null)
                {
                    boardString += "-";
                }
                else
                {
                    boardString += board[x, y].type.ToString();
                }
            }
            boardString += "\n";
        }
        Debug.Log(boardString);
    }

    public HashSet<Vector2> AttackedSquares(IPiece[,] inBoard, char color)
    {
        HashSet<Vector2> attackedSquares = new HashSet<Vector2>();
        for (int y = 7; y >= 0; y--)
        {
            for (int x = 0; x < 8; x++)
            {
                if (inBoard[x, y] != null && inBoard[x, y].color == color)
                {
                    foreach(Vector2 square in inBoard[x, y].GetLegalMoves(new Vector2(x, y)))
                    {
                        attackedSquares.Add(square);
                    }    
                }
            }
        }
        return attackedSquares;
    }

    public bool CheckCheck(Vector2 oldPos, Vector2 newPos)
    {

        IPiece[,] tempBoard = board;

        IPiece oldPiece = board[(int)oldPos.x, (int)oldPos.y];
        IPiece newPiece = board[(int)newPos.x, (int)newPos.y];

        board[(int)newPos.x, (int)newPos.y] = board[(int)oldPos.x, (int)oldPos.y];
        board[(int)oldPos.x, (int)oldPos.y] = null;

        char color = board[(int)newPos.x, (int)newPos.y].color;
        char opColor = (color == 'l') ? 'd' : 'l';

        string kChar = (color == 'l') ? "K" : "k";

        int kingX = (int)Math.Round(GameObject.Find(kChar).transform.position.x / Main.boardScale);
        int kingY = (int)Math.Round(GameObject.Find(kChar).transform.position.y / Main.boardScale);

        Vector2 kingLocation = new Vector2(kingX, kingY);

        bool isCheck = AttackedSquares(board, opColor).Contains(kingLocation);

               
        board[(int)oldPos.x, (int)oldPos.y] = oldPiece;
        board[(int)newPos.x, (int)newPos.y] = newPiece;

        if (isCheck)
        {
            return true;
        }
        return false;
        
        


    }
}

