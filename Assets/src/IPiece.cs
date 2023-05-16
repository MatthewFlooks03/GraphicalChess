using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public interface IPiece
{
    char color { get; }
    int value { get; }
    char type { get; }
    GameObject gameObject { get; set; }

    public bool isLegal(Vector2 oldPosition, Vector2 newPosition); 
    public List<Vector2> GetLegalMoves(Vector2 position);
}
