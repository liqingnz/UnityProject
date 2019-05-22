using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReachObjective : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        //print(other.transform.parent.tag + " has collided");
        if (GameManager.defendingTeam)
        {
            if(other.transform.parent.tag == "Player 2")
            {
                GameManager.teamOne.Clear();
                GameManager.EndGame();
            }

        }
        else
        {
            if (other.transform.parent.tag == "Player 1")
            {
                GameManager.teamTwo.Clear();
                GameManager.EndGame();
            }
        }
    }
}
