using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public float jumpForce = 700f;
	public float runSpeed = 15f;
	public LayerMask whatIsGround;
	public Transform groundCheck;

	float fallMultiplier = 2.5f;
	float lowJumpMultiplier = 2f;
	float delayToExtraJumpForce = 0.2f;
	float extraJumpForce = 500;
	float jumpTimer;

	float groundRadius = 0.1f;


	bool jumping = false;
	bool longJump = false;
	bool moving = false;


	bool grounded = false;
	bool facingRight = true;

	Rigidbody2D body;

    // Start is called before the first frame update
    void Awake()
    {
		body = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		/*if(jumping){
			HandleAirBornMovement();
		}*/
    }

	private void Update()
	{
		if(jumping){
			if((Time.time - jumpTimer) < delayToExtraJumpForce){
				//body.velocity = new Vector2(body.velocity.x, 10f);
				body.AddForce(new Vector2(0, jumpForce));
			}else{
				jumping = false;
			}
		} 



		if(body.velocity.y < 0){
			body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (body.velocity.y > 0 && !jumping){
			body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	public void Jump(){
		if(grounded && !jumping){
			jumping = true;
			jumpTimer = Time.time;
		}
	}

	public void Move(float horizontalMove)
	{
		body.velocity = new Vector2(horizontalMove * runSpeed, transform.position.y);
		if(horizontalMove != 0){
			moving = true;
			if(horizontalMove > 0 && !facingRight){
				Flip();
			}else if(horizontalMove < 0 && facingRight){
				Flip();
			}
		}else{
			moving = false;
		}
	}

	private void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void HandleAirBornMovement(){
		Vector2 extraGravityForce = (Physics.gravity * fallMultiplier) - Physics.gravity;
		body.AddForce(extraGravityForce);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (jumping)
		{
			if(collision.transform.tag == "Ground"){
				jumping = false;
				longJump = false;
			}
		}
	}
}
