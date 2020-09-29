using System.Diagnostics.Tracing;
using UnityEngine;

public enum Thrust {HIGH, MEDIUM, LOW, NONE}

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    private Character _character;
    private float _direction;
    private bool _isCrouching;
    private bool _isJumping;
    private bool _noSword;
    private bool _throwSword;
    private Thrust _thrust;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _thrust = Thrust.NONE;
    }

    private void Update()
    {
        _direction = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump")){_isJumping = true;}
        if (Input.GetButtonDown("Crouch")){_isCrouching = true;}
        else if (Input.GetButtonUp("Crouch")){_isCrouching = false;}
        if (Input.GetButtonDown("Fire1")){_thrust = Thrust.HIGH;}
        else if (Input.GetButtonDown("Fire2")){_thrust = Thrust.MEDIUM;}
        else if (Input.GetButtonDown("Fire3")){_thrust = Thrust.LOW;}
        if (Input.GetButtonDown("Mouse X")){_throwSword = true;}
    }

    private void FixedUpdate()
    {
        _character.Move(_direction,_isCrouching,_isJumping);
        _isJumping = false;
        if (_throwSword)
        {
            _character.ThrowSword();
            _noSword = true;
        }
        if(_noSword)  return;
        _character.Thrusting(_thrust);
        _thrust = Thrust.NONE;
    }
}