using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject mainLayout;
   
    public void StartGame()
    {
        
    }
    
    public void ShowGameConfigurationScreen()
    {
        mainLayout.SetActive(false);
    }

    public void BackToMainScreen()
    {
        mainLayout.SetActive(true);
    }
    
    public void QuitGame()
    {
    
    }
}
