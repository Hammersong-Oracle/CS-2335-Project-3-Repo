using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    //for water hazard type event where Player dies not due to change in GameData
    public UnityEvent onPlayerDied = new UnityEvent(); //call constructor
    public UnityEvent onPlayerReachExit = new UnityEvent();

    public enum HeroState { idle, walk, jump }  //create custom data-type - have integer values

    public HeroState currentHeroState;  //will display the current enum 

    private Animator animator;  //- null - reference variable to access animator component

    public bool facingRight;  //keep track of sprite direction - used in Flip
    private Rigidbody2D myRBody2D;
    public float forceX; //used for adjusting velocity

    public Transform groundCheck; //transform component on GroundCheck object
    public LayerMask groundLayer; //allows us to interact with a physics Layer 
    public float groundCheckRadius;  //
    public float jumpForce;
    public bool grounded = false;  //will let us see if the gameOjb is grounded

    // Start is called before the first frame update
    void Start()
    {
        currentHeroState = HeroState.idle;    //initialize to show it's in idle to start
        animator = GetComponent<Animator>();//is on the same game object as this script
        animator.SetInteger("HeroState", (int)HeroState.idle);  //send in the signal: 0
        facingRight = true;
        myRBody2D = GetComponent<Rigidbody2D>();
        forceX = 100.0f;  //force value may need adjusted

        groundCheckRadius = 0.2f; //may need modified
        jumpForce = 10f; //may need modified


    }

    // Update is called once per frame
    void FixedUpdate()  //physics methods executed - want consistant time between frames
    {
        float inputX = Input.GetAxis("Horizontal");  //values of -1, 0 , 1 
        bool isWalking = Mathf.Abs(inputX) > 0;

        if (isWalking)
        {
            //check for flipping
            if (inputX > 0 && !facingRight)  //moving right, facing left
            {
                Flip(); //flip right
            }
            else if (inputX < 0 && facingRight) //moving left, facing right
            {
                Flip(); //flip left
            }
            animator.SetInteger("HeroState", (int)HeroState.walk);
            myRBody2D.velocity = new Vector2(0, myRBody2D.velocity.y);  //reset the velocity to 0 //may come back and change
            myRBody2D.AddForce(new Vector2(inputX * forceX, 0)); //add horizontal force to move the player
        }
        else
        {
            animator.SetInteger("HeroState", (int)HeroState.idle);
        }

        bool jumpPressed = Input.GetButtonDown("Jump");   //is spacebar pressed
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (jumpPressed && grounded)
        {
            animator.SetInteger("HeroState", (int)HeroState.jump);
            Debug.Log("Jumping");

            ///Vertical Force for Movement
            myRBody2D.velocity = new Vector2(myRBody2D.velocity.x, 0);  //reset the velocity to 0 //keep horizontal movement
            myRBody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); //add horizontal force to move the player

        }

    }//end FixedUpdate

    //we have determined it is facing the wrong direction, it must need flipped for this to be executed
    private void Flip()
    {
        facingRight = !facingRight; //toggle direction variable
        //get Scale Vector
        Vector3 theScale = transform.localScale;  //get the current values for Scale Vector
        theScale.x *= -1; //modify the X component    //mirror the sprite
        transform.localScale = theScale; //set the actual Scale vector with our temp Vector
    }


    /// <summary>
    /// THIS IS THE EVENT that starts the chain reaction of events
    /// </summary>
    /// <param name="collision">Collision.</param>
    //Customize to your game needs
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string itemTag = collision.tag;
        //Debug.Log("itemTag " + itemTag);
        switch (itemTag)
        {
            case "Collectible":
                PickUp item = collision.GetComponent<PickUp>();
                if (item != null)
                {
                    GameData.instanceRef.Add(item.Value); //points for each specific item's value

                    //add to inventory
                    GameData.instanceRef.AddItem(item.itemInstance); //points for each specific item's value
                }

                //Add Audio Logic here if desired

                Destroy(collision.gameObject);
                break;

            case "Hazard":
                //decrease health
                //what type of object has tag "Hazard"
                Hazard hazardItem = collision.GetComponent<Hazard>();

                if (hazardItem != null) //make sure has Hazard Component
                {
                    GameData.instanceRef.TakeDamage(hazardItem.Value);
                }
                else
                {
                    Debug.Log("ERROR: Add Hazard Component, Remove PickUp Component");
                }

                Destroy(collision.gameObject);
                break;

            case "Water":
                Debug.Log("Hit Water");

                //Add Audio Logic Here if Desired

                if (onPlayerDied != null) //there are listeners
                {
                    onPlayerDied.Invoke();
                }
                break;

            case "Exit":
                if (onPlayerReachExit != null) //are there any listeners?
                {
                    onPlayerReachExit.Invoke(); //execute any method added as a listener
                }
                break;

            default:
                Debug.Log("No Match on Trigger's Tag");
                break;

        } //end switch Statement
    } //end OnTriggerEnter function
}///end of PlayerController Class