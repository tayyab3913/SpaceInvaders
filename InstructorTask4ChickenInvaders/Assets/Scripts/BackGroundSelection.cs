using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundSelection : MonoBehaviour
{
    public Button backGroundButton;
    public Color myColor;
    private GameManager gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        GetComponent<Button>().onClick.AddListener(StartGameWithBackground);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGameWithBackground()
    {
        gameManagerScript.StartGame(myColor);
    }
}
