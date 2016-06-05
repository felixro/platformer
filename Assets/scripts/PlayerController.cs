using UnityEngine;
using System.Collections;
using Prime31;


public class PlayerController : MonoBehaviour
{
    public KeyboardManager keyboardManager;

    // keyboard config
    private KeyCode leftKey;
    private KeyCode rightKey;
    private KeyCode jumpKey;
    private KeyCode downKey;
    private KeyCode shootKey;

	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

    public GameObject bombPrefab;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
    private bool _hasJumped = false;
    private bool _hasDoubleJumped = false;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

        if (name.Contains("Player"))
        {
            leftKey = keyboardManager.player1LeftKey;
            rightKey = keyboardManager.player1RightKey;
            jumpKey = keyboardManager.player1JumpKey;
            downKey = keyboardManager.player1DownKey;
            shootKey = keyboardManager.player1ShootKey;            
        }else
        {
            leftKey = keyboardManager.player2LeftKey;
            rightKey = keyboardManager.player2RightKey;
            jumpKey = keyboardManager.player2JumpKey;
            downKey = keyboardManager.player2DownKey;
            shootKey = keyboardManager.player2ShootKey;    
        }
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
        handleInput();

		if( _controller.isGrounded )
        {
            _hasDoubleJumped = false;
			_velocity.y = 0;
        }

		if( Input.GetKey( rightKey ) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
            {
				//_animator.Play( Animator.StringToHash( "Run" ) );
            }
		}
        else if( Input.GetKey( leftKey ) )
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
            {
				//_animator.Play( Animator.StringToHash( "Run" ) );
            }
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded )
            {
                // FIXME: uncomment when animator controller available
				//_animator.Play( Animator.StringToHash( "Idle" ) );
            }
		}

        // allow double jumping
		if( !_hasDoubleJumped && Input.GetKeyDown( jumpKey ) )
		{
            if (!_controller.isGrounded)
            {
                _hasDoubleJumped = true;
            }

			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
            _animator.SetTrigger("Jump");
		}


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets uf jump down through one way platforms
		if( _controller.isGrounded && Input.GetKey( downKey ) )
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

    void handleInput()
    {
        if (Input.GetKeyDown(shootKey))
        {
            GameObject bombObject = Instantiate (bombPrefab, transform.position, Quaternion.identity) as GameObject;

            Rigidbody2D body = bombObject.GetComponent<Rigidbody2D>();
            body.AddForce(
                new Vector2(
                    _controller.velocity.x * 10f, 
                    Mathf.Sign(_controller.velocity.y) * 20f
                ),
                ForceMode2D.Force
            );
        }
    }

    public void resetPosition()
    {
        Vector3 position;
        position.x = 0f;
        position.y = 0f;
        position.z = 0f;

        transform.localPosition = position;
    }
}
