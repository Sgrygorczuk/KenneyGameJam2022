using UnityEngine;

//Controls player health and dying 
public class PlayerHealth : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Players Visual Health 
    private SpriteRenderer _spriteRenderer; //Connects to the sprite 
    private readonly Color _dec = new(0.1f, 0.1f, 0.1f, 0); //The decrement in color for each hit 
    private int _healthCounter = 6; //How many times the player can be hit before dying 

    //Feed Back 
    private Animator _animator;  //Used to start the death animation 
    private AudioSource _audioSource; //Used to player the taking damage 
    
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Connects Game Components
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    //==================================================================================================================
    // Damage Methods 
    //==================================================================================================================

    //Takes damage, if the player has 0 health start the dying State 
    public void TakeDamage()
    {
        //Plays SFX 
        _audioSource.Play();
        //Lowers the Color 
        _spriteRenderer.color -= _dec;
        //Lowers Health Counter 
        _healthCounter--;

        //If player has 0 health 
        if (_healthCounter != 0) return;
        //Set color to be black
        _spriteRenderer.color = Color.black;
        //Change the State of Game 
        StartCoroutine(GameObject.Find("GameController").GetComponent<GameController>().FromPlayToLose());
        //Start Death Animation 
        _animator.Play($"Die");

    }
    
    //==================================================================================================================
    // Trigger Methods  
    //==================================================================================================================

    //Checks if the player touched an enemy bullet 
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("EnemyBullet")) return;
        //If player gets touched by ball take damage as long as health is bigger than 0
        if(_healthCounter > 0) { TakeDamage();}
        //Destroy the bullet 
        Destroy(col.gameObject);
    }
    
}
