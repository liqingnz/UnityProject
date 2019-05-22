using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static bool bluePlayerTurn = true;
    public static bool singlePlayer = true;

    public static bool defendingTeam = true;

    public static int boardWidth = 20;
    public static int boardHeight = 20;

    public static List<GameObject> teamOne = new List<GameObject>();
    public static List<GameObject> teamTwo = new List<GameObject>();

    public static MapGenerator map;
    public static CanvasController canvas;
    public static GameObject endGameText;
    
    public static string CurrentPlayerTag() {
        if (bluePlayerTurn) return "Player 1";
        return "Player 2";
    }
    
    public static void EndGame()
    {
        //print("end game function called");
        if (GameManager.teamTwo.Count == 0)
        {
            endGameText.SetActive(true);
            endGameText.GetComponent<Text>().text = "Blue Team Won!";
            if (Input.anyKeyDown)
            {
                print("the space key has been pressed");
                SceneManager.LoadScene(0);
                endGameText.SetActive(false);
            }
        }
        if (GameManager.teamOne.Count == 0)
        {
            endGameText.SetActive(true);
            endGameText.GetComponent<Text>().text = "Red Team Won!";
            if (Input.anyKeyDown)
            {
                print("the space key has been pressed");
                SceneManager.LoadScene(0);
                endGameText.SetActive(false);
            }
        }
    }

    public static void ClearAbilities()
    {
        //Removes anny UI elements left over
        foreach (GameObject player in teamOne) player.GetComponent<Unit>().abilitys.CleatUI();
        foreach (GameObject player in teamTwo) player.GetComponent<Unit>().abilitys.CleatUI();
    }
}
