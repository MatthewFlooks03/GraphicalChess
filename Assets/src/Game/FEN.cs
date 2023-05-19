using System;
using UnityEngine;

public class FEN
{
    /// <summary>
    /// Loads a FEN string into the board array
    /// </summary>
    /// <param name="FEN"></param>
    /// <param name="board"></param>
    public static void LoadFEN(string FEN, Board board)
    {
        /*
         * Example FEN
         * "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
         */

        //Split FEN into sections
        string[] FENSections = FEN.Split(' ');

        if (FENSections.Length != 6)
        {
            Debug.Log("Invalid FEN");
            return;
        }

        //Load Board
        int x = 0;
        int y = 7;
        foreach (char c in FENSections[0])
        {

            switch (c)
            {
                case '/':
                    y--;
                    x = 0;
                    break;

                case char n when (char.GetNumericValue(c) >= 1 && char.GetNumericValue(n) <= 8):
                    x += n;
                    break;

                default:
                    board.boardArray[x, y] = CreatePiece(c);
                    x++;
                    break;
            }
        }

        //Set Turn
        char[] turn = FENSections[1].ToCharArray();
        Main.game.turn = turn[0];

        //Set Castling
        Main.game.castling = FENSections[2];

        //Set En Passant
        if (FENSections[3] != "-")
        {
            int x2 = char.ToUpper(FENSections[3][0]) - 64;
            int y2 = (int)char.GetNumericValue(FENSections[3][1]);

            Main.gameBoard.SetEnPassant(new Coord2(x2, y2));
        }

        //Set Halfmove Clock
        Main.game.halfMoveClock = Int32.Parse(FENSections[4]);

        //Set Fullmove Number
        Main.game.fullMoveCounter = Int32.Parse(FENSections[5]);
    }

    /// <summary>
    /// Generates FEN string from board array
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public static string GenerateFEN(Board board)
    {
        string[] FENSection = new string[6];

        //Board 
        for (int y = 7; y >= 0; y--)
        {
            for (int x = 0; x < 8; x++)
            {
                int skipped = 0;
                if (board.boardArray[x, y] != null)
                {
                    FENSection[0] += (char)skipped;
                    skipped = 0;

                    FENSection[0] += board.boardArray[x, y].type;
                }
                else
                {
                    skipped++;
                }
            }
            FENSection[0] += '/';
        }


        //Turn
        FENSection[1] = Main.game.turn.ToString();

        //Castling
        FENSection[2] = Main.game.castling;

        //EnPassant
        if (board.GetEnPassant().x == -1)
        {
            FENSection[3] = "-";
        }
        FENSection[3] += char.ToLower((char)(board.GetEnPassant().x + 64));
        FENSection[3] += (char)(board.GetEnPassant().y);

        //Halfmove Clock
        FENSection[4] = Main.game.halfMoveClock.ToString();

        //Fullmove Number
        FENSection[5] = Main.game.fullMoveCounter.ToString();

        return string.Join(" ", FENSection);
    }

    private static IPiece CreatePiece(char c)
    {
        char color = Board.Color(c);

        IPiece piece;

        c = char.ToLower(c);

        switch (c)
        {
            case 'p':
                piece = new Pawn(Main.gameBoard, color);
                break;

            case 'r':
                piece = new Rook(Main.gameBoard, color);
                break;

            case 'n':
                piece = new Knight(Main.gameBoard, color);
                break;

            case 'b':
                piece = new Bishop(Main.gameBoard, color);
                break;

            case 'k':
                piece = new King(Main.gameBoard, color);
                break;

            case 'q':
                piece = new Queen(Main.gameBoard, color);
                break;

            default:
                piece = null;
                break;
        }
        return piece;
    }
}
