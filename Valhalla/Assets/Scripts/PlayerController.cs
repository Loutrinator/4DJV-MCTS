using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    private Character _character;
    private float _direction;
    private bool _isCrouching;
    private bool _isJumping;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        _direction = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump")){_isJumping = true;}
        if (Input.GetButtonDown("Crouch")){_isCrouching = true;}
        else if (Input.GetButtonUp("Crouch")){_isCrouching = false;}
    }

    private void FixedUpdate()
    {
        _character.Move(_direction,_isCrouching,_isJumping);
        _isJumping = false;
    }
}