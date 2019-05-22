using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Inside main camera
public class MouseInfo : MonoBehaviour {

    private void Update() {
        // Reference to the script MapGenerator

        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (MouseOverObject()) {
                //print(MouseOverObject().name);
                Transform mouseOverObject = MouseOverObject().transform;


                //Left click
                if (Input.GetMouseButtonDown(0)) {
                    if (mouseOverObject.parent.tag.Contains("Player")) {
                        if (mouseOverObject.parent.GetComponent<Unit>().tag == GameManager.CurrentPlayerTag()) {
                            GameObject selectedUnit = GameManager.map.SelectedUnitCurrentPlayer;
                            GameObject mousOverGameObject = mouseOverObject.parent.gameObject;
                            GameObject friendlyTarget = GameManager.map.TargetedUnitCurrentPlayer;


                            if (selectedUnit != null && mousOverGameObject != selectedUnit && mousOverGameObject != friendlyTarget) {
                                GameManager.map.TargetedUnitCurrentPlayer = mouseOverObject.parent.gameObject;
                                print("found friendly target");
                            } else {
                                if (GameManager.map.TargetedUnitCurrentPlayer == mouseOverObject.parent.gameObject) {
                                    GameManager.map.TargetedUnitCurrentPlayer = null;
                                }
                                toggleButtons();
                                GameManager.map.ClearHighLight();
                                GameManager.map.SelectedUnitCurrentPlayer = mouseOverObject.parent.gameObject;
                                GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>().abilitys.ToggleActive();
                                print("found a player: " + GameManager.map.SelectedUnitCurrentPlayer.name);
                                GameManager.map.PathHighLight();
                            }
                        } else {
                            GameManager.map.ClearHighLight();
                            GameManager.map.TargetedUnitEnemyPlayer = mouseOverObject.parent.gameObject;
                            print("found an enemy: " + GameManager.map.TargetedUnitEnemyPlayer.name);
                            //if (map.SelectedUnitCurrentPlayer != null) { print("?????"); }//testing
                        }
                    } else {//if additionnal GetMouseButtonDown(0) tests are requierd change this to elsif or put before this test
                        GameManager.map.ClearHighLight();
                        toggleButtons();
                        GameManager.map.SelectedUnitCurrentPlayer = null;
                        GameManager.map.TargetedUnitEnemyPlayer = null;
                        GameManager.map.TargetedUnitCurrentPlayer = null;
                        //if (map.SelectedUnitCurrentPlayer != null) { print("?????"); }//testing
                    }
                    GameManager.canvas.setTxt();
                }


                //Right click
                if (Input.GetMouseButtonDown(1)) {
                    if (mouseOverObject.tag == "Tile") {
                        int x, y;
                        x = mouseOverObject.GetComponent<Hightlight>().tileX;
                        y = mouseOverObject.GetComponent<Hightlight>().tileY;
                        print("path generated to: " + x + " " + y);

                        GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>().GeneratePathTo(x, y);
                        GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>().pathEnd = new int[2] { x, y };
                        //map.selectedUnit.GetComponent<Unit>();
                    } else if (mouseOverObject.parent.tag.Contains("Player") && mouseOverObject.parent.GetComponent<Unit>().tag != GameManager.CurrentPlayerTag()) {
                        if (GameManager.map.SelectedUnitCurrentPlayer) {
                            Unit atacker = GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>();
                            print("Player: " + atacker.getMovementPoints());
                            GameObject target = mouseOverObject.parent.gameObject;
                            atacker.atack.Target(target);
                            print("Atack!");
                        } else {
                            print("No Unit Selected!");
                        }
                    }
                    GameManager.canvas.setTxt();
                }
            }
        }
    }


    public void toggleButtons() {
        if (GameManager.map.SelectedUnitCurrentPlayer != null) {
            GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>().abilitys.ToggleActive();
        }
    }
    

    // Shows where the mouse is pointing at
    // And also return the object
    public Transform MouseOverObject() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo)) {
            //print(hitInfo.transform.name);
            return hitInfo.transform;
        }
        return null;
    }
}
