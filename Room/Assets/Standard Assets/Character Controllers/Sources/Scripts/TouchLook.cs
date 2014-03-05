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
public class TouchLook : MonoBehaviour
{

	enum TouchLookCommand
	{
		TlCmd_TurnLeft=0,
		TlCmd_TurnRight,
		TlCmd_ChangeRoomScale,
		TlCmd_None
	}
	enum TouchLookView
	{
		TlVw_Normal=0,
		TlVw_Top
	}
	public float sensitivityX = 1F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float guiboxHeight = 30F;
	public float guiboxWidth = 200F;
	float posLeft = 5F;
	float posTop = 5F;
	TouchLookCommand touchLookCmd = TouchLookCommand.TlCmd_None;
	TouchLookView touchLookView = TouchLookView.TlVw_Top;
	bool changeView = false;
	float vwCapNormalPosY = 20.0F;
	float vwCapTopPosY = 80.0F;
	float roomScale = 100F;
	float roomScaleWidth = 1.0F;
	float roomScaleLength = 1.0F;
	float roomLengthChange = 0.2F;
	float roomWidthChange = 0.2F;


	void Update ()
	{
			//if (!Input.GetMouseButton (0))
			//	return;
			//transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);

		if (touchLookCmd != TouchLookCommand.TlCmd_None && !changeView) {
			switch (touchLookCmd) {
			case TouchLookCommand.TlCmd_TurnLeft:
			// Turn Left
				transform.Rotate (0, -sensitivityX, 0);
				break;
			case TouchLookCommand.TlCmd_TurnRight:
			// Turn Right
				transform.Rotate (0, sensitivityX, 0);
				break;
			case TouchLookCommand.TlCmd_ChangeRoomScale:
				GameObject floor = GameObject.Find("Floor");
				if (floor)
				{
					Vector3 scale = floor.transform.localScale;
					scale.x = roomScale * roomScaleWidth;
					scale.z = roomScale * roomScaleLength;
					floor.transform.localScale = scale;
					changeCurrentView(touchLookView,true);
				}
				break;
			}
			touchLookCmd = TouchLookCommand.TlCmd_None;
		}
		if (changeView) 
		{
			changeCurrentView(touchLookView,false);
 			changeView = false;
		}
	}

	void Start ()
	{
			// Make the rigid body not change rotation
		if (rigidbody)
				rigidbody.freezeRotation = true;
//				GameObject screen = GameObject.Find("Screen");
//				if (screen) 
//				{
//						Vector3 scale = screen.transform.localScale;
//						scale.y = scale.y * (float)(0.5);
//						screen.transform.localScale = scale;
//				}
		changeCurrentView (touchLookView,false);
	}

	void changeCurrentView(TouchLookView view, bool noRotation)
	{
		Vector3 pos = transform.localPosition;
		switch(view)
		{
		case TouchLookView.TlVw_Normal:
			pos.y = vwCapNormalPosY;
			if (!noRotation)
			{
				transform.Rotate(-90,0,0);
			}
			break;
		case TouchLookView.TlVw_Top:
			if (roomScaleWidth*roomScaleLength != 1.0F)
			{

				pos.y = vwCapTopPosY* Mathf.Max(roomScaleWidth,roomScaleLength);
			}
			else
			{
				pos.y = vwCapTopPosY;
			}
			if (!noRotation)
			{
				transform.Rotate(90,0,0);
			}
			break;
		}
		transform.localPosition = pos;
	}
	void OnGUI ()
	{
		if (touchLookView == TouchLookView.TlVw_Normal) {
			if (GUI.RepeatButton (new Rect (posLeft, posTop, guiboxWidth, guiboxHeight), "<<")) {
				touchLookCmd = TouchLookCommand.TlCmd_TurnLeft;
			}
			if (GUI.RepeatButton (new Rect (Screen.width - posLeft - guiboxWidth, posTop, guiboxWidth, guiboxHeight), ">>")) {
				touchLookCmd = TouchLookCommand.TlCmd_TurnRight;
			}
			if (GUI.Button (new Rect (Screen.width/2 - guiboxWidth/2, posTop, guiboxWidth, guiboxHeight), "Top View")) {
				touchLookView = TouchLookView.TlVw_Top;
				changeView = true;
			}
		} else if (touchLookView == TouchLookView.TlVw_Top) {
			if (GUI.Button (new Rect (Screen.width/2 - guiboxWidth/2, posTop, guiboxWidth, guiboxHeight), "Normal View")) {
				touchLookView = TouchLookView.TlVw_Normal;
				changeView = true;
			}
			if (GUI.Button (new Rect (posLeft, posTop, guiboxWidth, guiboxHeight), "Width")) {
				roomScaleWidth += roomWidthChange;
				if (roomScaleWidth > 2.0F)
				{
					roomWidthChange = -1*roomWidthChange;
					roomScaleWidth = 2.0F;
				}
				if (roomScaleWidth < 1.0F)
				{
					roomWidthChange = -1*roomWidthChange;
					roomScaleWidth = 1.0F;
				}
				touchLookCmd = TouchLookCommand.TlCmd_ChangeRoomScale;
			}
			if (GUI.Button (new Rect (Screen.width - posLeft - guiboxWidth, posTop, guiboxWidth, guiboxHeight), "Length")) {
				roomScaleLength += roomLengthChange;
				if (roomScaleLength > 2.0F)				
				{
					roomLengthChange = -1*roomLengthChange;
					roomScaleLength = 2.0F;
				}
				if (roomScaleLength < 1.0F)
				{
					roomLengthChange = -1*roomLengthChange;
					roomScaleLength = 1.0F;
				}
				touchLookCmd = TouchLookCommand.TlCmd_ChangeRoomScale;
			}
		}
	}
}