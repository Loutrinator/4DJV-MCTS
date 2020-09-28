using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Character : MonoBehaviour
{
    #region Fields

    [Header("Collision")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _seekGround = null;
    [SerializeField] private Transform _seekCeiling = null;
    [SerializeField] private Collider2D _standingCollider = null;
    private bool _isGrounded;
    private bool _isCeilingColliding;
    const float GROUND_RADIUS  = .2f;
    const float CEILING_RADIUS = .2f;
    
    [Header("Movement")]
    [SerializeField] private float _jumpForce = 400f;
    [SerializeField] private float _speed = 10f;
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;
    private float _direction; // 1 = right, -1 = left
    private Rigidbody2D _body;
    private Vector3 _velocity = Vector3.zero;
    private SpriteRenderer _renderer;
    private bool _flip;
    
    [Header("Stance")]
    [SerializeField] private Sprite _idle = null;
    [SerializeField] private Sprite _crouch = null;
    
    [Header("Debug")]
    [SerializeField] private bool _showDebug = true;
   
    #endregion

    #region Overide functions

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _flip = false;
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_seekGround.position, GROUND_RADIUS, _groundLayer);
        _isGrounded = colliders.Length > 0;
    }

    private void OnDrawGizmos()
    {
        if(!_showDebug) return;
        Gizmos.color = _isGrounded ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(_seekGround.position, GROUND_RADIUS);
        Gizmos.color = _isCeilingColliding ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(_seekCeiling.position, CEILING_RADIUS);
    }

    #endregion
    
    #region Movement

    public void Move(float movement, bool isCrouching, bool isJumping)
    {
        Vector3 targetVelocity = new Vector2(movement * Time.deltaTime * _speed, _body.velocity.y);
        _body.velocity = Vector3.SmoothDamp(_body.velocity, targetVelocity, ref _velocity, _movementSmoothing);
        if (movement > 0 && _flip) Flip();
        else if (movement < 0 && !_flip) Flip();
        Stance(isCrouching);
        if (isJumping) Jump();
    }

    private void Jump()
    {
        if(!_isGrounded) return;
        _body.AddForce(new Vector2(0f, _jumpForce));
    }

    private void Flip()
    {
        _flip = !_flip;
        transform.localScale = new Vector3(-1*transform.localScale.x,transform.localScale.y,transform.localScale.z);
    }

    private void Stance(bool isCrouching)
    {
        if (Physics2D.OverlapCircle(_seekCeiling.position, CEILING_RADIUS, _groundLayer))
        {
            isCrouching = true;
        }
        _renderer.sprite = isCrouching ? _crouch :  _idle;
        _standingCollider.enabled = !isCrouching;
    }

    #endregion

    #region Attack

    public void HighThrust(){}
    public void MediumThrust(){}
    public void LowThrust(){}
    public void ThrowSword(){}

    #endregion
    


}
