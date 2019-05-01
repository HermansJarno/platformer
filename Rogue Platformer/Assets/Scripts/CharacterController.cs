using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public float jumpForce = 700f;
	public float kickJumpForce = 150f;
	public float kickForwardJumpForce = 200f;
	public float runSpeed = 15f;
	public LayerMask whatIsGround;
	public Transform groundCheck;

	public Animator animator;

	float fallMultiplier = 2.5f;
	float lowJumpMultiplier = 2f;
	float delayToExtraJumpForce = 0.2f;
	float extraJumpForce = 500;
	float jumpTimer;
	float kickTimer;

	float groundRadius = 0.03f;
	float gravityScale = 0f;


	bool jumping = false;
	bool longJump = false;
	bool moving = false;

	bool kicking = false;

	bool grounded = false;
	bool facingRight = true;

	Rigidbody2D body;

    // Start is called before the first frame update
    void Awake()
    {
		body = transform.GetComponent<Rigidbody2D>();
		gravityScale = body.gravityScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		if(grounded){
				body.gravityScale = gravityScale;
		}
		animator.SetBool("Jumping", !grounded);
    }

	private void Update()
	{
		if(jumping){
			if((Time.time - jumpTimer) < delayToExtraJumpForce){
				body.AddForce(new Vector2(0, jumpForce));
				body.gravityScale -= ((Time.time - jumpTimer) * 15);
			}else{
				if(grounded){
					jumping = false;
					if(kicking){
						animator.SetBool("Kick", false);
						kicking = false;
					}
				}else{
					body.gravityScale += ((Time.time - jumpTimer) * 1.5f);
				}
			}
		} 

		if(body.velocity.y < 0){
			body.velocity += Vector2.down * Physics2D.gravity.y * 25 * Time.deltaTime;
		} else if (body.velocity.y > 0 && !jumping){
			body.velocity += Vector2.down * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
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
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		if(grounded){
			if(!kicking){
 				body.velocity = new Vector2(horizontalMove * runSpeed, transform.position.y);
			}
		}else{
			body.velocity = new Vector2(horizontalMove * (runSpeed - ((Time.time - jumpTimer) * 1.5f)), transform.position.y);
		}
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

	public void Kick(){
		if(!kicking){
			kickTimer = Time.time;
			animator.SetBool("Kick", true);
			Invoke("DeactivateKick", 0.70f);
			if(grounded){
				Invoke("KickForce", 0.45f);
			}
		}
		kicking = true;
	}

	private void KickForce(){
		if(kicking && ((Time.time - kickTimer) >= 0.40f)){
			if(facingRight){
				body.AddForce(new Vector2(kickForwardJumpForce, kickJumpForce));
			}else{
				body.AddForce(new Vector2(-kickForwardJumpForce, kickJumpForce));
			}
		}
	}

	public void DeactivateKick(){
		kicking = false;
		animator.SetBool("Kick", false);
	}

	private void Flip(){
		if(!kicking){
			animator.SetBool("Kick", false);
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
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
