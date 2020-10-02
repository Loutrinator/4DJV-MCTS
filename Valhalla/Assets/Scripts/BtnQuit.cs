using Button = UnityEngine.UI.Button;
using UnityEngine;

public class BtnQuit : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener( () =>
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        });
    }
}
