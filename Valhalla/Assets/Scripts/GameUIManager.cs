using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{

    public Image leftArrow;
    public Image rightArrow;

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.Direction)
        {
            case -1:
                leftArrow.gameObject.SetActive(false);
                rightArrow.gameObject.SetActive(true);
                break;
            case 1:
                leftArrow.gameObject.SetActive(true);
                rightArrow.gameObject.SetActive(false);
                break;
            default:
                leftArrow.gameObject.SetActive(false);
                rightArrow.gameObject.SetActive(false);
                break;
        }
    }
}
