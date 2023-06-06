using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Board
{
    public IPiece[,] boardArray;

    public Board()
    {
        boardArray = new IPiece[8, 8];
    }

    /// <summary>
    /// Moves a piece from oldPosition to newPosition if the move is legal. Returns true if the move was successful, false otherwise.
    /// </summary>
    /// <param name="oldPosition"></param>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    public bool MovePiece(Coord2 oldPosition, Coord2 newPosition)
    {
        if (!(newPosition.IsOnBoard()))
        {
            Debug.Log("Not on board: " + newPosition.x + ", " + newPosition.y);
            return false;
        }

        if (oldPosition == newPosition)
        {
            Debug.Log("Not Moved");
            return false;
        }

        if (CheckCheck(oldPosition, newPosition))
        {
            Debug.Log("In check");
            return false;
        }

        if (boardArray[oldPosition.x, oldPosition.y].IsLegal(oldPosition, newPosition) && !(boardArray[oldPosition.x, oldPosition.y] is Pawn))
        {
            // Castling
            bool castled = false;
            if (boardArray[oldPosition.x, oldPosition.y] is King && Math.Abs(oldPosition.x - newPosition.x) == 2)
            {
                char kColor = boardArray[oldPosition.x, oldPosition.y].color;
                char rColor = boardArray[oldPosition.x, oldPosition.y].color;

                if(kColor != rColor)
                {
                    Debug.Log("Different colors");
                    return false;
                }

                char opColor = kColor == 'l' ? 'd' : 'l';
                HashSet<Coord2> attackedSquares = AttackedSquares(opColor);

                for(int i = 0; i < 3; i++)
                {
                    Coord2 square = new Coord2(4 - Math.Sign(oldPosition.x - newPosition.x) * i, oldPosition.y);
                    Debug.Log(square);
                    if(attackedSquares.Contains(square))
                    {
                        Debug.Log("Castling through check");
                        return false;
                    }
                }
                
                if (newPosition.x == 2) // Queen side
                {
                    if (kColor == 'l' && Main.game.castling.Contains('Q'))
                    {
                        boardArray[2, 0] = boardArray[4,0];
                        boardArray[3, 0] = boardArray[0,0];
                        boardArray[4,0] = null;
                        boardArray[0,0] = null;
                        castled = true;
                    }
                    else if (kColor == 'd' && Main.game.castling.Contains('q'))
                    {
                        boardArray[2, 7] = boardArray[4,7];
                        boardArray[3, 7] = boardArray[0,7];
                        boardArray[4,7] = null;
                        boardArray[0,7] = null;
                        castled = true;
                    }
                    else
                    {
                        Debug.Log("Castling not allowed");
                        return false;
                    }
                }
                else if(newPosition.x == 6)
                {
                    if (kColor == 'l' && Main.game.castling.Contains('K'))
                    {
                        Debug.Log("test");
                        boardArray[6, 0] = boardArray[4,0];
                        boardArray[5, 0] = boardArray[7,0];
                        boardArray[4,0] = null;
                        boardArray[7,0] = null;
                        castled = true;
                    }
                    else if (kColor == 'd' && Main.game.castling.Contains('k'))
                    {
                        boardArray[6, 7] = boardArray[4,7];
                        boardArray[5, 7] = boardArray[7,7];
                        boardArray[4,7] = null;
                        boardArray[7,7] = null;
                        castled = true;
                    }
                    else
                    {
                        Debug.Log("Castling not allowed");
                        return false;
                    }
                }
            }

            // Remove Castling ability

            if (boardArray[oldPosition.x, oldPosition.y] is King)
            {
                if (boardArray[oldPosition.x, oldPosition.y].color == 'l')
                {
                    Main.game.castling = Main.game.castling.Replace("K", String.Empty);
                    Main.game.castling = Main.game.castling.Replace("Q", String.Empty);
                }
                else
                {
                    Main.game.castling = Main.game.castling.Replace("k", String.Empty);
                    Main.game.castling = Main.game.castling.Replace("q", String.Empty);
                }
            }
            else if (boardArray[oldPosition.x, oldPosition.y] is Rook)
            {
                if (boardArray[oldPosition.x, oldPosition.y].color == 'l')
                {
                    if (oldPosition.x == 0)
                    {
                        Main.game.castling = Main.game.castling.Replace("Q", String.Empty);
                    }
                    else if (oldPosition.x == 7)
                    {
                       Main.game.castling = Main.game.castling.Replace("k", String.Empty);
                    }
                }
                else
                {
                    if (oldPosition.x == 0)
                    {
                         Main.game.castling = Main.game.castling.Replace("q", String.Empty);
                    }
                    else if (oldPosition.x == 7)
                    {
                         Main.game.castling = Main.game.castling.Replace("k", String.Empty);
                    }
                }
            }


            // Capturing
            if (boardArray[newPosition.x, newPosition.y] != null)
            {
                Capture(newPosition);
            }

            // Remove EnPassant Piece
            Coord2 enPassant = GetEnPassant();
            if (enPassant.x != -1)
            {
                RemovePiece(enPassant);
            }

            if (!castled)
            {
                boardArray[newPosition.x, newPosition.y] = boardArray[oldPosition.x, oldPosition.y];
                RemovePiece(oldPosition);
            }

            return true;

        }
        else if (boardArray[oldPosition.x, oldPosition.y] is Pawn && (boardArray[oldPosition.x, oldPosition.y].IsLegal(oldPosition, newPosition) || boardArray[oldPosition.x, oldPosition.y].IsAttackable(oldPosition,newPosition)))
        {

            bool moved = false;

            if (boardArray[newPosition.x, newPosition.y] == null && boardArray[oldPosition.x, oldPosition.y].IsLegal(oldPosition, newPosition))
            {
                boardArray[newPosition.x, newPosition.y] = boardArray[oldPosition.x, oldPosition.y];
                RemovePiece(oldPosition);
                moved = true;
            }
            else if (boardArray[newPosition.x, newPosition.y] != null && boardArray[oldPosition.x, oldPosition.y].IsAttackable(oldPosition, newPosition))
            {
                Capture(newPosition);
                boardArray[newPosition.x, newPosition.y] = boardArray[oldPosition.x, oldPosition.y];
                RemovePiece(oldPosition);
                moved = true;
            }


            if (moved)
            {
                PromotePiece(newPosition, 'q');

                // Remove EnPassant Piece
                Coord2 enPassant = GetEnPassant();
                if (enPassant.x != -1)
                {
                    RemovePiece(enPassant);
                }

                // EnPassant
                if (newPosition.y - oldPosition.y == 2)
                {
                    Debug.Log("En Passant : " + (newPosition - new Coord2(0, 1)).ToString());
                    SetEnPassant(newPosition - new Coord2(0, 1));
                }
                if (newPosition.y - oldPosition.y == -2)
                {
                    SetEnPassant(newPosition + new Coord2(0, 1));
                }

                return true;
            }
            return false;
        }
        else
        {
            Debug.Log("Illegal move: " + newPosition.x + ", " + newPosition.y);
            return false;
        }
    }

    /// <summary>
    /// Adds a piece to the board at the given position.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="position"></param>
    public void AddPiece(IPiece piece, Coord2 position)
    {
        boardArray[position.x, position.y] = piece;
        Graphics.DrawPiece(position);
    }

    /// <summary>
    /// Removes piece from the board at the given position.
    /// </summary>
    /// <param name="position"></param>
    public void RemovePiece(Coord2 position)
    {
        //TODO: make work with graphics, so it all gets done from Board, not by not reseting the dragged piece
        boardArray[position.x, position.y] = null;
    }
    public void MovePiece(Coord2 position)
    {
        //TODO:
    }

    /// <summary>
    /// Promotes a pawn at the given position to the given piece.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="promotion"></param>
    public void PromotePiece(Coord2 position, char promotion)
    {
        Pawn pawn;

        pawn = (Pawn)boardArray[position.x, position.y];

        if (!pawn.canPromote(position))
        {
            return;
        }

        RemovePiece(position);

        switch (promotion)
        {
            case 'q':
                AddPiece(new Queen(this, pawn.color), position);
                break;
            case 'r':
                AddPiece(new Rook(this, pawn.color), position);
                break;
            case 'b':
                AddPiece(new Bishop(this, pawn.color), position);
                break;
            case 'n':
                AddPiece(new Knight(this, pawn.color), position);
                break;
            default:
                throw new Exception("Invalid promotion");
        }

        Graphics.DeletePiece(pawn);
    }


    /// <summary>
    /// Creates a new people at the given position to allow EnPassant
    /// </summary>
    /// <param name="position"></param>
    public void SetEnPassant(Coord2 position)
    {
        boardArray[position.x, position.y] = new EnPassantPiece(this, 'n');
    }

    /// <summary>
    /// Gets EnPassant piece if it exists for the purpose of FEN Generation, otherwise returns null
    /// </summary>
    /// <returns></returns>
    public Coord2 GetEnPassant()
    {
        for (int y = 7; y >= 0; y--)
        {
            for (int x = 0; x < 8; x++)
            {
                if (boardArray[x, y] is EnPassantPiece)
                {
                    return new Coord2(x, y);
                }
            }
        }
        return new Coord2(-1, -1);
    }

    /// <summary>
    /// Graphically captures a piece at the given position
    /// </summary>
    /// <param name="position"></param>
    public void Capture(Coord2 position)
    {
        Debug.Log("Capture" + "(" + position.x + "," + position.y + ")");
        Graphics.DeletePiece(boardArray[position.x, position.y]);
    }

    /// <summary>
    /// Return the color of the given piece char
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Prints off the current boardArray
    /// </summary>
    public void Print()
    {
        string boardString = "";
        for (int y = 7; y >= 0; y--)
        {
            for (int x = 0; x < 8; x++)
            {
                if (boardArray[x, y] == null)
                {
                    boardString += "-";
                }
                else
                {
                    boardString += boardArray[x, y].type.ToString();
                }
            }
            boardString += "\n";
        }
        Debug.Log(boardString);
    }

    /// <summary>
    /// Gets all attacked squares by the given color
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public HashSet<Coord2> AttackedSquares(char color)
    {
        HashSet<Coord2> attackedSquares = new HashSet<Coord2>();
        for (int y = 7; y >= 0; y--)
        {
            for (int x = 0; x < 8; x++)
            {
                if (boardArray[x, y] != null && boardArray[x, y].color == color)
                {
                    foreach (Coord2 square in boardArray[x, y].GetAttackMoves(new Coord2(x, y)))
                    {
                        attackedSquares.Add(square);
                    }
                }
            }
        }
        return attackedSquares;
    }

    /// <summary>
    /// Checks if the king is currently in check given the current position and potential move, and returns true if so
    /// </summary>
    /// <param name="oldPos"></param>
    /// <param name="newPos"></param>
    /// <returns></returns>
    public bool CheckCheck(Coord2 oldPos, Coord2 newPos)
    {

        IPiece oldPiece = boardArray[oldPos.x, oldPos.y];
        IPiece newPiece = boardArray[newPos.x, newPos.y];

        boardArray[newPos.x, newPos.y] = boardArray[oldPos.x, oldPos.y];
        boardArray[oldPos.x, oldPos.y] = null;

        char color = boardArray[newPos.x, newPos.y].color;

        char opColor = (color == 'l') ? 'd' : 'l';
        string kChar = (color == 'l') ? "K" : "k";

        int kingX = (int)Math.Round(GameObject.Find(kChar).transform.position.x / Main.boardScale);
        int kingY = (int)Math.Round(GameObject.Find(kChar).transform.position.y / Main.boardScale);

        Coord2 kingLocation = new Coord2(kingX, kingY);

        bool isCheck = AttackedSquares(opColor).Contains(kingLocation);

        boardArray[oldPos.x, oldPos.y] = oldPiece;
        boardArray[newPos.x, newPos.y] = newPiece;

        return isCheck;
    }
}

