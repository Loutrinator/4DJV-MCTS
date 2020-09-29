using System.Collections;
using UnityEngine;

public class RandomController : AController
{
    private int randomInt;
    public override void ExecuteActions()
    {
        StartCoroutine(ChooseAnAction());
        
    }

    private IEnumerator ChooseAnAction()
    {
        randomInt = Random.Range(0, 4);
        switch (randomInt)
        {
            case 0:
            {
                var r = Random.Range(-1, 2);
                _direction = r;
                break;
            }
            case 1:
            {
                var r = Random.Range(0, 2) != 0;
                _isJumping = r;
                break;
            }
            case 2:
            {
                var r = Random.Range(0, 2) != 0;
                _isCrouching = Random.Range(0, 2) != 0;
                break;
            }
            case 3:
            {
                var r = Random.Range(0, 3);
                _thrust = (Thrust) r;
                break;
            }
            default: yield return new WaitForSeconds(10);break;
        }


    }

}
    

