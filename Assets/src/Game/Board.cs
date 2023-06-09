using System;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public IPiece[,] boardArray;

    public Board()
    {
        boardArray = new IPiece[8, 8];
    }

    public char[,] GetPosition()
    {
        char[,] array = new char[8, 8];
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (boardArray[x, y] != null)
                {
                    array[x, y] = boardArray[x, y].type;
                }
                else
                {
                    array[x, y] = ' ';
                }
            }
        }

        return array;
    }

    /// <summary>
    /// Moves a piece from oldPosition to newPosition if the move is legal. Returns true if the move was successful, false otherwise.
    /// </summary>
    /// <param name="oldPos"></param>
    /// <param name="newPos"></param>
    /// <returns></returns>
    public bool MovePiece(Coord2 oldPos, Coord2 newPos)
    {
        Game game = Main.game;

        if (boardArray[oldPos.x, oldPos.y].color != game.turn)
        {
            Debug.Log("Not your turn");
            return false;
        }

        if (!(newPos.IsOnBoard()))
        {
            Debug.Log("Not on board: " + newPos.x + ", " + newPos.y);
            return false;
        }

        if (oldPos == newPos)
        {
            Debug.Log("Not Moved");
            return false;
        }

        if (CheckCheck(oldPos, newPos))
        {
            Debug.Log("In Check");
            return false;
        }

        if (boardArray[oldPos.x, oldPos.y].IsLegal(oldPos, newPos) && !(boardArray[oldPos.x, oldPos.y] is Pawn))
        {
            bool resetHMC = false;

            // Castling
            bool castled = false;
            if (boardArray[oldPos.x, oldPos.y] is King && Math.Abs(oldPos.x - newPos.x) == 2)
            {
                char kColor = boardArray[oldPos.x, oldPos.y].color;
                char rColor = boardArray[oldPos.x, oldPos.y].color;

                if (kColor != rColor)
                {
                    Debug.Log("Different colors");
                    return false;
                }

                char opColor = kColor == 'l' ? 'd' : 'l';
                HashSet<Coord2> attackedSquares = AttackedSquares(opColor);

                for (int i = 0; i < 3; i++)
                {
                    Coord2 square = new Coord2(4 - Math.Sign(oldPos.x - newPos.x) * i, oldPos.y);
                    Debug.Log(square);
                    if (attackedSquares.Contains(square))
                    {
                        Debug.Log("Castling through check");
                        return false;
                    }
                }

                if (newPos.x == 2) // Queen side
                {
                    if (kColor == 'l' && Main.game.castling.Contains('Q'))
                    {
                        boardArray[2, 0] = boardArray[4, 0];
                        boardArray[3, 0] = boardArray[0, 0];
                        boardArray[4, 0] = null;
                        boardArray[0, 0] = null;
                        castled = true;
                    }
                    else if (kColor == 'd' && Main.game.castling.Contains('q'))
                    {
                        boardArray[2, 7] = boardArray[4, 7];
                        boardArray[3, 7] = boardArray[0, 7];
                        boardArray[4, 7] = null;
                        boardArray[0, 7] = null;
                        castled = true;
                    }
                    else
                    {
                        Debug.Log("Castling not allowed");
                        return false;
                    }
                }
                else if (newPos.x == 6)
                {
                    if (kColor == 'l' && Main.game.castling.Contains('K'))
                    {
                        Debug.Log("test");
                        boardArray[6, 0] = boardArray[4, 0];
                        boardArray[5, 0] = boardArray[7, 0];
                        boardArray[4, 0] = null;
                        boardArray[7, 0] = null;
                        castled = true;
                    }
                    else if (kColor == 'd' && Main.game.castling.Contains('k'))
                    {
                        boardArray[6, 7] = boardArray[4, 7];
                        boardArray[5, 7] = boardArray[7, 7];
                        boardArray[4, 7] = null;
                        boardArray[7, 7] = null;
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

            if (boardArray[oldPos.x, oldPos.y] is King)
            {
                if (boardArray[oldPos.x, oldPos.y].color == 'l')
                {
                    Main.game.castling = Main.game.castling.Replace("K", String.Empty);
                    Main.game.castling = Main.game.castling.Replace("Q", String.Empty);
                }
                else
                {
                    Main.game.castling = Main.game.castling.Replace("k", String.Empty);
                    Main.game.castling = Main.game.castling.Replace("q", String.Empty);
                }
                resetHMC = true;
            }
            else if (boardArray[oldPos.x, oldPos.y] is Rook)
            {
                if (boardArray[oldPos.x, oldPos.y].color == 'l')
                {
                    if (oldPos.x == 0)
                    {
                        Main.game.castling = Main.game.castling.Replace("Q", String.Empty);
                    }
                    else if (oldPos.x == 7)
                    {
                        Main.game.castling = Main.game.castling.Replace("k", String.Empty);
                    }
                }
                else
                {
                    if (oldPos.x == 0)
                    {
                        Main.game.castling = Main.game.castling.Replace("q", String.Empty);
                    }
                    else if (oldPos.x == 7)
                    {
                        Main.game.castling = Main.game.castling.Replace("k", String.Empty);
                    }
                }
                resetHMC = true;
            }

            // Remove EnPassant Piece
            Coord2 enPassant = GetEnPassant();
            if (enPassant.x != -1)
            {
                RemovePiece(enPassant);
            }
            
            // Capturing
            if (boardArray[newPos.x, newPos.y] != null)
            {
                Debug.Log(boardArray[newPos.x, newPos.y]);
                Debug.Log("Capture1: " + newPos.x + ", " + newPos.y);
                Capture(newPos);
                resetHMC = true;
            }

            if (!castled)
            {
                boardArray[newPos.x, newPos.y] = boardArray[oldPos.x, oldPos.y];
                RemovePiece(oldPos);
            }

            game.halfMoveClock = resetHMC ? 0 : game.halfMoveClock + 1;
            game.NextTurn();
            return true;

        }
        else if (boardArray[oldPos.x, oldPos.y] is Pawn && (boardArray[oldPos.x, oldPos.y].IsLegal(oldPos, newPos) || boardArray[oldPos.x, oldPos.y].IsAttackable(oldPos, newPos)))
        {

            bool moved = false;

            if (boardArray[newPos.x, newPos.y] == null && boardArray[oldPos.x, oldPos.y].IsLegal(oldPos, newPos))
            {
                boardArray[newPos.x, newPos.y] = boardArray[oldPos.x, oldPos.y];
                RemovePiece(oldPos);
                moved = true;
            }
            else if (boardArray[newPos.x, newPos.y] != null && boardArray[oldPos.x, oldPos.y].IsAttackable(oldPos, newPos))
            {
                Capture(newPos);
                boardArray[newPos.x, newPos.y] = boardArray[oldPos.x, oldPos.y];
                RemovePiece(oldPos);
                moved = true;
            }


            if (moved)
            {
                PromotePiece(newPos, 'q');

                // Remove EnPassant Piece
                Coord2 enPassant = GetEnPassant();
                if (enPassant.x != -1)
                {
                    RemovePiece(enPassant);
                }

                // EnPassant
                char color = boardArray[newPos.x, newPos.y].color;
                if (newPos.y - oldPos.y == 2)
                {
                    Debug.Log("En Passant : " + (newPos - new Coord2(0, 1)).ToString());
                    SetEnPassant(newPos - new Coord2(0, 1), color);
                }
                if (newPos.y - oldPos.y == -2)
                {
                    SetEnPassant(newPos + new Coord2(0, 1), color);
                }

                game.halfMoveClock = 0;
                game.NextTurn();
                return true;
            }
            return false;
        }
        else
        {
            Debug.Log("Illegal move: " + newPos.x + ", " + newPos.y);
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
    public void SetEnPassant(Coord2 position, char color)
    {
        boardArray[position.x, position.y] = new EnPassantPiece(this, color);
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
        IPiece piece = boardArray[position.x, position.y];
        // En Passant
        if (piece.type == 'e')
        {
            Debug.Log(piece.color);
            int sign;
            if (piece.color == 'l')
            {
                sign = 1;
            }
            else
            {
                sign = -1;
            }
            Graphics.DeletePiece(boardArray[position.x, position.y + sign]);
            boardArray[position.x, (int)(position.y + sign)] = null;
        }
        Graphics.DeletePiece(piece);

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

        bool isCheck = IsCheck(color);

        boardArray[oldPos.x, oldPos.y] = oldPiece;
        boardArray[newPos.x, newPos.y] = newPiece;

        return isCheck;
    }

    public HashSet<Move> GetLegalMoves(char color)
    {
        HashSet<Move> moveHash = new HashSet<Move>();

        //Get Moves
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                IPiece piece = boardArray[x, y];
                if (piece != null)
                {
                    if (piece.color == color)
                    {
                        Coord2 initial = new Coord2(x, y);
                        List<Coord2> finalMoves = piece.GetLegalMoves(initial);
                        foreach (Coord2 finalMove in finalMoves)
                        {
                            if (!CheckCheck(initial, finalMove))
                            {
                                moveHash.Add(new Move(initial, finalMove));
                            }
                        }
                    }
                }
            }
        }

        return moveHash;
    }

    public bool IsCheck(char color)
    {
        char opColor = (color == 'l') ? 'd' : 'l';
        string kChar = (color == 'l') ? "K" : "k";
        bool isCheck = false;

        int kingX = -1;
        int kingY = -1;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (boardArray[x, y] != null && boardArray[x, y].type.ToString() == kChar)
                {
                    kingX = x;
                    kingY = y;
                }
            }
        }

        if (kingX != -1)
        {
            Coord2 kingLocation = new Coord2(kingX, kingY);

            isCheck = AttackedSquares(opColor).Contains(kingLocation);
        }

        return isCheck;
    }

    public List<Coord2> CleanMoves(List<Coord2> moveList, Coord2 oldPos)
    {
        moveList.RemoveAll(newPos => (
          !newPos.IsOnBoard() || //Remove moves that are off the board
          (boardArray[newPos.x, newPos.y] != null && boardArray[newPos.x, newPos.y].color == boardArray[oldPos.x, oldPos.y].color) //Remove moves that capture your own piece
        ));

        return moveList;
    }
}


