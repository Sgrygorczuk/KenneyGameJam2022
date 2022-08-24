using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Used to control how the Game Behaves at any moment 
public class GameController : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Player 
    private PlayerMovement _playerMovement; //Used to control players movement   
    private PlayerShoot _playerShoot;       //Used to control player shooting 

    //Level 
    private GameObject _deadGrid;   //Level that's scene when the level is dead 
    private GameObject _greenGrid;  //Level that's scene when the level is green 
    private Animator _animatorTransition;   //Used to transition the black panel in canvas  

    //SFX && Music
    private AudioSource _loseSfx; //Plays when player loses 
    private AudioSource _winSfx; //Plays when player wins 
    private GameObject _deadMusic; //Music when the game is dead 
    private GameObject _greenMusic; //Music when the game is green 
    
    //Flags 
    public string thisLevel; //Tells us what level to reload upon death
    private bool _won;       //Tells us that the player won and to not execute CheckIfWin 
    private int _enemyCount; //Tells us how many enemies the level started with 
    private int _enemiesBeat; //Tells us how many enemies the player beat 
    
    //Game States 
    private enum States  //States used to define different state the game can be in 
    {
        Start,
        Playing,
        Win,
        Lose
    }
    private States _currentState = States.Start; //Keep track of what our current state is 
    
    //==================================================================================================================
    // Base Methods 
    //==================================================================================================================
    
    //Connects all of the game object and game component then starts transtion to start the game 
    private void Start()
    {
        //Gets Enemy count 
        _enemyCount = transform.GetChild(0).childCount;
        _animatorTransition = GameObject.Find("Canvas").transform.Find("Panel").GetComponent<Animator>();
        
        _deadGrid = GameObject.Find("Game").transform.Find("DeadGrid").gameObject;
        _greenGrid = GameObject.Find("Game").transform.Find("GreenGrid").gameObject;

        _deadMusic = GameObject.Find("Game").transform.Find("GameController").transform.Find("Music").transform
            .Find("Dead").gameObject;
        _greenMusic = GameObject.Find("Game").transform.Find("GameController").transform.Find("Music").transform
            .Find("Green").gameObject;

        _loseSfx = transform.Find("SFX").transform.Find("Lose").GetComponent<AudioSource>();
        _winSfx = transform.Find("SFX").transform.Find("Win").GetComponent<AudioSource>();
        
        _playerMovement = GameObject.Find("Game").transform.Find("Player").GetComponent<PlayerMovement>();
        _playerShoot = GameObject.Find("Game").transform.Find("Player").transform.GetChild(1)
            .GetComponent<PlayerShoot>();
        
        StartCoroutine(FromStartToPlay());
    }

    //Updates the game differently for every state the game can be in 
    private void Update()
    {
        switch (_currentState)
        {
            case States.Start: { break; }
            //Allows the player to move and shoot and check if the player has beat enough enemies to win 
            case States.Playing:
            {
                _playerMovement.UpdateMovement();
                _playerShoot.UpdateShooting();
                CheckIfWin();
                break;
            }
            case States.Win: { break; }
            case States.Lose: { break; }
            default: throw new ArgumentOutOfRangeException();
        }
    }
    //==================================================================================================================
    // Win Methods 
    //==================================================================================================================
    
    //Used by Enemy Health to update the number of enemies beat 
    public void BeatEnemy() { _enemiesBeat++; }
    
    //Checks if the player has beaten all of the enemies if so start the new state 
    private void CheckIfWin()
    {
        if (_enemyCount != _enemiesBeat || _won) return;
        _currentState = States.Win;
        StartCoroutine(FromPlayToWin());
    }

    //==================================================================================================================
    // Transition Methods  
    //==================================================================================================================
    
    //Waits 0.5 sec as the screen goes from black to transparent before giving player control 
    private IEnumerator FromStartToPlay()
    {
        yield return new WaitForSeconds(0.5f);
        _currentState = States.Playing;
    }

    //Occurs when the player has beaten all of the enemies in the level 
    private IEnumerator FromPlayToWin()
    {
     
        //Plays win SFX, start the fade in and out animation  
        _winSfx.Play();
        _won = true;
        _animatorTransition.Play($"Transition");
        
        yield return new WaitForSeconds(0.5f);
        //Changes the tilemap and the music 
        _deadGrid.SetActive(false);
        _greenGrid.SetActive(true);
        _deadMusic.SetActive(false);
        _greenMusic.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        //Lets Player control the character again 
        _currentState = States.Playing;
    }
    
    //Used when player dies 
    public IEnumerator FromPlayToLose()
    {
        //Play Die SFX and start the animation 
        _loseSfx.Play();
        _animatorTransition.Play($"Exit");
        _currentState = States.Lose;
        yield return new WaitForSeconds(1.1f);
        //Reloads the scene 
        SceneManager.LoadScene(thisLevel);
    }
}
