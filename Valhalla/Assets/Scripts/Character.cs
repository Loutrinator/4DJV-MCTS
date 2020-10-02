using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    #region Fields

    [Header("Sounds FX")] 
    [SerializeField] private AudioSource _footstepSFX = null;
    [SerializeField] private AudioSource _jumpSFX = null;
    [SerializeField] private AudioSource _crouchSFX = null;
    [SerializeField] private AudioSource _thrustSFX = null;
    [SerializeField] private AudioSource _hurtSFX = null;

    [Header("Collision")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _seekGround = null;
    [SerializeField] private Transform _seekCeiling = null;
    [SerializeField] private Transform _seekAttackCollision = null;
    [SerializeField] private Vector2 _swordColliderRange = Vector2.one;
    [SerializeField] private BoxCollider2D _standingCollider = null;
    private bool _isGrounded;
    private bool _isCeilingColliding;
    private bool _isAttackCollidingAnotherPlayer;
    [SerializeField] private bool _isInTheAir;
    const float GROUND_RADIUS  = .2f;
    const float CEILING_RADIUS = .2f;
    
    [Header("Movement")]
    [SerializeField] private float _jumpForce = 400f;
    [SerializeField] private float _speed = 10f;
    
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;
    private Rigidbody2D _body;
    public PlayerData data;
    private bool _flip;
    
    [Header("Animation")]
    private Animator _animator;

    [Header("Events")]
    [SerializeField] private UnityEvent OnDie;
    [SerializeField] private UnityEvent OnRespawn;
    
    [Header("Debug")]
    [SerializeField] private bool _showDebug = true;
    private bool _isDead;
    
    #endregion

    #region Overide functions

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _flip = false;
    }
    private void Start()
    {
        data.transform.position = transform.position;
        data.transform.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_seekGround.position, GROUND_RADIUS, _groundLayer);
        _isGrounded = colliders.Length > 0;

        if (!_isInTheAir || _isGrounded) return;
        _isInTheAir = false;
        _animator.SetBool("isJumping", false);

    }

    private void OnDrawGizmos()
    {
        if(!_showDebug) return;
        Gizmos.color = _isGrounded ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(_seekGround.position, GROUND_RADIUS);
        Gizmos.color = _isCeilingColliding ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(_seekCeiling.position, CEILING_RADIUS);
        Gizmos.color = _isAttackCollidingAnotherPlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireCube(new Vector3(_seekAttackCollision.position.x,_seekAttackCollision.position.y,0 ),
                                                _swordColliderRange);
    }

    #endregion
    
    #region Movement

    public void Move(float movement, bool isCrouching, bool isJumping)
    {
        //Debug.Log("isJumping : " + isJumping);
        if(_isDead) return;

        Vector2 acceleration = movement * _speed * Vector2.right;
        if (isJumping)
        {
            Debug.Log("CA SAUTE PAR ICI");
            acceleration = Jump(isJumping, acceleration);
        }
        
        data.transform = SimulatedPhysic.Move(data.transform,_standingCollider, acceleration, Time.fixedDeltaTime);
        //data.transform = SimulatedPhysic.Move(data.transform, acceleration, Time.fixedDeltaTime);
        transform.position = new Vector3(data.transform.position.x,data.transform.position.y);
        
        _animator.SetBool("isRunning", movement!=0);
        if (movement > 0 && _flip) Flip();
        else if (movement < 0 && !_flip) Flip();
        
        Crouch(isCrouching);

    }
    
    private Vector2 Jump(bool isJumping, Vector2 currentAcceleration)
    {
        if (!_isGrounded)
        {
            
            Debug.Log("PAS DE SOL PAS DE CHOCOLAT");
            return currentAcceleration;
        }
        
        Debug.Log("ENVOLE TOI PETIT OISEAU");
        
        currentAcceleration += _jumpForce * Vector2.up;
        
        _animator.SetBool("isJumping", true);
        _isInTheAir = true;
        _jumpSFX.Play();
        
        return currentAcceleration;
    }

    public void Flip()
    {
        _flip = !_flip;
        transform.localScale = new Vector3(-1*transform.localScale.x,transform.localScale.y,transform.localScale.z);
    }

    private void Crouch(bool isCrouching)
    {

        if (Physics2D.OverlapCircle(_seekCeiling.position, CEILING_RADIUS, _groundLayer))
        {
            isCrouching = true;
        }

        _animator.SetBool("isCrouching", isCrouching);
        //_standingCollider.enabled = !isCrouching;
    }

    #endregion

    #region Attack

    public void Attack()
    {
        _animator.SetTrigger(("attack"));
        Collider2D[] other = Physics2D.OverlapBoxAll(_seekAttackCollision.position, _swordColliderRange, _playerLayer);
        _isAttackCollidingAnotherPlayer = other.Length > 0;
        if (_isAttackCollidingAnotherPlayer) TryAttack(other);
        
        // switch (height)
        // {
        //     case Thrust.NONE : break;
        //     case Thrust.HIGH:
        //     {
        //         HighThrust();
        //         break;
        //     }
        //     case Thrust.MEDIUM:
        //     {
        //         MediumThrust();
        //         break;
        //     }
        //     case Thrust.LOW:
        //     {
        //         LowThrust();
        //         break;
        //     }
        //     default:
        //         throw new ArgumentOutOfRangeException(nameof(height), height, height + " isn't defined in Thrust enum.");
        // }
    }
