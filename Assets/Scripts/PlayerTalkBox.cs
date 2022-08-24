using UnityEngine;
using TMPro;

//Checks if the player has walked into a Talk Zone if they have gather the info and display it 
//If you left turn off dialogue  
public class PlayerTalkBox : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    private GameObject _frame;      //Controls if the frame sprites are on/off
    private TextMeshProUGUI _text;  //Controls what's being show in text 
    
    //==================================================================================================================
    // Base Methods  
    //==================================================================================================================
    
    //Connects Game Objects 
    private void Start()
    {
        _frame = GameObject.Find("Space").transform.Find("TextBox").gameObject;
        _text = GameObject.Find("Canvas").transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    
    //==================================================================================================================
    // Trigger Methods  
    //==================================================================================================================

    //Checks if player has walked into a Talk Zone, if they have display the sprites and get text 
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag($"TalkZone")) return;
        //Turn on the Frame 
        _frame.SetActive(true);
        //Check if the zone is from enemy or Planet and gather the text from it and display it 
        if (col.transform.parent.GetComponent<EnemyHealth>() != null)
        {
            _text.text =col.transform.parent.GetComponent<EnemyHealth>().GetText();
        }
        else if (col.transform.parent.GetComponent<PlanetText>() != null)
        {
            _text.text =col.transform.parent.GetComponent<PlanetText>().GetText();
        }
    }
    
    //Checks if the player has walked out to the talk zone, turn off the display and text 
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag($"TalkZone")) return;
        //Remove the frame 
        _frame.SetActive(false);
            
        //Disable the planet upon exit 
        if (col.transform.parent.GetComponent<PlanetText>() != null)
        {
            col.transform.parent.gameObject.SetActive(false);
        }
           
        //Set the text to be blank 
        _text.text = "";
    }
}
