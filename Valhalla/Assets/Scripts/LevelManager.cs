using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    public Vector2[] goodSideSpawnPoints; //the spawn points available when you have to respawn in your own part of the map
    public Vector2[] wrongSideSpawnPoints;//the spawn points available when you have to respawn in your opponent's part of the map
    [Tooltip("counting the number of rooms for one side including the middle room.")] public int nbRooms = 5; 
    [SerializeField] private CameraPanningController camera;
    
    void Start()
    {
        SetSpawnPoints();
        if (GameManager.Instance.InitGame(this))
        {
            Debug.Log("Que la partie commence !");
        }
    }

    void SetSpawnPoints()
    {
        GameManager.Instance.spawnPoints[0] = (Vector3)(goodSideSpawnPoints[0] * new Vector2(-1,1));
        GameManager.Instance.spawnPoints[1] = (Vector3)(goodSideSpawnPoints[0] * new Vector2(1,1));
    }

    public bool GetSpawnPosition(out Vector3 position)
    {
        int sens = GameManager.Instance.Direction;
        int camScreen = camera.cameraScreenPosition;
        int spawnId = Mathf.Abs(camScreen);
        if (spawnId >= nbRooms)
        {
            position = Vector3.zero;
            return false;
        }
        
        /*
         *wrong side doit etre = à false si on est dans notre camp et qu'on défends.
         *                       à true si on respawn dans le territoire adverse.
         *
         *si joueur 1 attaque
         * le jeu va de gauche a droite
         * on arrive salle 3 soit la dernière salle
         * sens = -1;
         * camscreen = 3
         * joueur 1 est tué
         * sens = 1;
         * il doit donc respawn en salle 2
         * il doit spawn dans une salle autre que dans son camps [-5,-1] il est donc du mauvais coté
         *
         * detection de bon coté ou pas
         * cas 1 wrongside = true ssi respawnVoulu > 0 && mainPlayer.id == 1
         * cas 2 wrongside = true ssi respawnVoulu < 0 && mainPlayer.id == 2
         *
         * Qu'on peut réduire à
         * 
         * wrongside = true ssi respawnVoulu*sens > 0
         */
        int desiredSpawnScreen = camScreen - sens;
        bool wrongSide = desiredSpawnScreen * sens > 0;
        
        Vector3 spawnPoint = Vector3.zero;
        if (wrongSide)
        {
            spawnPoint = wrongSideSpawnPoints[spawnId-sens];
            
        }
        else
        {
            spawnPoint = goodSideSpawnPoints[spawnId-sens];
        }
        spawnPoint.x = camScreen < 0 ? -spawnPoint.x : spawnPoint.x;
        position = spawnPoint;
        return true;
    }
    
    
    public void OnDrawGizmos()
    {
        float radius = 0.2f;
        Gizmos.color = Color.green;
        foreach (var spawn in wrongSideSpawnPoints)
        {
            Vector3 left = (Vector3) spawn;
            Vector3 right = (Vector3) spawn;
            left.x *= -1;
            Gizmos.DrawSphere(left, radius);
            Gizmos.DrawSphere(right, radius);
        }
        Gizmos.color = Color.cyan;
        foreach (var spawn in goodSideSpawnPoints)
        {
            Vector3 left = (Vector3) spawn;
            Vector3 right = (Vector3) spawn;
            left.x *= -1;
            Gizmos.DrawSphere(left, radius);
            Gizmos.DrawSphere(right, radius);
        }
    }
}
