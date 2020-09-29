using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject mainLayout;
    [SerializeField] private GameObject setupLayout;
    [SerializeField] private Button  VSButton;
    [SerializeField] private int  gameSceneIndex = 1;

    public PlayerType[] playerTypes;

    private void Start()
    {
        playerTypes = new PlayerType[GameManager.Instance.NbPlayers];
        ShowMainScreen();
    }

    private void Update()
    {
        bool readyToPlay = true;
        foreach (var playerType in playerTypes)
        {
            if (playerType == PlayerType.none)
            {
                readyToPlay = false;
                break;
            }
        }

        if (readyToPlay){
            VSButton.interactable = true;
        }
    }
    public void StartGame()
    {
        string log = playerTypes[0].ToString();
        for (int i = 1; i < playerTypes.Length; ++i)
        {
            log += " VS " + playerTypes[i].ToString();
        }

        if (GameManager.Instance.InitGame(playerTypes))
        {
            SceneManager.LoadScene(gameSceneIndex);
        }
    }

    public void ShowGameConfigurationScreen()
    {
        mainLayout.SetActive(false);
        setupLayout.SetActive(true);
    }

    public void ShowMainScreen()
    {
        mainLayout.SetActive(true);
        setupLayout.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetPlayer1Player(){playerTypes[0] = PlayerType.player;}
    public void SetPlayer1MCTS(){playerTypes[0] = PlayerType.mcts;}
    public void SetPlayer1Random(){playerTypes[0] = PlayerType.random;}
    public void SetPlayer2Player(){playerTypes[1] = PlayerType.player;}
    public void SetPlayer2MCTS(){playerTypes[1] = PlayerType.mcts;}
    public void SetPlayer2Random(){playerTypes[1] = PlayerType.random;}
}
