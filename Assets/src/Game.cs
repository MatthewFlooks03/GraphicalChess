using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public char turn;
    public string castling;
    public int halfMoveClock;
    public int fullMoveCounter;
    
    public Game()
    {
        turn = 'l';
        castling = "KQkq";
        halfMoveClock = 0;
        fullMoveCounter = 1;
    }
}
