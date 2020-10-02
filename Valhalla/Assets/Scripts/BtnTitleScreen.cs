using Button = UnityEngine.UI.Button;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnTitleScreen : MonoBehaviour
{
   private void Start()
   {
      GetComponent<Button>().onClick.AddListener( () => SceneManager.LoadScene(0) );
   }
}
