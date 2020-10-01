using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            Character character = other.gameObject.GetComponent<Character>();
            Debug.Log("Haha, ending your life isn't the proper way to win this game XD");
            StartCoroutine(character.Respawn());
        }
    }
}
