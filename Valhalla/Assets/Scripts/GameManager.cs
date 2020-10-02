using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerType{none,player,random,mcts}

public class GameManager : MonoBehaviour
{
    #region fields

    [Header("Events")] 
    [SerializeField] private UnityEvent UpdateLoop;
    [SerializeField] private UnityEvent FixedUpdateLoop;
    [SerializeField] private UnityEvent AILoop;
    [SerializeField, Range(1,30)] private float AILoopFrequency = 5f;
    public static GameManager Instance => _instance;
    private static GameManager _instance;
    [SerializeField] private Character[] characterPrefabs = new Character[2];
    
    [SerializeField] private int _nbPlayers = 2; //In case one day the game can handle more than 2 _gameData.players
    public int NbPlayers {get { return _nbPlayers; }}

    private GameState _gameState;
    
    public PlayerType[] playerTypes;
    public Character MainPlayer {get{return _gameState.advantage == 1 ? _gameState.players[0] : _gameState.advantage == 2 ? _gameState.players[1] : null;}} // the one who get the direction

    public Vector3[] spawnPoints;
    
    public int Direction{get{return _gameState.advantage == 1 ? -1 : _gameState.advantage == 2 ? 1 : 0;}}
    public static bool IsPaused = true; // TRUE if the game is paused, FALSE otherwise 

    public AudioSource lowBeep;
    public AudioSource highBeep;

    public LevelManager currentLevel;
    public float dampening;
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
        
        _gameState.players = new Character[_nbPlayers];// On set le nombre de joueurs
        playerTypes = new PlayerType[_nbPlayers];
        spawnPoints = new Vector3[_nbPlayers];
        
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
        //print("AI LOOP");
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
            
            _gameState.players[i] = Instantiate(characterPrefabs[i], position, Quaternion.identity);
            _gameState.players[i].tag = "player" + (i + 1);
            _gameState.players[i].name = "player" + (i + 1);
            _gameState.players[i].data.id = (i + 1);
            if(i == 1) _gameState.players[i].Flip();
            AController controller;
            switch (playerTypes[i])
            {
                case PlayerType.player:
                    PlayerController playerController = _gameState.players[i].gameObject.AddComponent<PlayerController>();
                    controller = playerController;
                    UpdateLoop.AddListener(controller.ExecuteActions);
                    FixedUpdateLoop.AddListener(controller.CustomFixedUpdate);
                    controller.id = i + 1;
                    break;
                case PlayerType.random:
                    controller = _gameState.players[i].gameObject.AddComponent<RandomController>();
                    AILoop.AddListener(controller.ExecuteActions);
                    FixedUpdateLoop.AddListener(controller.CustomFixedUpdate);
                    controller.id = i + 1;
                    break;
                case PlayerType.mcts:
                    controller = _gameState.players[i].gameObject.AddComponent<MCTSController>();
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
