using UnityEngine;

//Controls the walking of the Shooting Enemy 
public class EnemyShootWalk : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
        
    //Walking 
    public float speed;         //Speed at which player can walk 
    public float minDistance;   //Min distance from player till enemy starts walking 
    private Rigidbody2D _rigidbody2D; //Used to control enemy movement 
    private Transform _player;   //Used to track player position
    
    //Used for shooting 
    public float shootRange;  //Set range till enemy can shoot at player 
    private EnemyShootBullet _enemyShootBullet; //Script used to make bullets 
   
    //Flag
    private EnemyHealth _enemyHealth; //Used to tell if enemy has been saved or not 

    //Animation 
    private Animator _animator; //Animator  
    private static readonly int IsWalking = Animator.StringToHash("isWalking"); //Used to move between idle and walking 

    //==================================================================================================================
    // Variables 
    //==================================================================================================================

    //Connects game objects and game components 
    private void Start() 
    {
        _player = GameObject.Find("Game").transform.Find("Player");
        _enemyShootBullet = transform.GetChild(1).GetComponent<EnemyShootBullet>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        //As long as the enemy has not been saved
        if (!_enemyHealth.cleansed)
        {
            var distance = Vector3.Distance(transform.position, _player.position);

            //If player is near enough ideal and Shoot at the player 
            if (distance < shootRange)
            {
                _enemyShootBullet.Shoot();
                Idle();
            }
            //If player is within walking distance but not shooting walk towards them 
            else if (distance < minDistance) { Move(); }
            //Else Stand Still 
            else { Idle(); }
        }
        //Stand Still 
        else { Idle(); }
    } 
    
    //==================================================================================================================
    // Walking  
    //==================================================================================================================

    //Stands still and uses Idle animation 
    private void Idle()
    {
        _animator.SetBool(IsWalking, false);
        _rigidbody2D.velocity = Vector2.zero;
    }

    //Moves towards the player and uses walk animation 
    private void Move()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);
        _animator.SetBool(IsWalking, true);
    }
    
    //==================================================================================================================
    // Gizmo Methods  
    //==================================================================================================================
        
    //Shows where enemy walking and shooting ranges 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,minDistance);
        
        Gizmos.color = Color.red;
        if (transform != null) Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
