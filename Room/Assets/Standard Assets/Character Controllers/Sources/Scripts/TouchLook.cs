using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class TouchLook : MonoBehaviour {

	public float sensitivityX = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;
	

	public float guiboxHeight= 30F;
	public float guiboxWidth= 200F;

	float rotationY = 0F;

	float touchX = 0F;
	float touchY = 0F;

	float posLeft = 5F;
	float posTop = 5F;

	void Update ()
	{
		//if (!Input.GetMouseButton (0))
		//	return;
		//transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		if (Input.touchCount > 0) 
		{
			Touch lastTouch = Input.GetTouch(Input.touchCount-1);

			if (lastTouch.phase == TouchPhase.Ended)
			{
				touchX = lastTouch.position.x;
				touchY = lastTouch.position.y;

				if (isLeftTurnTouched(lastTouch.position))
				{
					// Turn Left
					transform.Rotate(0,-sensitivityX,0);
				}
				else if(isRightTurnTouched(lastTouch.position))
				{
					// Turn Right
					transform.Rotate(0,sensitivityX,0);
				}
			}

		}
	}

	bool isLeftTurnTouched(Vector2 pos)
	{
		if ((pos.x >= posLeft && pos.x <= posLeft+guiboxWidth)
		    &&
		    (pos.y <= (Screen.height- posTop) && pos.y >= (Screen.height - posTop - guiboxWidth))
		    ) 
		{
			return true;
		}
		return false;
	}
	bool isRightTurnTouched(Vector2 pos)
	{
		if ((pos.x <= (Screen.width - posLeft) && pos.x >= (Screen.width - posLeft-guiboxWidth))
		    &&
		    (pos.y <= (Screen.height- posTop) && pos.y >= (Screen.height - posTop - guiboxWidth))
		    ) 
		{
			return true;
		}
		return false;
	}
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
	void OnGUI()
	{
		GUI.Box (new Rect(posLeft,posTop,guiboxWidth,guiboxHeight), "Left");
		GUI.Box (new Rect(Screen.width-posLeft-guiboxWidth,posTop,guiboxWidth,guiboxHeight), "Right");
		//GUI.Label (new Rect (posLeft, posTop + guiboxHeight , guiboxWidth, guiboxHeight), touchX.ToString ());
		//GUI.Label (new Rect (posLeft, posTop + guiboxHeight * 2, guiboxWidth, guiboxHeight), touchY.ToString ());
	}
}