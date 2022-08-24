using System;
using System.Collections;
using UnityEngine;

//Player movement controls the players ability to move around the screen 
public class PlayerMovement : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Movement 
    private Rigidbody2D _rigidbody2D; //Rigidbody component that will move player 
    private float _xVelocity;   //Keeps track of player input on horizontal velocity 
    private float _yVelocity;   //Keeps track of player input on vertical velocity 
    [Header("Character Speed")]
    public float speed = 3;    //Speed at which player can move horizontally and vertically 
     
    //Cursor 
    [Header("Cursor Texture")]
    public Texture2D cursor;   //Keeps track of the cursor image used 

    //Animation 
    private Animator _animator; //Animator that will change based on player input 
    //The Animator Parameter that is used to switch between walking and idle  
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    //Flags 
    private bool _lostControl; //Tells us if the player has been bounced or not 

    
    //==================================================================================================================
    // Base Methods 
    //==================================================================================================================
    
    // Start is called before the first frame update
    private void Start()
    {
        //Connects to the components 
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        //Centers the cursor and connects the texture 
        var cursorHotspot = new Vector2 ((float) cursor.width / 2, (float) cursor.height / 2);
        Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
    }
    
    //Updates the velocities based on player input or enemy push 
    private void FixedUpdate()
    {
        if (!_lostControl)
        {
            _rigidbody2D.velocity = new Vector2(_xVelocity, _yVelocity);
        }
    }

    //==================================================================================================================
    // Movement Methods 
    //==================================================================================================================
    
    //Used in the Game Controller Script 
    //Gets player inputs, update the direction the player is facing and updates their animation state 
    public void UpdateMovement()
    {
        //Collects the player input 
        _xVelocity = Input.GetAxis("Horizontal") * speed;
        _yVelocity = Input.GetAxis("Vertical") * speed;

        UpdateScale();
        UpdateAnimation();
    }
    
    //Checks which way the player is moving and updates the scale to turn the player 
    private void UpdateScale()
    {
        var tran = transform;
        tran.localScale = _xVelocity switch
        {
            < 0 => new Vector3(-1, 1, 1),
            > 0 => new Vector3(1, 1, 1),
            _ => tran.localScale
        };
    }
    
    //Checks if the player is moving or not and updates the animation state based on that 
    private void UpdateAnimation()
    {
        if (Math.Abs(_xVelocity) > 0 || Math.Abs(_yVelocity) > 0)
        {
            _animator.SetBool(IsWalking, true);
        }
        else
        {
            _animator.SetBool(IsWalking, false);
        }
    }
    
    //==================================================================================================================
    // Enemy Affect Methods 
    //==================================================================================================================


    //Used by Enemy Runner Script to stop user input and use Enemy's inflicted velocity 
    public void LoseControl()
    {
        //Stops player from controlling the character 
        _lostControl = true;
        //Allows the player to spin around 
        _rigidbody2D.constraints = RigidbodyConstraints2D.None;
        //Start Counter to Regain Control 
        StartCoroutine(ReGainControl());
    }

    //Waits for player to regain control and reset them to normal control scheme 
    private IEnumerator ReGainControl()
    {
        //Waits for 1 second 
        yield return new WaitForSeconds(1);
        //Returns control to player 
        _lostControl = false;
        //Resets the characters rotation 
        transform.rotation = Quaternion.Euler(0,0,0);
        //Stops them from spinning along Z axis 
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
