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
				TlCmd_None
		}
		public float sensitivityX = 1F;
		public float minimumX = -360F;
		public float maximumX = 360F;
		public float guiboxHeight = 30F;
		public float guiboxWidth = 200F;
		float posLeft = 5F;
		float posTop = 5F;
		TouchLookCommand touchLookCmd = TouchLookCommand.TlCmd_None;

		void Update ()
		{
				//if (!Input.GetMouseButton (0))
				//	return;
				//transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);

				if (touchLookCmd != TouchLookCommand.TlCmd_None) {
						switch (touchLookCmd) {
						case TouchLookCommand.TlCmd_TurnLeft:
						// Turn Left
								transform.Rotate (0, -sensitivityX, 0);
								break;
						case TouchLookCommand.TlCmd_TurnRight:
						// Turn Right
								transform.Rotate (0, sensitivityX, 0);
								break;
						}
						touchLookCmd = TouchLookCommand.TlCmd_None;
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
		}

		void OnGUI ()
		{
				if (GUI.RepeatButton (new Rect (posLeft, posTop, guiboxWidth, guiboxHeight), "Left")) {
						touchLookCmd = TouchLookCommand.TlCmd_TurnLeft;
				}
				if (GUI.RepeatButton (new Rect (Screen.width - posLeft - guiboxWidth, posTop, guiboxWidth, guiboxHeight), "Right")) {
						touchLookCmd = TouchLookCommand.TlCmd_TurnRight;
				}
				//GUI.Label (new Rect (posLeft, posTop + guiboxHeight , guiboxWidth, guiboxHeight), touchX.ToString ());
				//GUI.Label (new Rect (posLeft, posTop + guiboxHeight * 2, guiboxWidth, guiboxHeight), touchY.ToString ());
		}
}