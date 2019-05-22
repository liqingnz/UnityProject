using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Main manu UI set up
public class MainManu : MonoBehaviour
{
    // Old main manu
    /*
    public GUISkin skin;
    public float width = 150;
    public float height = 80;
    Rect playButton;
    Rect singlePlayerButton;
    Rect quitButton;
    Rect title;// = new Rect((Screen.width / 2) - 200, 200, 400, 80);

    // Use this for initialization
    private void Start()
    {
        playButton = new Rect((Screen.width / 3) - (width / 2), 380, width, height);
        singlePlayerButton = new Rect((Screen.width / 2) - (width / 2), 380, width, height);
        quitButton = new Rect(((Screen.width / 3) * 2) - (width / 2), 380, width, height);
        title = new Rect((Screen.width / 2) - 200, 200, 400, 80);
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        GUI.Label(title, "159.333 Project");
        if (GUI.Button(playButton, "Play"))
        {
            GameManager.singlePlayer = false;
            SceneManager.LoadScene(1);
        }

        if (GUI.Button(singlePlayerButton, "Single Player"))
        {
            GameManager.singlePlayer = true;
            SceneManager.LoadScene(1);
        }

        if (GUI.Button(quitButton, "Quit"))
        {
            Application.Quit();
        }
    }
    */

    public void Defend()
    {
        GameManager.singlePlayer = true;
        GameManager.defendingTeam = true;
        SceneManager.LoadScene(1);
    }

    public void Invade()
    {
        GameManager.singlePlayer = true;
        GameManager.defendingTeam = false;
        SceneManager.LoadScene(1);
    }

    public void Multiplayer()
    {
        GameManager.singlePlayer = false;
        GameManager.defendingTeam = true;
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
