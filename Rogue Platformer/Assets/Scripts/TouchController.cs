using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;

	public Joystick joystick;
	public CharacterController characterController;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (joystick.Horizontal >= 0.2f)
		{
			horizontalMove = 1;
		}
		else if (joystick.Horizontal <= -0.2f)
		{
			horizontalMove = -1;
		}else{
			horizontalMove = 0;
		}
		characterController.Move(horizontalMove);

		if (joystick.Vertical >= 0.5f)
		{
			Debug.Log("jumping");
			jump = true;
			characterController.Jump();
		}

		if (joystick.Vertical <= -0.5f)
		{
			crouch = true;
		}else{
			crouch = false;
		}
    }
}
