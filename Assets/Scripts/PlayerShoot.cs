using UnityEngine;

//Controls Player's ability to Shoot 
public class PlayerShoot : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Game Objects 
    public GameObject preFab;        //The Bullet PreFab we will spawn
    private Transform _bulletTrash;  //Where the bullets will be placed upon spawning 
    private Transform _bulletSpawn;  //Where the bullet will be spawned at 
    
    //Camera + Mouse 
    private Camera _camera;    //Camera Game Object 
    private Vector3 _mousePos; //Keeps track of where the mouse cursor is 

    //Bullet Spawning Timers 
    private const float Timer = 0.5f;  //How long should it take till player can next bullet 
    private float _currentTime = 0.5f; //Counter to allow player to shoot 
    private bool _canShoot = true;     //Tells us if we can shoot or not 
    
    //==================================================================================================================
    // Base Methods   
    //==================================================================================================================

    //Connects Game Objects 
    public void Start()
    {
        //Connects Components 
        _camera = GameObject.Find("Game").transform.Find("Game_Camera").GetComponent<Camera>();
        _bulletTrash = GameObject.Find("Game").transform.Find("BulletTrash").transform;
        _bulletSpawn = transform.GetChild(0).transform;
    }

    //==================================================================================================================
    // Shooting Methods   
    //==================================================================================================================

    //Used by Game Controller 
    //Checks where the player's mouse is, move the spawner to face that direction 
    //Check if the player can spawn a bullet
    //Spawns bullets 
    public void UpdateShooting()
    {
        MousePosUpdate();
        BulletSpawnTimer();
        SpawnBullet();
    }

    //Checks where the mouse is and update the rotation of the spawn pos 
    private void MousePosUpdate()
    {
        //Gets the player mouse in respect to the camera 
        _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        //Gets where the based on where the mouse and character are 
        var pos = _mousePos - transform.position;
        //Gets the rotation based on this position difference 
        var rotZ = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        //Updates the rotation based on previous calculations
        transform.rotation = Quaternion.Euler(0, 0, rotZ - 90);
    }

    //Checks if the player can shoot, if they can't counts down till they can again  
    private void BulletSpawnTimer()
    {
        //If player can shoot don't do anything else, count down 
        if (_canShoot) return;
        _currentTime -= Time.deltaTime;
        //If timer is less than 0 allow player to shoot and reset the counter 
        if (!(_currentTime <= 0)) return;
        _currentTime = Timer;
        _canShoot = true;
    }

    //Creates the bullet, puts it in a trash game object and stops player from shooting further 
    private void SpawnBullet()
    {
        //Checks if player can shoot/clicked input to shoot 
        if (!Input.GetKey(KeyCode.Mouse0) || !_canShoot) return;
        //Make bullet 
        var bullet = Instantiate(preFab, _bulletSpawn.position, Quaternion.identity);
        //Attach to trash 
        bullet.transform.SetParent(_bulletTrash);
        //Stop player from shooting 
        _canShoot = false;
    }
}
