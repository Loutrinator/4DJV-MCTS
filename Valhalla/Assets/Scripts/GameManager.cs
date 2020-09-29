using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum PlayerType{none,player,random,mcts}

public class GameManager : MonoBehaviour
{
    #region fields

    [Header("Events")] 
    [SerializeField] private UnityEvent UpdateLoop;
    [SerializeField] private UnityEvent FixedUpdateLoop;

    public static GameManager Instance => _instance;
    private static GameManager _instance;

    [SerializeField] private Character characterPrefab;
    [SerializeField] private int _nbPlayers = 2; //In case one day the game can handle more than 2 players
    public int NbPlayers {get { return _nbPlayers; }}
    
    private int advantage = 0;
    private Character[] players;
    public Character MainPlayer {get{return advantage == 1 ? players[0] : advantage == 2 ? players[1] : null;}} // the one who get the direction

    public Transform[] spawnPoints = new Transform[2];
    
    public int Direction{get{return advantage == 1 ? -1 : advantage == 2 ? 1 : 0;}}
    public static bool isPaused = false; // false if the game is paused, true otherwise

    #endregion

    #region Overriden functions

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {Destroy(this);}
        
        players = new Character[_nbPlayers];// On set le nombre de joueurs
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = GameObject.FindGameObjectWithTag("player" + (i+1)).GetComponent<Character>();
        }
    }

    public void Start()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
        if(isPaused) return;
        UpdateLoop?.Invoke();
    }

    private void FixedUpdate()
    {
        if(isPaused) return;
        FixedUpdateLoop?.Invoke();
    }

    #endregion

    #region other function

    public bool InitGame(PlayerType[] playerTypes) //returns true if the game properly initialised
    {
        if (players.Length != playerTypes.Length) return false;
        
        for(int i = 0; i < players.Length; ++i)
        {
            players[i] = Instantiate(characterPrefab, spawnPoints[i].position, Quaternion.identity);
            players[i].tag = "player" + (i + 1);
            players[i].name = "player" + (i + 1);
            
            switch (playerTypes[i])
            {
                case PlayerType.player:
                    players[i].gameObject.AddComponent<PlayerController>();
                    break;
                case PlayerType.random:
                    players[i].gameObject.AddComponent<PlayerController>();
                    break;
                case PlayerType.mcts:
                    Debug.LogWarning("Warning : Attempting to initialise player " + (i+1) + " as a MCTS AI but MCTS is not currently implemented.");
                    break;
                case PlayerType.none:
                    Debug.LogError("Error : Attempting to initialise player " + (i+1) + " as with no PlayerType.");
                    return false;
            }
        }

        return true;
        //le jeu commence sans avoir d'avantage
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
    #endregion

    
    
}
