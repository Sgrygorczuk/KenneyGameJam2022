using System.Collections;
using UnityEngine;

//Controls the Enemy that Runs at the player 
public class EnemyRunner : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
      
    //Movement 
    public float speed; //Tells us how fast the enemy is moving 
    private Rigidbody2D _rigidbody2D; //Used to control enemy 

    //Distance to player 
    public float minDistance;
    private Transform _player;

    //Animation 
    private Animator _animator; //Animator 
    private static readonly int IsWalking = Animator.StringToHash("isWalking"); //Used to switch between walking and idle 
      
    //Flag and SFX 
    private bool _didDamage; //Tells us if you can damage player 
    private AudioSource _audioSource; //Plays Damage SFX 
    private EnemyHealth _enemyHealth; //Used to update enemy health 

    //==================================================================================================================
    // Base Methods  
    //==================================================================================================================
    
    //Connects Game Components 
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Game").transform.Find("Player");
        _enemyHealth = GetComponent<EnemyHealth>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //Checks if the player is close enough to move towards, if so walks towards them otherwise Idles 
    private void Update()
    {
        if (!_enemyHealth.cleansed)
        {
            var distance = Vector3.Distance(transform.position, _player.position);
                
            if (distance < minDistance) { Move(); }
            else { Idle(); }
        }
        else { Idle(); }
    }
    
    //==================================================================================================================
    // Walking Methods 
    //==================================================================================================================

    //Makes sure the Enemy is Standing Still and uses the Idle Animation  
    private void Idle()
    {
        _animator.SetBool(IsWalking, false);
        _rigidbody2D.velocity = Vector2.zero;
    }

    //Makes sure the Enemy is Standing Walking towards the player and uses the Walking Animation  
    private void Move()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);
        _animator.SetBool(IsWalking, true);
    }
    
    //==================================================================================================================
    // Trigger Methods  
    //==================================================================================================================
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        //Checks if touching player and able to damage player 
        if (!col.collider.CompareTag("Player") || _didDamage) return;
        //If touched player can't damaged them for 1 second 
        _didDamage = true;
        StartCoroutine(CanDamage());
        //Get the players body and give it a force 
        col.gameObject.GetComponent<Rigidbody2D>().AddForce((_player.position - transform.position).normalized * 250);
        //Takes away player's ability to control the character 
        col.gameObject.GetComponent<PlayerMovement>().LoseControl();
        //If the enemy is not saved deal damage to the player 
        if (!_enemyHealth.cleansed)
        {
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage();   
        }
        //Play sound effect 
        _audioSource.Play();
    }

    //Waits 1 Second before being able to hit the player again  
    private IEnumerator CanDamage()
    {
        yield return new WaitForSeconds(1);
        _didDamage = false;
    }
    
    //==================================================================================================================
    // Gizmo Functions  
    //==================================================================================================================
        
    //Used to visualize the Radius the Enemy can traverse 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,minDistance);
    }
}
