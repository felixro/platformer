using UnityEngine;
using System.Collections;
using Prime31;


public class SmoothFollow : MonoBehaviour
{
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;
    private Transform _target;
	
	void Awake()
	{
		transform = gameObject.transform;
	}
	
    public void setTarget(Transform target)
    {
        _target = target;
    }

	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, _target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			return;
		}
		
		if( _playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, _target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, _target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
		}
	}
	
}
