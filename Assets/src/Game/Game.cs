using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Game
{
    public char turn;
    public string castling;
    public int halfMoveClock;
    public int fullMoveCounter;
    public List<char[,]> boards;
    private Board gameBoard;

    public Game(Board gameBoard)
    {
        turn = 'l';
        castling = "KQkq";
        halfMoveClock = 0;
        fullMoveCounter = 1;
        this.gameBoard = gameBoard;
        boards = new List<char[,]>();
    }

    public void NextTurn()
    {
        boards.Add(gameBoard.GetPosition());
        if(turn == 'd')
        {
            fullMoveCounter++;
        }

        turn = turn == 'l' ? 'd' : 'l';

        LockPieces(turn);
        if (CheckGameEnd())
        {
            LockPieces(turn == 'l' ? 'd' : 'l');
            Debug.Log("Game Over");
        }
    }

    public bool CheckGameEnd()
    {
        bool gameEnd = false;
        if(gameBoard.GetLegalMoves(turn).Count == 0)
        {
            if (gameBoard.IsCheck(turn))
            {
                gameEnd = true;
                Debug.Log("Checkmate");

            }
            else
            {
                gameEnd = true;
                Debug.Log("Stalemate");
            }
        } 

        // 50 Move rule
        if(halfMoveClock >= 100)
        {
            gameEnd = true;
            Debug.Log("Draw by 50 move rule"); //need to reset counter on castle rights removed
        }

        // 3 fold repetition

        List<string> boardStrings = new List<string>();

        boards.ForEach(board => boardStrings.Add(new string(board.Cast<char>().ToArray())));

        bool tfold = (from x in boardStrings
                 group x by x into g
                 let count = g.Count()
                 orderby count descending
                 select count).Any(c => c > 2);

        if (tfold)
        {
            gameEnd = true;
            Debug.Log("Draw by 3 Fold Repetition");
        }

        return gameEnd;
    }

    public void LockPieces(char turn)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (gameBoard.boardArray[x, y] != null)
                {
                    if (gameBoard.boardArray[x, y].color != turn)
                    {
                        gameBoard.boardArray[x, y].canMove = false;
                    }
                    else
                    {
                        gameBoard.boardArray[x, y].canMove = true;
                    }
                }
            }
        }
    }
}
