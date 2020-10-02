using UnityEngine;
[RequireComponent(typeof(Character))]

public abstract class AController : MonoBehaviour
{
    public int id;
    protected Character _character;
    [SerializeField]protected float _direction = 0;
    [SerializeField]protected bool _isCrouching = false;
    [SerializeField]protected bool _isJumping;
    [SerializeField]protected bool _noSword;
    protected bool _throwSword = false;
    [SerializeField]protected bool _attack;
    [HideInInspector] public bool _isDead = false; // false when the player is dead 

    public abstract void ExecuteActions();

    private void Awake()
    {
        _character = GetComponent<Character>();
        _attack = false;
    }
 
    public void CustomFixedUpdate()
    {
        if(_isDead || GameManager.IsPaused) return;
        _character.Move(_direction,_isCrouching,_isJumping);
        _isJumping = false;
        if (_throwSword)
        {
            _character.ThrowSword();
            _noSword = true;
        }
        if(_noSword)  return;
        if(_attack) _character.Attack();
        _attack = false;
    }
    
    public void changeAliveStatus()
    {
        _isDead = !_isDead;
    }
}
