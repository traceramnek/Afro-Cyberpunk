using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformer : MonoBehaviour
{
    public Camera myCamera;

    //Initializations
    private Rigidbody2D myRigidBody;
    private BoxCollider2D myBoxCollider;
    private Animator myAnimatorController;
    private bool isEnabled;
    private LevelManager theLevelManager;
    //private bool isAtHome;

    //public Transform homeLocation;
    //public int maxWill;
    //public int maxHealth;
    //private int currentWill;
    //private int currentHealth;
    //public GameObject healthBar;
    //public GameObject willBar;
    //Moving the player
    public float moveSpeed;

    public float stunCooldown;
    private float stunCounter;


    //jump handling
    public float jumpSpeed;
    public LayerMask ground;

    public int maxJumps;
    private int remainingJumps;
    private bool pressingJump;

    //wall jumping
    //public int maxWallJumps;
    //private int remainingWallJumps;
    public float wallJumpSpeed;
    public float wallJumpAngle;

    public float wallJumpFactorX;
    public float wallJumpFactorY;

    // Air Dashing
    public int maxAirDashes;
    private int remainingAirDashes;
    private bool pressingAirDash;

    // wall running

    public float wallRunSpeed;
    public int maxWallRunCount;
    private int remainingWallRunCount;

    public float maxWallRunTime;
    private float remainingWallRunTime;

    public float airDashSpeed;
    public float gravityScale;

    // Use this for initialization
    void Start()
    {
        //currentHealth = maxHealth;
        //currentWill = maxWill;
        theLevelManager = FindObjectOfType<LevelManager>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myAnimatorController = GetComponent<Animator>();
        remainingJumps = 0;
        pressingJump = false;
        stunCounter = 0;
        remainingWallRunCount = maxWallRunCount;
        remainingWallRunTime = maxWallRunTime;
        remainingAirDashes = maxAirDashes;
        isEnabled = true;
        //isAtHome = false;
    }

    public void DisableCollider()
    {
        myBoxCollider.enabled = false;
        myRigidBody.gravityScale = 0;
    }
    public void EnableCollider()
    {
        myBoxCollider.enabled = true;
        myRigidBody.gravityScale = 4;
    }

    void FixedUpdate()
    {
        if (isEnabled)
        {
            // if the player is grounded, reset their jumps
            /*if (Constants.PlayerInput.isPressingReturnToHome && IsGrounded())
            {
                this.transform.position = homeLocation.position;
            }*/

            if (IsGrounded())
            {
                remainingJumps = maxJumps;
                remainingAirDashes = maxAirDashes;
                remainingWallRunCount = maxWallRunCount;
                remainingWallRunTime = maxWallRunTime;
                myAnimatorController.SetBool("grounded", true);


            }
            // if the player is not grounded, but has full jumps, subtract one to prevent free air jump
            else if (remainingJumps >= maxJumps)
            {
                remainingJumps = maxJumps - 1;
                myAnimatorController.SetBool("grounded", false);
            }

            if (IsTouchingWall())
            {
                stunCounter = 0.0f;
            }

            //Moving the Player
            if (stunCounter <= 0)
            {
                myRigidBody.gravityScale = gravityScale;
                if (Constants.PlayerInput.IsPressingRight)
                {
                    myRigidBody.velocity = new Vector3(moveSpeed, myRigidBody.velocity.y, 0f);
                    myAnimatorController.SetFloat("velocity", myRigidBody.velocity.x);

                    //keep the scaling intact
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else if (Constants.PlayerInput.IsPressingLeft)
                {
                    myRigidBody.velocity = new Vector3(-moveSpeed, myRigidBody.velocity.y, 0f);

                    //flip the player while keeping the scaling intact
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else if (IsGrounded())
                {
                    myRigidBody.velocity = new Vector3(0f, myRigidBody.velocity.y, 0f);
                }

                if (Constants.PlayerInput.IsPressingDown)
                {
                    myRigidBody.velocity = new Vector3(0f, myRigidBody.velocity.y, 0f);
                }

                myAnimatorController.SetFloat("velocity", Mathf.Abs(myRigidBody.velocity.x));

            }

            // wall jump
            if (Constants.PlayerInput.IsPressingSpace && IsTouchingWall() && !IsGrounded() && !pressingJump)
            {
                stunCounter = 0.0f;
                myRigidBody.gravityScale = gravityScale;
                float wallJumpx = wallJumpSpeed * Mathf.Cos(wallJumpAngle) * (wallJumpFactorX * transform.localScale.x);
                float wallJumpy = wallJumpSpeed * Mathf.Sin(wallJumpAngle) * (wallJumpFactorY * transform.localScale.y);
                myRigidBody.velocity = new Vector2(wallJumpx, wallJumpy);
                pressingJump = true;
                stunCounter = stunCooldown;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            // multi jump
            //if a player is pressing jump, has jumps left to press, and isn't holding jump from a previous input, and we are not touching the wall jump
            else if (Constants.PlayerInput.IsPressingSpace && remainingJumps > 0 && !pressingJump)
            {
                myRigidBody.gravityScale = gravityScale;
                myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, jumpSpeed, 0f);
                --remainingJumps;
                pressingJump = true;
            }
            else if (!Constants.PlayerInput.IsPressingSpace)
            {
                pressingJump = false;
            }

            // wall run
            if (Constants.PlayerInput.IsPressingRight && IsTouchingWall() && !IsGrounded() && !pressingJump && (remainingWallRunCount > 0) && (remainingWallRunTime >= 0.0f))
            {
                wallRun();
                remainingWallRunTime -= Time.deltaTime;
                // Debug.Log(remainingWallRunCount);
                //remainingWallRunCount--;

            }
            else if (Constants.PlayerInput.IsPressingLeft && IsTouchingWall() && !IsGrounded() && !pressingJump && (remainingWallRunCount > 0) && (remainingWallRunTime >= 0.0f))
            {
                wallRun();
                remainingWallRunTime -= Time.deltaTime;
                //remainingWallRunTime -= Time.deltaTime;
                // Debug.Log(remainingWallRunCount);
                //remainingWallRunCount--;

            }


            // air dash
            if (!IsGrounded() && Constants.PlayerInput.IsPressingAirDash && !pressingAirDash && remainingAirDashes > 0)
            {
                pressingAirDash = true;
                myRigidBody.gravityScale = 0f;
                //localscale will take care of dashing in different directions
                float airDashx = (airDashSpeed * transform.localScale.x);
                float airDashy = 0f;
                myRigidBody.velocity = new Vector2(airDashx, airDashy);
                --remainingAirDashes;
                stunCounter = stunCooldown;

            }
            else if (!Constants.PlayerInput.IsPressingAirDash)
            {
                pressingAirDash = false;
            }
            if (stunCounter >= 0)
            {
                stunCounter -= Time.deltaTime;
            }


            myAnimatorController.SetFloat("velocity", myRigidBody.velocity.x);
        }
        else
        {
            myRigidBody.velocity = new Vector3();
            myAnimatorController.SetFloat("velocity", Mathf.Abs(myRigidBody.velocity.x));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.collider.tag == "death")
        {
            if(currentHealth > 20)
            {
                myRigidBody.position = Checkpoint.activeCheckpoint.transform.position;
            }
            takeHealthDamage(20);
            takeWillDamage(10);
        }
        if(collision.collider.tag == "finished")
        {
            theLevelManager.youWin();
        }*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "levelSelect")
        {
            theLevelManager.EnablePressUpCanvas();
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "levelSelect")
        {
            if (Constants.PlayerInput.IsPressingUp)
            {
                theLevelManager.EnableLevelSelect();
                theLevelManager.DisablePressUpCanvas();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "levelSelect")
        {
            theLevelManager.DisablePressUpCanvas();
            theLevelManager.DisableLevelSelect();
        }
    }

    // checks the grounded state of the BoxCollider2D
    public bool IsGrounded()
    {
        //tweaked the xLeftPosition for scaling purposes (quick fix)
        Vector2 offset = myBoxCollider.offset;
        if (transform.localScale.x < 0) offset.x *= -1;

        Vector2 position = myRigidBody.position + offset;
        Vector2 direction = Vector2.down;
        float xRightPosition = (float)(position.x + ((myBoxCollider.size.x / 2) * transform.localScale.x * 0.95));
        float yRIghtPosition = position.y;
        float xLeftPosition = (float)(position.x - ((myBoxCollider.size.x / 2) * transform.localScale.x * 0.75));
        float yLeftPosition = position.y;

        float distance = (myBoxCollider.size.y / 2) * transform.localScale.y * 1.05f;
        Vector2 positionRight = new Vector2(xRightPosition, yRIghtPosition);
        Vector2 positionLeft = new Vector2(xLeftPosition, yLeftPosition);

        RaycastHit2D hitLeft = Physics2D.Raycast(positionLeft, direction, distance, ground);
        RaycastHit2D hitRight = Physics2D.Raycast(positionRight, direction, distance, ground);

        Debug.DrawRay(positionRight, distance * direction, Color.green);
        Debug.DrawRay(positionLeft, distance * direction, Color.green);

        if ((hitLeft.collider != null) || (hitRight.collider != null))
        {
            return true;
        }

        return false;
    }

    public bool IsTouchingWall()
    {
        Vector2 offset = myBoxCollider.offset;
        if (transform.localScale.x < 0) offset.x *= -1;
        Vector2 position = myRigidBody.position + offset;


        //if you are facing the left, make checks to see if the player is touching the wall on the left
        if (transform.localScale.x < 0)
        {

            Vector2 direction = Vector2.left;
            float xTopPosition = (float)(position.x + ((myBoxCollider.size.x / 2) * transform.localScale.x * 0.95));
            float yTopPosition = position.y;
            float xBottomPosition = (float)(position.x + ((myBoxCollider.size.x / 2) * transform.localScale.x * 0.95));
            float yBottomPosition = position.y;

            //tweaked distance for scaling purposes
            float distance = -(myBoxCollider.size.x / 3) * transform.localScale.x * 1.05f;
            Vector2 positionTop = new Vector2(xTopPosition, yTopPosition);
            Vector2 positionBottom = new Vector2(xBottomPosition, yBottomPosition);

            RaycastHit2D hitTop = Physics2D.Raycast(positionTop, direction, distance, ground);
            RaycastHit2D hitBottom = Physics2D.Raycast(positionBottom, direction, distance, ground);

            Debug.DrawRay(positionTop, distance * direction, Color.red);
            Debug.DrawRay(positionBottom, distance * direction, Color.red);

            if ((hitTop.collider != null) || (hitBottom.collider != null))
            {
                return true;
            }

            return false;
        }
        //if you are facing the right, make checks to see if the player is touching the wall on the right
        else
        {

            Vector2 direction = Vector2.right;
            float xTopPosition = (float)(position.x + ((myBoxCollider.size.x / 2) * transform.localScale.x * 0.95));
            float yTopPosition = position.y;
            float xBottomPosition = (float)(position.x + ((myBoxCollider.size.x / 2) * transform.localScale.x * 0.95));
            float yBottomPosition = position.y;

            float distance = (myBoxCollider.size.x / 2) * transform.localScale.x * 1.05f;
            Vector2 positionTop = new Vector2(xTopPosition, yTopPosition);
            Vector2 positionBottom = new Vector2(xBottomPosition, yBottomPosition);

            RaycastHit2D hitTop = Physics2D.Raycast(positionTop, direction, distance, ground);
            RaycastHit2D hitBottom = Physics2D.Raycast(positionBottom, direction, distance, ground);

            Debug.DrawRay(positionTop, distance * direction, Color.blue);
            Debug.DrawRay(positionBottom, distance * direction, Color.blue);

            if ((hitTop.collider != null) || (hitBottom.collider != null))
            {
                return true;
            }

            return false;
        }
    }

    public void wallRun()
    {
        // move player upward along an object
        myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, wallRunSpeed, 0f);
    }

    /*public void takeHealthDamage(int damageToDeal)
    {
        currentHealth -= damageToDeal;
        if (currentHealth <= 0)
        {
            currentHealth = 5;
            takeWillDamage(10);
            healthBar.transform.localScale = new Vector3((currentHealth * 1.0f) / maxHealth, 1.0f, 1.0f);
            myRigidBody.position = homeLocation.position;
        }
        else
        {
            healthBar.transform.localScale = new Vector3((currentHealth * 1.0f) / maxHealth, 1.0f, 1.0f);
        }
    }
    public void takeWillDamage(int damageToDeal)
    {
        currentWill -= damageToDeal;
        if (currentWill <= 0)
        {
            currentWill = 0;
            willBar.transform.localScale = new Vector3((currentWill * 1.0f) / maxWill, 1.0f, 1.0f);
            theLevelManager.GameOverWill();
        }
        else
        {
            willBar.transform.localScale = new Vector3((currentWill * 1.0f) / maxWill, 1.0f, 1.0f);
        }
    }
    public void healHealth(int amountToHeal)
    {
        currentHealth += amountToHeal;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.transform.localScale = new Vector3((currentHealth * 1.0f) / maxHealth, 1.0f, 1.0f);
        }
        else
        {
            healthBar.transform.localScale = new Vector3((currentHealth * 1.0f) / maxHealth, 1.0f, 1.0f);
        }
    }
    public void healWill(int amountToHeal)
    {
        currentWill += amountToHeal;
        if (currentWill >= maxWill)
        {
            currentWill = maxWill;
            willBar.transform.localScale = new Vector3((currentWill * 1.0f) / maxWill, 1.0f, 1.0f);
        }
        else
        {
            willBar.transform.localScale = new Vector3((currentWill * 1.0f) / maxWill, 1.0f, 1.0f);
        }
    }

    

    public bool isHome()
    {
        return isAtHome;
    }*/
}

