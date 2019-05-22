using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

    public GameObject endGameText;

    private void Start() {
        GameManager.canvas = this;
        GameManager.endGameText = endGameText;
    }

    public void setTxt() {
        Text txtP1, txtP2;
        if (GameManager.bluePlayerTurn) {
            txtP1 = GameObject.Find("BlueUnitInfo").GetComponent<Text>();
            txtP2 = GameObject.Find("RedUnitInfo").GetComponent<Text>();
        } else {
            txtP2 = GameObject.Find("BlueUnitInfo").GetComponent<Text>();
            txtP1 = GameObject.Find("RedUnitInfo").GetComponent<Text>();
        }

        // print("Acess Map " + map.mapWidth);
        if (GameManager.map.SelectedUnitCurrentPlayer) {

            Unit BlueUnit = GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>();
            txtP1.text = "Name: " + BlueUnit.name +
                        "\nHP: " + BlueUnit.getHelth() +
                        "\nMP: " + BlueUnit.getMovementPoints();
        } else {
            txtP1.text = "No Unit Selected";
        }

        if (GameManager.map.TargetedUnitEnemyPlayer) {
            Unit RedUnit = GameManager.map.TargetedUnitEnemyPlayer.GetComponent<Unit>();
            txtP2.text = "Name: " + RedUnit.name +
                        "\nHP: " + RedUnit.getHelth() +
                        "\nMP: " + (int)RedUnit.getMovementPoints();
        } else {
            txtP2.text = "No Unit Selected";
        }
    }


    public void EndCurrentTurn() {
        print("Num of blue team unit:" + GameManager.teamOne.Count + "Num of red team unit:" + GameManager.teamTwo.Count);
        GameManager.EndGame();
        if (GameManager.map.SelectedUnitCurrentPlayer != null) {
            GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>().abilitys.CleatUI();
            GameManager.map.SelectedUnitCurrentPlayer = null;
        }

        if (!GameManager.singlePlayer) {
            GameManager.bluePlayerTurn = !GameManager.bluePlayerTurn;
            if (GameManager.bluePlayerTurn) {
                print("List size " + GameManager.teamOne.Count);
                foreach (GameObject players in GameManager.teamOne) players.GetComponent<Unit>().NextTurn();
                print("Player 1 Turn");
            } else {
                foreach (GameObject players in GameManager.teamTwo) players.GetComponent<Unit>().NextTurn();
                print("Player 2 Turn");
            }
        } else {
            // AI working in progress
            foreach (GameObject players in GameManager.teamOne) players.GetComponent<Unit>().NextTurn();

            //AI moves
            StartCoroutine(AIMoves());
            
            // Clear the node for next turn calculation
            AIAlgorithms.occupiedTiles.Clear();
        }
        GameManager.map.ClearHighLight();
        GameManager.map.SelectedUnitCurrentPlayer = null;
        GameManager.map.TargetedUnitEnemyPlayer = null;
        GameManager.map.TargetedUnitCurrentPlayer = null;
        setTxt();

        GameManager.ClearAbilities();
    }

    IEnumerator AIMoves(float waitTime = 2.5f)
    {
        if(GameManager.teamTwo.Count > 0)
        {
            foreach (GameObject players in GameManager.teamTwo)
            {
                //print(players);
                players.GetComponent<Unit>().NextTurn();
                players.GetComponent<AI>().AIMoves();
                yield return new WaitForSeconds(waitTime);
            }
        }
        
    }
}
