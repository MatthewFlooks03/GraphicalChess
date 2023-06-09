using System.Collections.Generic;
using UnityEngine.PlayerLoop;

public class Move
{
    public Coord2 initial;
    public Coord2 final;

    public Move(Coord2 initial, Coord2 final)
    {
        this.initial = initial;
        this.final = final;
    }
}