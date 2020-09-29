using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    public Transform[] spawnPoints;
    
    void Start()
    {
        SetSpawnPoints();
        if (GameManager.Instance.InitGame())
        {
            Debug.Log("Que la partie commence !");
        }
    }

    void SetSpawnPoints()
    {
        if (spawnPoints.Length == GameManager.Instance.spawnPoints.Length)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {

                GameManager.Instance.spawnPoints[i] = spawnPoints[i].position;
            }

        }
    }
}
