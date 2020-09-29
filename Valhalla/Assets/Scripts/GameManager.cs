using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region fields

    [Header("Events")] 
    [SerializeField] private UnityEvent UpdateLoop;
    [SerializeField] private UnityEvent FixedUpdateLoop;

    public static GameManager Instance => _instance;
    private static GameManager _instance;

    private Character _player1;
    private Character _player2;
    private Character _mainPlayer; // the one who get the direction
    private int _direction; // -1 left, 1 right
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

        _player1 = GameObject.FindGameObjectWithTag("player1").GetComponent<Character>();
        _player2 = GameObject.FindGameObjectWithTag("player2").GetComponent<Character>();
    }

    public void Start()
    {
        InitGame();
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

    private void InitGame()
    {
        _mainPlayer = (Random.Range(0, 2) > 0) ? _player1 : _player2;
        _direction = (_mainPlayer.Equals(_player1)) ? 1 : -1;
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
    #endregion

    
    
}
