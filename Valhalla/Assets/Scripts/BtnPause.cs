using UnityEngine;
using Button = UnityEngine.UI.Button;

public class BtnPause : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener( GameManager.Instance.PauseGame );
    }
}
