using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {
    public int tileX;
    public int tileY;


    public int movementPoints = 5;
    public float remainingMovementPoints = 5;
    public int playerHealth = 5;
    public int remainingPlayerHealth = 5;

    // For futhur use
    public enum HealthState { full, damaged, low }
    public HealthState UnitHealthState = HealthState.full;

    public List<Node> currentPath;
    public int[] pathEnd;

    public bool thereIsAPath;
    public GameObject deathParticles;
    //public Renderer playerRenderer;
    public Color playerOriginalColor;

    //public float percent;
    //public float attackSpeed = 5;
    public Image hpBar, mpBar;
    public Canvas unitCanvas;
    Transform unitCanvasTransform;


    GameObject standingTile;
    public MapGenerator map;
    CanvasController canvas;

    Text unitInfoDisplay;
    public Ability atack;
    public AbilityController abilitys;

    
    IEnumerator Flash() {
        for(int i=0; i<5; i++) {
            print("Damage!");
            setColor(Color.gray);
            yield return new WaitForSeconds(0.1f);
            if(tag.Contains("1")) {
                setColor(Color.blue);
            } else {
                setColor(Color.red);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetMovementPoints(int mpMax) {
        movementPoints = mpMax;
        remainingMovementPoints = mpMax;
    }
    public void SetHelth(int hpMax) {
        remainingPlayerHealth = hpMax;
        playerHealth = hpMax;
    }
    public int getMovementPoints() {
        return (int)remainingMovementPoints;
    }
    public int getHelth() {
        return remainingPlayerHealth;
    }
    public int GetX() { return tileX; }
    public int GetY() { return tileY; }

    public void ChangeHelth(int hpMod) {
        remainingPlayerHealth += hpMod;
        if (remainingPlayerHealth > playerHealth) {
            remainingPlayerHealth = playerHealth;
        } else if (0 >= remainingPlayerHealth) Die();
        else StartCoroutine(Flash());//unit flashes when taking damage
    }
    public bool ChangeMovementPoints(int mpMod) {
        remainingMovementPoints += mpMod;
        if (remainingMovementPoints > movementPoints) remainingMovementPoints = movementPoints;
        else if (0 > remainingMovementPoints) {
            remainingMovementPoints -= mpMod;
            return false;
        }
        return true;
    }
    public int decrementMp() {
        if (remainingMovementPoints == 0) { return -1; }
        return (int)remainingMovementPoints--;
    }


    private void UpdateUnitInfoDisplay() {
        unitInfoDisplay.text = "Hp: " + remainingPlayerHealth + "\n" + "Mp: " + remainingMovementPoints;

        if(!(remainingPlayerHealth < 1)) hpBar.rectTransform.localScale = new Vector3(1f / (float)playerHealth * (float)remainingPlayerHealth, 1, 1);
        else hpBar.rectTransform.localScale = new Vector3(0, 1, 1);

        if(!(remainingMovementPoints < 1f)) mpBar.rectTransform.localScale = new Vector3(1f / (float)movementPoints * remainingMovementPoints, 1, 1);
        else mpBar.rectTransform.localScale = new Vector3(0, 1, 1);
    }

    public void AddDefaultAbility<T>() {
        atack = (Ability)gameObject.AddComponent(typeof(T));
        // Not sure whats the point of this, bugs the game, wait for confirmation
        //transform.Translate(map.TileCoordToWorldCoord(tileX, tileY));
        atack.Init();
        abilitys.Add(atack);
    }
    public void AddAbility<T>() {
        Ability newAbility = (Ability)gameObject.AddComponent(typeof(T));
        newAbility.Init();
        abilitys.Add(newAbility);
    }


    public void setStandingTile(GameObject tile) {
        standingTile = tile;
    }

    public void init() { // initilise new instance of unit
        pathEnd = null;
        //active = true;
        currentPath = null;
        thereIsAPath = false;
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();
        unitInfoDisplay = this.GetComponentInChildren<Text>();
        canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<CanvasController>();
        unitCanvasTransform = transform;

        abilitys = Instantiate(Resources.Load("AbilityControllerHolder", typeof(AbilityController)) as AbilityController);
        abilitys.Init();
    }

    public void setColor(Color color) {
        GetComponentInChildren<Renderer>().material.color = color;
        playerOriginalColor = color;
    }

    private void drawDebugline() {
        if (currentPath != null) {
            int currentNode = 0;
            thereIsAPath = true;

            while (currentNode < currentPath.Count - 1) {
                Vector3 start = map.TileCoordToWorldCoord(currentPath[currentNode].x, currentPath[currentNode].y) - new Vector3(0, 0.4f, 0);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[currentNode + 1].x, currentPath[currentNode + 1].y) - new Vector3(0, 0.4f, 0);

                Debug.DrawLine(start, end, Color.red);
                currentNode++;
            }
        }
    }

    private void Update() {
        if (standingTile) {
            standingTile.GetComponent<Hightlight>().restricted = false;
        }

        if (remainingPlayerHealth == playerHealth)
        {
            UnitHealthState = HealthState.full;
        }
        if (remainingPlayerHealth < playerHealth)
        {
            UnitHealthState = HealthState.damaged;
        }
        if (remainingPlayerHealth < 3)
        {
            UnitHealthState = HealthState.low;
        }
        // Draw our debug line showing the pathfinding!
        // NOTE: This won't appear in the actual game view.
        drawDebugline();
        // Update the unit info display
        UpdateUnitInfoDisplay();



        /*  Have we moved our visible piece close enough to the target tile that we can
            advance to the next step in our pathfinding? */
        if (Vector3.Distance(transform.position, map.TileCoordToWorldCoord(tileX, tileY)) < 0.1f) {
            AdvancePathing();
            //print("is moving");
        }

        // Smoothly animate towards the correct map tile.

        transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord(tileX, tileY), 5f * Time.deltaTime);
        RaycastHit hitInfo;
        Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out hitInfo, 5f);
        //print(hitInfo.transform.name);
        if (hitInfo.transform)
        {
            standingTile = hitInfo.transform.gameObject;
            standingTile.GetComponent<Hightlight>().restricted = true;

        }
        canvas.setTxt();
    }

    // Advances our pathfinding progress by one tile.
    void AdvancePathing() {
        if (currentPath == null || remainingMovementPoints < 1) return;

        // Triggers bug, not sure whats causing it
        //if (remainingMovementPoints < map.CostToEnterTile(currentPath[1].x, currentPath[1].y))
        //    return;
        transform.position = map.TileCoordToWorldCoord(tileX, tileY); // Teleport us to our correct "current" position, in case we haven't finished the animation yet.

        // Make sure the unit has enough movementpoint to reach its destination
        remainingMovementPoints -= map.CostToEnterTile(currentPath[1].x, currentPath[1].y); // Get cost from current tile to next tile

        // Move us to the next tile in the sequence
        tileX = currentPath[1].x;
        tileY = currentPath[1].y;

        // Let the unit to face the direction its moving to
        transform.forward = map.TileCoordToWorldCoord(tileX, tileY) - transform.position;

        if (unitCanvas.transform.eulerAngles.y != 0) {
            unitCanvas.transform.Rotate(new Vector3(0, -unitCanvas.transform.eulerAngles.y, 0), Space.World);
        }

        // Remove the old "current" tile from the pathfinding list
        currentPath.RemoveAt(0);

        if (currentPath.Count == 1) { // Final destination reached
            currentPath = null;
            pathEnd = null;
        }
    }

    public void GeneratePathTo(int x, int y)
    {
        pathEnd = new int[2];
        pathEnd[0] = x; pathEnd[1] = y;
        map.GeneratePathTo(x, y, this.gameObject);
    }
    public void GeneratePathTo(int x, int y, GameObject go)
    {
        pathEnd = new int[2];
        pathEnd[0] = x; pathEnd[1] = y;
        map.GeneratePathTo(x, y, go);
    }

    public void NextTurn() {
        // Make sure to wrap-up any outstanding movement left over.
        if(currentPath != null) { // do this incase enemy has moved into path
            map.SelectedUnitCurrentPlayer = this.gameObject;
            GeneratePathTo(pathEnd[0],pathEnd[1]);
        }
        while (currentPath != null && remainingMovementPoints > 0) AdvancePathing();
        // Reset our available movement points.
        remainingMovementPoints = movementPoints;
    }

    public void Die() {
        GameObject go = Instantiate(deathParticles, transform.position, Quaternion.identity);
        go.GetComponent<Renderer>().material.color = playerOriginalColor;
        int index = 0;
        if (map.SelectedUnitCurrentPlayer != null && map.SelectedUnitCurrentPlayer.GetComponent<Unit>() == this) {
            map.SelectedUnitCurrentPlayer.GetComponent<Unit>().abilitys.ToggleActive();
            map.SelectedUnitCurrentPlayer = null;
        }
        if (map.TargetedUnitCurrentPlayer != null && map.TargetedUnitCurrentPlayer.GetComponent<Unit>() == this) map.TargetedUnitCurrentPlayer = null;
        if (map.TargetedUnitEnemyPlayer != null && map.TargetedUnitEnemyPlayer.GetComponent<Unit>() == this) map.TargetedUnitEnemyPlayer = null;

        if (tag == "Player 1") {
            foreach (GameObject player in GameManager.teamOne) {
                if (player.GetComponent<Unit>() == this) {
                    //GameManager.teamOne.Remove(GameManager.teamOne[index]);
                    GameManager.teamOne.RemoveAt(index);
                    print("Removing...");
                    break;
                }
                index++;
            }
        } else if (tag == "Player 2") {
            foreach (GameObject player in GameManager.teamTwo) {
                if (player.GetComponent<Unit>() == this) {
                    //GameManager.teamTwo.Remove(GameManager.teamOne[index]);
                    GameManager.teamTwo.RemoveAt(index);
                    print("Removing...");
                    break;
                }
                index++;
            }
        }

        standingTile.GetComponent<Hightlight>().restricted = false;
        Destroy(this.gameObject);
    }

}