/*
    private void HighThrust()
    {
        _animator.SetTrigger(("highThrust"));
        Collider2D[] other = Physics2D.OverlapBoxAll(_seekHighThrustCollision.position, _swordColliderRange, _playerLayer);
        _isHThrustCollidingAnotherPlayer = other.Length > 0;
        if (_isHThrustCollidingAnotherPlayer) TryAttack(other);
        
    }

    private void MediumThrust()
    {
        _animator.SetTrigger(("mediumThrust"));
        Collider2D[] other = Physics2D.OverlapBoxAll(_seekMediumThrustCollision.position, _swordColliderRange, _playerLayer);
        _isMThrustCollidingAnotherPlayer = other.Length > 0;
        if (_isMThrustCollidingAnotherPlayer) TryAttack(other);

    }

    private void LowThrust()
    {
        _animator.SetTrigger(("lowThrust"));
        Collider2D[] other = Physics2D.OverlapBoxAll(_seekLowThrustCollision.position, _swordColliderRange, _playerLayer);
        _isLThrustCollidingAnotherPlayer = other.Length > 0;
        if (_isLThrustCollidingAnotherPlayer) TryAttack(other);

    }*/

    public void ThrowSword()
    {
        _animator.SetTrigger(("throwSword"));

    }

    public IEnumerator Respawn()
    {
        _isDead = true;
        _animator.SetBool("isDead",true);
        GameManager.Instance.PlayerDied(data.id);
        OnDie?.Invoke();
        if(_isDead) _hurtSFX.Play();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);

        Vector3 position;
        if (GameManager.Instance.currentLevel.GetSpawnPosition(out position))//si on est autorisé à spawn
        {
            yield return new WaitForSeconds(1f);
            transform.position = position;
            gameObject.SetActive(true);
            // send a signal to the game manager who will give it his new position
            _animator.SetBool("isDead",false);
            OnRespawn?.Invoke();
            _isDead = false;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }

    private void TryAttack(Collider2D[] other)
    {
        _thrustSFX.Play();
        foreach (Collider2D o in other)
        {
            try
            {
                if (o.gameObject.Equals(gameObject)) continue;
                StartCoroutine(o.GetComponent<Character>().Respawn()); break;

            }
            catch(NullReferenceException e){}
        }
        _isAttackCollidingAnotherPlayer = false;
    }
    

    #endregion

    private void PlayFootstep()
    {
        _footstepSFX.Play();
    }

    private void PlayCrouch()
    {
     //   if(!_crouchSFX.isPlaying) _crouchSFX.Play();
    }
  
}
