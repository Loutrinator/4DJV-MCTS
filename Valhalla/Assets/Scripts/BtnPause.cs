using UnityEngine;
using Button = UnityEngine.UI.Button;

public class BtnPause : MonoBehaviour
{
    public GameObject PauseGameMenu = null;
    void Start()
    {
        PauseGameMenu.SetActive(false);
        GetComponent<Button>().onClick.AddListener( () =>
        {
            GameManager.Instance.PauseGame();
            PauseGameMenu.SetActive(GameManager.IsPaused);
        });
    }
}
