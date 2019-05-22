using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour {

    private struct AbilityHolder {
        public Ability ability;
        public Button abilityUIButton;
    }

    private List<AbilityHolder> abilitys;
    private int abilityCount;
    private GameObject centerPanel;
    private Button buttonPrefab;
    bool active;

    public void CleatUI() {
        if (active) {
            ToggleActive();
        }
    }

    public bool ToggleActive() {
        active = !active;

        if (active) AddToUI();
        else RemoveFromUI();

        return active;
    }

    private Button InstantiateButton() {
        return Instantiate(buttonPrefab);
    }

    private void AddToUI() {
        int offset = 80;
        int count = 0;
        foreach (AbilityHolder ability in abilitys) {
            //print("Ability added");
            ability.abilityUIButton.transform.SetParent(centerPanel.transform);
            ability.abilityUIButton.transform.localPosition = new Vector3(20 + offset * count, 50, 0);
            count++;
        }
    }

    private void RemoveFromUI() {
        foreach (AbilityHolder ability in abilitys) {
            ability.abilityUIButton.transform.SetParent(null);
        }
    }

    private void TaskOnClick(Ability ability) {
        if (ability.CanTargetFriendly() && GameManager.map.TargetedUnitCurrentPlayer != null) {
            ability.Target(GameManager.map.TargetedUnitCurrentPlayer);
        } else if (ability.CanTargetSelf()) {
            ability.Target(GameManager.map.SelectedUnitCurrentPlayer);
        } else if (GameManager.map.TargetedUnitEnemyPlayer == null) {
            print("No enemy selected");
        } else {
            print("atack reached");
            ability.Target(GameManager.map.TargetedUnitEnemyPlayer);
        }
    }

    public int GetCount() { return abilityCount; }

    public void Init() {
        abilitys = new List<AbilityHolder>();
        abilityCount = 0;
        centerPanel = GameObject.Find("Center Panel");
        buttonPrefab = Resources.Load("AbilityButton", typeof(Button)) as Button;
        active = false;
    }

    public void Add(Ability newAbility) {
        //Create new ability
        AbilityHolder newAbilityHolder = new AbilityHolder();
        newAbilityHolder.ability = newAbility;
        newAbilityHolder.abilityUIButton = InstantiateButton();
        newAbilityHolder.abilityUIButton.onClick.AddListener(delegate { TaskOnClick(newAbility); });

        //Set button
        Text[] buttonTxt = newAbilityHolder.abilityUIButton.GetComponentsInChildren<Text>() as Text[];
        buttonTxt[0].text = newAbility.GetName();
        buttonTxt[1].text = newAbility.GetInfo();


        //Add new ability to the abilitiys list
        abilitys.Add(newAbilityHolder);
        abilityCount++;
    }


    // Get & Can Methods for AI
    // To use input the index of the ability you want to get info on
    // If no ability at the index given then Get... methods will return -1 and the Can... methods return false
    public int GetStrength(int index) { // indes of ability
        if (abilitys.Count >= index) return abilitys[index].ability.GetStrength();
        return -1; // return -1 if index is greater than the number of elements in the abilitys list
    }
    public int GetRange(int index) { // indes of ability
        if (abilitys.Count >= index) return abilitys[index].ability.GetRange();
        return -1; // return -1 if index is greater than the number of elements in the abilitys list
    }
    public int GetAreaOfEffect(int index) { // indes of ability
        if (abilitys.Count >= index) return abilitys[index].ability.GetAreaOfEffect();
        return -1; // return -1 if index is greater than the number of elements in the abilitys list
    }
    public int GetCost(int index) { // indes of ability
        if (abilitys.Count >= index) return abilitys[index].ability.GetCost();
        return -1; // return -1 if index is greater than the number of elements in the abilitys list
    }


    public bool CanTargetEnemy(int index) { // indes of ability
        if (abilitys.Count >= index) return abilitys[index].ability.CanTargetEnemy();
        return false; // return false if index is greater than the number of elements in the abilitys list
    }
    public bool CanTargetFriendly(int index) { // indes of ability
        if (abilitys.Count >= index) return abilitys[index].ability.CanTargetFriendly();
        return false; // return false if index is greater than the number of elements in the abilitys list
    }
    public bool CanTargetSelf(int index) { // indes of ability
        if (abilitys.Count >= index) return abilitys[index].ability.CanTargetSelf();
        return false; // return false if index is greater than the number of elements in the abilitys list
    }
    public bool CanTargetTile(int index) { // indes of ability
        if (abilitys.Count >= index) return abilitys[index].ability.CanTargetTile();
        return false; // return false if index is greater than the number of elements in the abilitys list
    }

}

