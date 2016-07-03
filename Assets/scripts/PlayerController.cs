﻿using UnityEngine;
using System.Collections;
using Prime31;


public class PlayerController : MonoBehaviour
{
    public KeyboardManager keyboardManager;
    public PowerUpManager powerupManager;

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

    public AudioClip jumpAudio;
    public AudioClip pickupAudio;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
    private bool _hasDoubleJumped = false;

    private float defaultRunSpeed;
    private float defaultJumpHeight;

    private AudioSource audioSource;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();

        audioSource = GetComponent<AudioSource>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;

        if (name.Contains("Player1"))
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

        defaultRunSpeed = runSpeed;
        defaultJumpHeight = jumpHeight;

        audioSource = GetComponent<AudioSource>();
	}

	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
        {
			return;
        }
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
            audioSource.clip = jumpAudio;
            audioSource.Play();

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

            Bomb bomb = bombObject.GetComponent<Bomb>();

            bomb.setOwner(gameObject);

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

    public int getMovementDirection()
    {
        Vector3 localVelocity = transform.InverseTransformVector(_velocity);

        if (localVelocity.x <= -.5f)
        {
            return -1;
        }else if (localVelocity.x >= 0.5f)
        {
            return 1;
        }

        return 0;
    }

    public void resetPosition()
    {
        Vector3 position;
        position.x = 0f;
        position.y = 0f;
        position.z = 0f;

        transform.localPosition = position;
    }

    public void resetStats()
    {
        jumpHeight = defaultJumpHeight;
        runSpeed = defaultRunSpeed;
    }

    public void performPowerUpAction(PowerUpType type)
    {
        audioSource.clip = pickupAudio;
        audioSource.Play();

        StartCoroutine(
            performTemporaryPowerUpAction(
                type
            )
        );
    }

    IEnumerator performTemporaryPowerUpAction(PowerUpType type)
    {
        switch(type)
        {
        case PowerUpType.JUMP_UP:
            {
                float updatedValue = powerupManager.jumpUp;
                jumpHeight = updatedValue;

                yield return new WaitForSeconds(powerupManager.powerupTime);

                jumpHeight = defaultJumpHeight;
            }
            break;
        case PowerUpType.SPEED_UP:
            {
                float updatedValue = powerupManager.speedUp;
                runSpeed = updatedValue;

                yield return new WaitForSeconds(powerupManager.powerupTime);

                runSpeed = defaultRunSpeed;
            }
            break;
        }
    }
}
