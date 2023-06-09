using System;
using System.Linq;
using UnityEngine;

public class PrintButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Main.gameBoard.Print();
        //Debug.Log(Main.gameBoard.GetLegalMoves(Main.game.turn).Count);
        /*
        foreach (Move move in Main.gameBoard.GetLegalMoves(Main.game.turn))
        {
            Debug.Log("[" + move.initial.x + "," + move.initial.y + "] -> [" + move.final.x + "," + move.final.y + "]");
        }
        */
    }
}
