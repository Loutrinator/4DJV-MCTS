using UnityEngine;

public enum Thrust {NONE = 0, HIGH = 1, MEDIUM = 2, LOW = 3}

[RequireComponent(typeof(Character))]
public class PlayerController : AController
{
    [SerializeField] private bool _isPlayerOne = true;
   public override void ExecuteActions()
    {
        if( _isDead || GameManager.IsPaused) return;
        if (_isPlayerOne)
        {
            _direction = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump")){_isJumping = true;}
            if (Input.GetButtonDown("Crouch")){_isCrouching = true;}
            else if (Input.GetButtonUp("Crouch")){_isCrouching = false;}
            if (Input.GetButtonDown("Fire1")){_thrust = Thrust.HIGH;}
            else if (Input.GetButtonDown("Fire2")){_thrust = Thrust.MEDIUM;}
            else if (Input.GetButtonDown("Fire3")){_thrust = Thrust.LOW;}
    //        if (Input.GetButtonDown("Mouse X")){_throwSword = true;}
        }
        else
        {
            _direction = Input.GetAxis("Horizontal2");
            if (Input.GetButtonDown("Jump2")){_isJumping = true;}
            if (Input.GetButtonDown("Crouch2")){_isCrouching = true;}
            else if (Input.GetButtonUp("Crouch2")){_isCrouching = false;}
            if (Input.GetButtonDown("Fire12")){_thrust = Thrust.HIGH;}
            else if (Input.GetButtonDown("Fire22")){_thrust = Thrust.MEDIUM;}
            else if (Input.GetButtonDown("Fire32")){_thrust = Thrust.LOW;}
         //   if (Input.GetButtonDown("Mouse X2")){_throwSword = true;}
        }
    }

   
}