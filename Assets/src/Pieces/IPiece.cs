using System.Collections.Generic;
using UnityEngine;

public interface IPiece
{
    char color { get; }
    int value { get; }
    char type { get; }
    GameObject gameObject { get; set; }
    bool canMove { get; set;}

    /// <summary>
    /// Checks a given move is legal.
    /// </summary>
    /// <param name="oldPosition"></param>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    public bool IsLegal(Coord2 oldPosition, Coord2 newPosition);

    /// <summary>
    /// Checks if move is a valid attack.
    /// </summary>
    /// <param name="oldPosition"></param>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    public bool IsAttackable(Coord2 oldPosition, Coord2 newPosition);

    /// <summary>
    /// Gets all legal moves of the piece at the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public List<Coord2> GetLegalMoves(Coord2 position);

    /// <summary>
    /// Gets all squares that the piece at the given position attacks.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public List<Coord2> GetAttackMoves(Coord2 position);
}
