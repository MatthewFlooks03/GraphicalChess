using UnityEngine;

public class Main : MonoBehaviour
{
    public static int boardScale = 32;
    public static Color darkColor = Color.red;
    public static Color lightColor = Color.white;

    public static Board gameBoard;
    public static Game game;

    void Start()
    {
        //Create Gameboard
        gameBoard = new Board();

        //Create Game
        game = new Game();

        //Load
        FEN.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", gameBoard);
        Graphics.InitialiseCamera();
        Graphics.DrawBoard();
        Graphics.DrawPieces();
    }
}
