using UnityEngine;

//Randomly picks sprite skin for the Shooting Enemy 
public class EnemySkin : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    public Sprite[] sprites;                    //Holds all the possible Sprites the Enemy could look like 
    private SpriteRenderer _spriteRenderer;     //Where the new sprite will be applied to 

    //==================================================================================================================
    // Base Methods  
    //==================================================================================================================
    
    //Connects game component adn then randomly picks a sprite 
    private void Start()
    {
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length - 1)];
    }
}
