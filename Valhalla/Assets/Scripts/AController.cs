using UnityEngine;
[RequireComponent(typeof(Character))]

public abstract class AController : MonoBehaviour
{
    protected Character _character;
    protected float _direction = 0;
    protected bool _isCrouching = false;
    protected bool _isJumping;
    protected bool _noSword;
    protected bool _throwSword = false;
    protected Thrust _thrust;

    public abstract void ExecuteActions();

    private void Awake()
    {
        _character = GetComponent<Character>();
        _thrust = Thrust.NONE;
    }
 
    public void CustomFixedUpdate()
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
