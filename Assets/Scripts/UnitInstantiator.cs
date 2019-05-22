using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInstantiator : MonoBehaviour {

    public GameObject[] unit;
    MapGenerator map;
    AI AIController;
    //public 

    //public Material[] unitMaterial;


    public void MakeSwordsman(bool player1, int x, int y) {
        GameObject unitInstance;
        //unitInstance = Instantiate(unit[0], map.TileCoordToWorldCoord(x, y), Quaternion.identity) as GameObject;
        unitInstance = Instantiate(unit[0], new Vector3(1,0,1), Quaternion.identity) as GameObject;
        unitInstance.GetComponent<Unit>().init();
        unitInstance.transform.position = map.TileCoordToWorldCoord(x, y);

        if (player1)
        {
            unitInstance.GetComponent<Unit>().setColor(Color.blue);
            unitInstance.tag = "Player 1";
            unitInstance.name = "Player 1 Swordsman";
            unitInstance.GetComponent<Unit>().tileX = x;
            unitInstance.GetComponent<Unit>().tileY = y;
            GameManager.teamOne.Add(unitInstance);
        }
        else
        { // else player 2
            if (GameManager.singlePlayer)
            {
                AIController = unitInstance.AddComponent<AI>();
                AIController.AIBehaviors = new List<AIBehaviors>();
                AIBehaviors newBehavior = unitInstance.AddComponent<AIMelee>();
                AIController.AIBehaviors.Add(newBehavior);
                newBehavior = unitInstance.AddComponent<Retreat>();
                AIController.AIBehaviors.Add(newBehavior);
            }
            unitInstance.GetComponent<Unit>().setColor(Color.red);
            unitInstance.tag = "Player 2";
            unitInstance.name = "Player 2 Swordsman";
            unitInstance.GetComponent<Unit>().tileX = x;
            unitInstance.GetComponent<Unit>().tileY = y;
            GameManager.teamTwo.Add(unitInstance);
        }
        unitInstance.GetComponent<Unit>().AddDefaultAbility<SwordStrike>();
        unitInstance.GetComponent<Unit>().AddAbility<SwordSpin>();
    }

    public void MakeArcher(bool player1, int x, int y) {
        GameObject unitInstance;
        unitInstance = Instantiate(unit[1], map.TileCoordToWorldCoord(x, y), Quaternion.identity) as GameObject;

        unitInstance.GetComponent<Unit>().init();
        unitInstance.transform.position = map.TileCoordToWorldCoord(x, y);

        if (player1) {
            unitInstance.GetComponent<Unit>().setColor(Color.blue);
            unitInstance.tag = "Player 1";
            unitInstance.name = "Player 1 Archer";
            unitInstance.GetComponent<Unit>().tileX = x;
            unitInstance.GetComponent<Unit>().tileY = y;
            GameManager.teamOne.Add(unitInstance);
        }
        else { // else player 2
            if (GameManager.singlePlayer) {
                AIController = unitInstance.AddComponent<AI>();
                AIController.AIBehaviors = new List<AIBehaviors>();
                AIBehaviors newBehavior = unitInstance.AddComponent<AIRange>();
                AIController.AIBehaviors.Add(newBehavior);
                newBehavior = unitInstance.AddComponent<Retreat>();
                AIController.AIBehaviors.Add(newBehavior);
            }
            unitInstance.GetComponent<Unit>().setColor(Color.red);
            unitInstance.tag = "Player 2";
            unitInstance.name = "Player 2 Archer";
            unitInstance.GetComponent<Unit>().tileX = x;
            unitInstance.GetComponent<Unit>().tileY = y;
            GameManager.teamTwo.Add(unitInstance);
        }
        unitInstance.GetComponent<Unit>().AddDefaultAbility<BowStirke>();
        unitInstance.GetComponent<Unit>().AddAbility<QuickShot>();
        unitInstance.GetComponent<Unit>().AddAbility<AimedShot>();
    }

    public void MakeMage(bool player1, int x, int y) {
        GameObject unitInstance;
        unitInstance = Instantiate(unit[2], map.TileCoordToWorldCoord(x, y), Quaternion.identity) as GameObject;

        unitInstance.GetComponent<Unit>().init();
        unitInstance.transform.position = map.TileCoordToWorldCoord(x, y);

        if (player1) {
            unitInstance.GetComponent<Unit>().setColor(Color.blue);
            unitInstance.tag = "Player 1";
            unitInstance.name = "Player 1 Mage";
            unitInstance.GetComponent<Unit>().tileX = x;
            unitInstance.GetComponent<Unit>().tileY = y;
            GameManager.teamOne.Add(unitInstance);
        }
        else { // else player 2
            if (GameManager.singlePlayer) {
                AIController = unitInstance.AddComponent<AI>();
                AIController.AIBehaviors = new List<AIBehaviors>();
                AIBehaviors newBehavior = unitInstance.AddComponent<AIHeal>();
                AIController.AIBehaviors.Add(newBehavior);
                newBehavior = unitInstance.AddComponent<Retreat>();
                AIController.AIBehaviors.Add(newBehavior);
            }
            unitInstance.GetComponent<Unit>().setColor(Color.red);
            unitInstance.tag = "Player 2";
            unitInstance.name = "Player 2 Mage";
            unitInstance.GetComponent<Unit>().tileX = x;
            unitInstance.GetComponent<Unit>().tileY = y;
            GameManager.teamTwo.Add(unitInstance);
        }
        unitInstance.GetComponent<Unit>().AddDefaultAbility<StaffStrike>();
        unitInstance.GetComponent<Unit>().AddAbility<MagicBolt>();
        unitInstance.GetComponent<Unit>().AddAbility<FireBall>();
        unitInstance.GetComponent<Unit>().AddAbility<Heal>();
    }


    // Old instantiate! dont use
    //public void InstantiateUnit(bool player1, int x, int y) {
    //    GameObject unitInstance;
    //    unitInstance = Instantiate(unit, map.TileCoordToWorldCoord(x, y), Quaternion.identity) as GameObject;

    //    unitInstance.GetComponent<Unit>().init();
    //    unitInstance.transform.position = map.TileCoordToWorldCoord(x, y);

    //    if (player1) {
    //        unitInstance.GetComponent<Unit>().setColor(Color.blue);
    //        unitInstance.tag = "Player 1";
    //        unitInstance.name = "Player 1 Unit";
    //        unitInstance.GetComponent<Unit>().tileX = x;
    //        unitInstance.GetComponent<Unit>().tileY = y;
    //        GameManager.teamOne.Add(unitInstance);
    //    } else { // else player 2
    //        AIController = unitInstance.AddComponent<AI>();
    //        AIController.AIBehaviors = new List<AIBehaviors>();
    //        AIBehaviors newBehavior = unitInstance.AddComponent<Melee>();
    //        AIController.AIBehaviors.Add(newBehavior);
    //        newBehavior = unitInstance.AddComponent<Retreat>();
    //        AIController.AIBehaviors.Add(newBehavior);
    //        unitInstance.GetComponent<Unit>().setColor(Color.red);
    //        unitInstance.tag = "Player 2";
    //        unitInstance.name = "Player 2 Unit";
    //        unitInstance.GetComponent<Unit>().tileX = x;
    //        unitInstance.GetComponent<Unit>().tileY = y;
    //        GameManager.teamTwo.Add(unitInstance);
    //    }
    //}

    private void Start() {
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();

        // Testing only
        MakeArcher(GameManager.defendingTeam, 13, 3);
        MakeArcher(GameManager.defendingTeam, 12, 3);
        //MakeArcher(GameManager.defendingTeam, 11, 3);
        MakeArcher(!GameManager.defendingTeam, 11, 2);
        MakeMage(!GameManager.defendingTeam, 10, 2);
        //MakeArcher(!GameManager.defendingTeam, 10, 5);
        //MakeSwordsman(!GameManager.defendingTeam, 10, 5);

        //team 1
        //MakeSwordsman(GameManager.defendingTeam, 10, 1);
        //MakeSwordsman(GameManager.defendingTeam, 9, 1);
        //MakeArcher(GameManager.defendingTeam, 11, 0);
        //MakeMage(GameManager.defendingTeam, 9, 0);
        ////team 2
        //MakeSwordsman(!GameManager.defendingTeam, 1, 14);
        //MakeSwordsman(!GameManager.defendingTeam, 2, 14);
        //MakeArcher(!GameManager.defendingTeam, 0, 13);
        //MakeArcher(!GameManager.defendingTeam, 1, 13);
        //MakeMage(!GameManager.defendingTeam, 0, 14);

    }

    private void Update() {
        //testing
        //if (Input.GetKeyDown("p")) {
            //InstantiateUnit(true);
        //}
    }

}
