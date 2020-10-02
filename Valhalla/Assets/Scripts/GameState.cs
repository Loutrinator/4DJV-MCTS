using UnityEngine;

public struct GameState
{
    public Character[] players;
    public int advantage;
    public bool gameWon;
    public BoxCollider2D[] levelColliders;
}