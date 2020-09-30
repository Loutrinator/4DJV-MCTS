using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraPanningController : MonoBehaviour
{
	[SerializeField] private float cameraMovingDistance = 18f;
	[SerializeField] private float detectionRange = 1f;
	[SerializeField] private float movingSpeed = 5f;
	[SerializeField] private float cameraPrecision = 0.1f;
	
	private enum CameraState {idle,panning}
	
	private CameraState cameraState = CameraState.idle;

	private Vector3 targetPosition;
		
    // Start is called before the first frame update
    void Start()
    {
	    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
	    if (cameraState == CameraState.idle)
	    {
		    CheckTarget();
	    }
	    else if(cameraState == CameraState.panning)
	    {
		    MoveCamera();
	    }
    }

    private void CheckTarget()
    {
	    float sens = GameManager.Instance.Direction;

	    Character player = GameManager.Instance.MainPlayer;
	    if (player != null)
	    {
		    float diff = (player.transform.position.x - this.transform.position.x + sens * cameraMovingDistance/2);
		
		    if (diff < detectionRange)
		    {
			    StartMoveCamera();
		    }
	    }
	    
    }

    private void MoveCamera()
    {
	    Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * movingSpeed);
	    if ((newPos - targetPosition).magnitude < cameraPrecision)
	    {
		    transform.position = targetPosition;
		    cameraState = CameraState.idle;
	    }
	    else
	    {
		    transform.position = newPos;
	    }
    }

    void StartMoveCamera()
	{
		float sens = GameManager.Instance.Direction;
		targetPosition = this.transform.position + sens * cameraMovingDistance * Vector3.right;
		cameraState = CameraState.panning;
	}
}
