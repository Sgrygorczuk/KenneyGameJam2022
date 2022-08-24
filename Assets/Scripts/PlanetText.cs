using UnityEngine;

//Used by the planet when it talks to the Player 
public class PlanetText : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    public string text  = ""; //What the planet will say to the player 
    
    //==================================================================================================================
    // Text Method 
    //==================================================================================================================
    
    //Gives the player the text that will be displayed  
    public string GetText()
    {
        return text;
    }
}
