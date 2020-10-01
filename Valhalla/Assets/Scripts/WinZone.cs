using System;
using UnityEngine;

enum Zone
{
    PLAYER1 = 0, PLAYER2 = 1
}
public class WinZone : MonoBehaviour
{
    [Tooltip("Select the player who have to reach this zone to win")]
    [SerializeField] private Zone playerAttached;
    private void Start()
    {
        GameManager.Instance.WinZoneBounds[(int) playerAttached] = GetComponent<BoxCollider2D>().bounds;
    }

  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.Equals( GameManager.Instance.players[(int) playerAttached].gameObject))
        {
            GameManager.Instance.Win( GameManager.Instance.players[(int) playerAttached]);
        }
    }
}
