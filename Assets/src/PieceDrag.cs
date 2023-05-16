using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class PieceDrag : MonoBehaviour
{
    public bool canMove = true;
    private Vector2 initialPos;
    private Vector2 finalPos;
    private Vector2 difference;
    private void OnMouseDown()
    {
        initialPos = transform.position;
        int initialX = (int)Math.Round(initialPos.x / Main.boardScale);
        int initialY = (int)Math.Round(initialPos.y / Main.boardScale);
        if (canMove)
        {
            difference = (Vector2)GameObject.Find("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        }
        List<Vector2> legalMoves = Main.gameBoard.board[initialX, initialY].GetLegalMoves(new Vector2(initialX, initialY));
        foreach (var move in legalMoves)
        {
            Graphics.AddBoardDot(move);
        }
    }

    private void OnMouseDrag()
    {
        if (canMove)
        {
            transform.position = (Vector2)GameObject.Find("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - difference;
        }
    }

    private void OnMouseUp()
    {

        finalPos = transform.position;

        float initialX = (float)Math.Round(initialPos.x / Main.boardScale);
        float initialY = (float)Math.Round(initialPos.y / Main.boardScale);
        float finalX = (float)Math.Round(transform.position.x / Main.boardScale);
        float finalY = (float)Math.Round(transform.position.y / Main.boardScale);

        if (Main.gameBoard.MovePiece(new Vector2(initialX, initialY), new Vector2(finalX, finalY)))
        {
            transform.position = new Vector2(finalX * Main.boardScale, finalY * Main.boardScale);
        }
        else
        {
            transform.position = initialPos;
        }

        Graphics.ClearBoardDot(); 
    }
}
