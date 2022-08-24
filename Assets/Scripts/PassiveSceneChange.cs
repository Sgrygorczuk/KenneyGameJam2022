using UnityEngine;
using UnityEngine.SceneManagement;

//Controls for the Credits Level 
public class PassiveSceneChange : MonoBehaviour
{

    //==================================================================================================================
    // Variables 
    //==================================================================================================================

    public string nextSceneName = "Menu"; //Tell us which scene to go to
    public bool level = true;
    
    //==================================================================================================================
    // Base Methods  
    //==================================================================================================================
    
    private void Update()
    {
        //Checks if the player click LMB then moves to chosen Scene 
        if (Input.GetKeyDown(KeyCode.Mouse0) && !level)
        {
            SceneManager.LoadScene(nextSceneName);   
        }
    }
    
    //==================================================================================================================
    // Trigger Methods  
    //==================================================================================================================
    
    //If player walks into the collider it moves them to the next level 
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && level)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
