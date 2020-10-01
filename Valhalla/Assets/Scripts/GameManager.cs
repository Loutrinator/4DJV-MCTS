using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerType{none,player,random,mcts}

public class GameManager : MonoBehaviour
{
    #region fields

    [Header("Simulation parameters")]
    public float suicidY = -50f;
    [Header("Events")] 
    [SerializeField] private UnityEvent UpdateLoop;
    [SerializeField] private UnityEvent FixedUpdateLoop;
    [SerializeField] private UnityEvent AILoop;
    [SerializeField, Range(1,30)] private float AILoopFrequency = 5f;
    public static GameManager Instance => _instance;
    private static GameManager _instance;
    [SerializeField] private Character characterPrefab;
    
    [SerializeField] private int _nbPlayers = 2; //In case one day the game can handle more than 2 _gameData.players
    public int NbPlayers {get { return _nbPlayers; }}

    private GameState _gameState;
    
    public PlayerType[] playerTypes;
    [HideInInspector] public Character[] players;
    public Character MainPlayer {get{return _gameState.advantage == 1 ? players[0] : _gameState.advantage == 2 ? players[1] : null;}} // the one who get the direction
    public Vector3[] spawnPoints;
    
    public int Direction{get{return _gameState.advantage == 1 ? -1 : _gameState.advantage == 2 ? 1 : 0;}}
    public static bool IsPaused = true; // TRUE if the game is paused, FALSE otherwise 

    public AudioSource lowBeep;
    public AudioSource highBeep;

    public LevelManager currentLevel;

    [HideInInspector] public Bounds[] WinZoneBounds;
    [HideInInspector] public Vector3 characterColliderSize;
    
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
        
        players = new Character[_nbPlayers];// set number of player
        _gameState.players = new PlayerData[_nbPlayers]; // same for player data inside game state
        playerTypes = new PlayerType[_nbPlayers];
        spawnPoints = new Vector3[_nbPlayers];
        characterColliderSize = players[0].GetComponent<BoxCollider2D>().size;

        /*for (int i = 0; i < _gameData.players.Length; i++)
        {
            _gameData.players[i] = GameObject.FindGameObjectWithTag("player" + (i+1)).GetComponent<Character>();
        }*/
        InvokeRepeating("CallAILoop",0f,1/AILoopFrequency);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
        if(IsPaused) return;
        UpdateLoop?.Invoke();
    }

    private void FixedUpdate()
    {
        if(IsPaused) return;
        FixedUpdateLoop?.Invoke();
    }

    private void CallAILoop()
    {
        if(IsPaused) return;
        print("AI LOOP");
        AILoop?.Invoke();
    }

    #endregion

    #region other function

    public bool InitGame(LevelManager level) //returns true if the game properly initialised
    {
        currentLevel = level;
        
        if (_gameState.players.Length != playerTypes.Length) return false;
        
        for(int i = 0; i < _gameState.players.Length; ++i)
        {
            var position = (spawnPoints != null && (spawnPoints.Length == _nbPlayers)) ? spawnPoints[i] :Vector3.zero;
            
            players[i] = Instantiate(characterPrefab, position, Quaternion.identity);
            players[i].tag = "player" + (i + 1);
            players[i].name = "player" + (i + 1);
            _gameState.players[i].id = (i + 1);
            AController controller;
            switch (playerTypes[i])
            {
                case PlayerType.player:
                    PlayerController playerController = players[i].gameObject.AddComponent<PlayerController>();
                    controller = playerController;
                    UpdateLoop.AddListener(controller.ExecuteActions);
                    FixedUpdateLoop.AddListener(controller.CustomFixedUpdate);
                    playerController.isPlayerOne = i == 0;
                    controller.id = i + 1;
                    break;
                case PlayerType.random:
                    controller = players[i].gameObject.AddComponent<RandomController>();
                    AILoop.AddListener(controller.ExecuteActions);
                    FixedUpdateLoop.AddListener(controller.CustomFixedUpdate);
                    controller.id = i + 1;
                    break;
                case PlayerType.mcts:
                    controller = players[i].gameObject.AddComponent<MCTSController>();
                    AILoop.AddListener(controller.ExecuteActions);
                    FixedUpdateLoop.AddListener(controller.CustomFixedUpdate);
                    controller.id = i + 1;
                    break;
                case PlayerType.none:
                    Debug.LogError("Error : Attempting to initialise player " + (i+1) + " as with no PlayerType.");
                    return false;
            }

        }
        
        IsPaused = true;
        
        StartCoroutine(GameStart());
        return true;
        //le jeu commence sans avoir d'avantage
    }

    IEnumerator GameStart()
    {
        float delay = 0.7f;
        Debug.Log("GameStart");
        lowBeep.Play();
        yield return new WaitForSeconds(delay);
        lowBeep.Play();
        yield return new WaitForSeconds(delay);
        lowBeep.Play();
        yield return new WaitForSeconds(delay);
        highBeep.Play();
        IsPaused = false;
    }
    
    public void PauseGame()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0 : 1;
    }
    public void PlayerDied(int id)
    {
        if (id == 1)
        {
            _gameState.advantage = 2;
        }else if (id == 2)
        {
            _gameState.advantage = 1;
        }
    }

    public void Win(Character winner)
    {
        Debug.Log(winner + " win !!");
        PauseGame();
        // display win screen or cutscene
    }

    public GameState GetCurrentGameState()
    {
        return _gameState;
    }
    #endregion

    
    
}
