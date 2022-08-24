using UnityEngine;

//Controls the Shooting done by the Shooting Enemy 
public class EnemyShootBullet : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Game Objects 
    public GameObject preFab;      //Bullet PreFab 
    private Transform _player;     //Player Location 
    private Transform _spawnPoint; //When the bullet will spawn 
    
    //Other 
    public int offSet; //Used to offset the angle to the bullets shoot in the right direction 
    
    //Timers 
    private const float Timer = 0.5f;   //How long till the bullet is shot 
    private float _currentTime = Timer; //What is the current time till shooing 
    
    //==================================================================================================================
    // Base Methods  
    //==================================================================================================================
    
    //Connect to the Game Objects adn Components  
    private void Start()
    {
        _player = GameObject.Find("Game").transform.Find("Player");
        _spawnPoint = transform.GetChild(0).transform;
    }

    //Calcuates the rotation spawn in respect to player 
    private void Update()
    {
        var rot = _player.position - transform.position;
        var rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ - offSet);
    }
    
    //==================================================================================================================
    // Variables 
    //==================================================================================================================

    //Used by Enemy Shoot Walk
    //Shoots a bullet at the player if the timer is set to 0
    public void Shoot()
    {
        //Counts down 
        _currentTime -= Time.deltaTime;
        if (!(_currentTime <= 0)) return;
        //Reset and spawn bullet 
        _currentTime = Timer;
        Instantiate(preFab, _spawnPoint.position, Quaternion.identity);
    }
}
