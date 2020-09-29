using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject mainLayout;
    [SerializeField] private GameObject setupLayout;
    [SerializeField] private Button  VSButton;
    
    public enum PlayerType{player,random,mcts,none}

    public PlayerType player1Type;
    public PlayerType player2Type;

    private void Start()
    {
        player1Type = PlayerType.none;
        player2Type = PlayerType.none;
        ShowMainScreen();
    }

    private void Update()
    {
        if (player1Type != PlayerType.none && player2Type != PlayerType.none)
        {
            VSButton.interactable = true;
        }
    }
    public void StartGame()
    {
        Debug.Log(player1Type + " VS " + player2Type);
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

    public void SetPlayer1Player(){player1Type = PlayerType.player;}
    public void SetPlayer1MCTS(){player1Type = PlayerType.mcts;}
    public void SetPlayer1Random(){player1Type = PlayerType.random;}
    public void SetPlayer2Player(){player2Type = PlayerType.player;}
    public void SetPlayer2MCTS(){player2Type = PlayerType.mcts;}
    public void SetPlayer2Random(){player2Type = PlayerType.random;}
}
