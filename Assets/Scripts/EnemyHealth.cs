using UnityEngine;
using Random = UnityEngine.Random;

//Keeps track of what the enemy's health is and keeps track of what text they will say when they are cleansed 
public class EnemyHealth : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Images 
    private SpriteRenderer _bodySpriteRenderer;   //Keeps track of sprite so they can be lighter with each hit  
    private SpriteRenderer _emoteSpriteRenderer;  //Controls which emote sprite is shown 
    public Sprite[] sprites;                      //Holds all of the emote sprites that one will be picked out of 
    private readonly Color _inc = new (0.2f, 0.2f, 0.2f);  //How much the color is incremented by 
    
    //Text 
    public string text  = ""; //Keeps track of what the enemy will say after they are saved 

    //Connects to Sprite 0, random 5 texts 
    private readonly string[] _happy = 
    {
        "You're a hero!", "All the sadness has left me!", "I can laugh again!", "HAPPY!",
        "Dude, like this is, like really cool man!"
    };
    
    //Connects to Sprite 1, random 5 texts 
    private readonly string[] _excited = 
    {
        "You're amazing", "You gotta save the planet!", "Man that was sick, keep it up!",
        "Never seen moves like yours before.", "Man you're like my best friend now."
    };

    //Connects to Sprite 2, random 5 texts 
    private readonly string[] _love = 
    {
        "You looking for a date?", "I love you!", "Look at those big muscles.", "Can I get an autograph!",
        "My shining knight!"
    };

    //Connects to Sprite 3, random 5 texts 
    private readonly string[] _star = 
    {
        "Next time we go out, the drinks are on me!", "I'm gonna make a movie about you!", "You're the hero of legend!",
        "OHHHHHH GREAT LORD HOW CAN I EVER REPAY YOU!", "You're like a 5/7 hero."
    };

    //Connects to Sprite 4, random 5 texts 
    private readonly string[] _confused = 
        { "What happened....?", "Was I just covered in sludge...?", "Where am I?", "......", "I feel cold." };
    
    //Flag, Health and SFX 
    public bool cleansed;              //Checks if the enemy has been saved 
    private int _counter = 5;          //How much health the enemy has left 
    private AudioSource _audioSource;  //The hit SFX 

    //==================================================================================================================
    // Based Methods  
    //==================================================================================================================
    
    //Connects all of the Game Components, and Randomizes Emote Sprite/Text 
    private void Start()
    {
        //
        _audioSource = GetComponent<AudioSource>();
        _bodySpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _emoteSpriteRenderer = transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>();
        //Pick a random emote and connect the sprite and text 
        var rand = Random.Range(0, sprites.Length - 1);
        _emoteSpriteRenderer.sprite = sprites[rand];
        PickText(rand);
    }
    
    //==================================================================================================================
    // Text Methods  
    //==================================================================================================================
    
    //Using the Random Int used to pick emote choose which array we pick from then randomizes one of the 
    //Choices out that array 
    private void PickText(int choice)
    {
        var rand = Random.Range(0, 4);
        text = choice switch
        {
            0 => _happy[rand],
            1 => _excited[rand],
            2 => _love[rand],
            3 => _star[rand],
            4 => _confused[rand],
            _ => text
        };
    }

    //Used by Player Talk Box to get the Enemy data 
    public string GetText()
    {
        return text;
    }
    
    //==================================================================================================================
    // Trigger Methods 
    //==================================================================================================================
    
    //Checks if the player bullet hit the Enemy 
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("PlayerBullet") || cleansed) return;
        //Update the color and lower the health 
        _bodySpriteRenderer.color += _inc;
        _counter--;

        //If the enemy reaches 0 health turn them white, turn on the emote and allow player to talk to them and update 
        //Game Controller 
        if (_counter == 0)
        {
            cleansed = true;
            _emoteSpriteRenderer.color = Color.white;
            _bodySpriteRenderer.color = Color.white;
            GameObject.Find("GameController").GetComponent<GameController>().BeatEnemy();
            transform.Find("ChatZone").gameObject.SetActive(true);
        }
        
        //Play Hit SFX and Destroy the bullet 
        _audioSource.Play();
        Destroy(col.gameObject);
    }
}
