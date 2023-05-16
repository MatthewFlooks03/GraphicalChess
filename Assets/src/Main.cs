using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static int boardScale = 32;
    public static Color darkColor = Color.red;
    public static Color lightColor = Color.white;
    public static Color dotColor = Color.black;//new Color(169, 169, 169, 100);

    public static Board gameBoard;
    public static Game game;

    void Start()
    {
        //Create Game
        game = new Game();

        //Create Gameboard
        gameBoard = new Board();

        //Load
        FEN.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Graphics.InitialiseCamera();
        Graphics.DrawBoard();
        Graphics.DrawPieces();
    }
}
