using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundSelection : MonoBehaviour
{
    // Only Object Pooling and Vfx bonus are not done. Every other requirement and bonuses are done

    public Button backGroundButton;
    public Color myColor;
    private GameManager gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        GetComponent<Button>().onClick.AddListener(StartGameWithBackground);
    }
    
    //This method sets the gamebackground and starts the game by passing it's color value
    void StartGameWithBackground()
    {
        gameManagerScript.StartGame(myColor);
    }
}
