using System;
using UnityEngine;

public class PrintButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Main.gameBoard.Print();
    }
}
