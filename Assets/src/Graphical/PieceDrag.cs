using System;
using UnityEngine;

public class PieceDrag : MonoBehaviour
{
    private Vector2 initialPos;
    private Vector2 finalPos;
    private Vector2 difference;

    private void OnMouseDown()
    {
        initialPos = transform.position;
        Coord2 boardPlace = new Coord2((int)Math.Round(initialPos.x / Main.boardScale), (int)Math.Round(initialPos.y / Main.boardScale));

        if (Main.gameBoard.boardArray[boardPlace.x, boardPlace.y].canMove == true)
        {
            difference = (Vector2)GameObject.Find("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        }
    }

    private void OnMouseDrag()
    {
        Coord2 boardPlace = new Coord2((int)Math.Round(initialPos.x / Main.boardScale), (int)Math.Round(initialPos.y / Main.boardScale));
        if (Main.gameBoard.boardArray[boardPlace.x, boardPlace.y].canMove == true)
        {
            transform.position = (Vector2)GameObject.Find("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - difference;
        }
    }

    private void OnMouseUp()
    {

        finalPos = transform.position;

        Coord2 initial = new Coord2((int)Math.Round(initialPos.x / Main.boardScale), (int)Math.Round(initialPos.y / Main.boardScale));
        Coord2 final = new Coord2((int)Math.Round(finalPos.x / Main.boardScale), (int)Math.Round(finalPos.y / Main.boardScale));

        Main.gameBoard.MovePiece(initial, final);
        Graphics.DrawPieces();
    }
}
