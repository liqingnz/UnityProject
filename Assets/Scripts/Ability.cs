using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


abstract public class Ability : MonoBehaviour {
    protected int strength;
    protected int range;
    protected int areaOfEffect;
    protected int cost;

    protected bool canTargetEnemy;
    protected bool canTargetFriendly;
    protected bool canTargetSelf;
    protected bool canTargetTile;



    //protected methods
    abstract protected void ApplyToTarget(GameObject target);
    abstract protected bool PayCost();
    abstract protected bool CheckRange(GameObject target);

    //public methods
    abstract public void Init(); //initilise new unit
    abstract public bool Target(GameObject target); //use to apply ability to target
    abstract public string GetName();
    abstract public string GetInfo();

    public int GetStrength() { return strength; }
    public int GetRange() { return range; }
    public int GetAreaOfEffect() { return areaOfEffect; }
    public int GetCost() { return cost; }

    public bool CanTargetEnemy() { return canTargetEnemy; }
    public bool CanTargetFriendly() { return canTargetFriendly; }
    public bool CanTargetSelf() { return canTargetSelf; }
    public bool CanTargetTile() { return canTargetTile; }
}


//Swordsman Abilitys
public class SwordStrike : Ability {

    protected override void ApplyToTarget(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        enemy.ChangeHelth(-strength);
    }

    protected override bool PayCost() {
        if (GetComponent<Unit>() == null) { print("Unit Not Set"); } else { print("Unit Set"); }

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }

    protected override bool CheckRange(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();

        if (GetComponent<Unit>() == null) print("GetComponent<Unit>() = null");
        if (enemy == null) print("enemy.tileX = null");

        int xDiff = Math.Abs(GetComponent<Unit>().tileX - enemy.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - enemy.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range");
        return false;
    }


    public override void Init() {
        strength = 3;
        range = 1;
        areaOfEffect = 0;
        cost = 2;
        canTargetEnemy = true;
        canTargetFriendly = false;
        canTargetSelf = false;
        canTargetTile = false;
    }

    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        //Return the name of this Ability to be displayed in Ui
        return "Sword Strike";
    }

    public override string GetInfo() {
        return "Damage: " + strength + "\n"
                + "Range: Meilei" + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }

}

public class SwordSpin : Ability {

    protected override void ApplyToTarget(GameObject target) {
        List<GameObject> enemy = new List<GameObject>();
        print(target.tag);
        if (target.tag == "Player 2") enemy.AddRange(GameManager.teamTwo);
        else enemy.AddRange(GameManager.teamOne);

        int x = GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>().GetX();
        int y = GameManager.map.SelectedUnitCurrentPlayer.GetComponent<Unit>().GetY();
        foreach (GameObject unit in enemy) {
            int unitX, unitY;
            unitX = unit.GetComponent<Unit>().GetX();
            unitY = unit.GetComponent<Unit>().GetY();
            if (Math.Abs(x-unitX)<=1 && Math.Abs(y - unitY) <= 1) unit.GetComponent<Unit>().ChangeHelth(-strength);
        }
    }

    protected override bool PayCost() {
        //if (GetComponent<Unit>() == null) { print("Unit Not Set"); } else { print("Unit Set"); }

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }

    protected override bool CheckRange(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        int xDiff = Math.Abs(GetComponent<Unit>().tileX - enemy.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - enemy.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range");
        return false;
    }

    public override void Init() {
        strength = 2;
        range = 1;
        areaOfEffect = 1;
        cost = 4;
        canTargetEnemy = true;
        canTargetFriendly = false;
        canTargetSelf = false;
        canTargetTile = false;
    }

    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        return "Sword Spin";
    }

    public override string GetInfo() {
        return "Damage: " + strength + "\n"
                + "Range: " + range + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }
}


//Mage Abilitys
public class StaffStrike : Ability {

    protected override void ApplyToTarget(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        enemy.ChangeHelth(-strength);
    }

    protected override bool PayCost() {
        if (GetComponent<Unit>() == null) { print("Unit Not Set"); } else { print("Unit Set"); }

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }

    protected override bool CheckRange(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        int xDiff = Math.Abs(GetComponent<Unit>().tileX - enemy.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - enemy.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range");
        return false;
    }


    public override void Init() {
        strength = 1;
        range = 1;
        areaOfEffect = 0;
        cost = 2;
        canTargetEnemy = true;
        canTargetFriendly = false;
        canTargetSelf = false;
        canTargetTile = false;
    }

    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        //Return the name of this Ability to be displayed in Ui
        return "Staff Strike";
    }

    public override string GetInfo() {
        return "Damage: " + strength + "\n"
                + "Range: Meilei" + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }

}

public class FireBall : Ability {

    protected override void ApplyToTarget(GameObject target) {
        double x = target.GetComponent<Unit>().GetX();
        double y = target.GetComponent<Unit>().GetY();
        List<GameObject> allUnits = new List<GameObject>();
        allUnits.AddRange(GameManager.teamOne);
        allUnits.AddRange(GameManager.teamTwo);

        foreach (GameObject unit in allUnits) {
            double unitX, unitY, distance;
            unitX = unit.GetComponent<Unit>().GetX();
            unitY = unit.GetComponent<Unit>().GetY();
            distance = Math.Sqrt(Math.Pow(unitX-x, 2.0) + Math.Pow(unitY-y, 2.0));
            if ((distance - (double)areaOfEffect) <= 0) {
                unit.GetComponent<Unit>().ChangeHelth(-strength);
            } else print("Safe");
        }
    }

    protected override bool PayCost() {
        if (GetComponent<Unit>() == null) { print("Unit Not Set"); } else { print("Unit Set"); }

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }

    protected override bool CheckRange(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        int xDiff = Math.Abs(GetComponent<Unit>().tileX - enemy.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - enemy.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range");
        return false;
    }

    public override void Init() {
        strength = 2;
        range = 8;
        areaOfEffect = 2;
        cost = 5;
        canTargetEnemy = true;
        canTargetFriendly = false;
        canTargetSelf = false;
        canTargetTile = true;
    }

    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        return "Fire Ball";
    }

    public override string GetInfo() {
        return "Damage: " + strength + "\n"
                + "Range: " + range + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }
}

public class Heal : Ability {

    protected override void ApplyToTarget(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        enemy.ChangeHelth(strength);
    }
    protected override bool PayCost() {
        if (GetComponent<Unit>() == null) { print("Error: Unit Not Set"); } else { print("Unit Set"); } // testing

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }
    protected override bool CheckRange(GameObject target) {
        Unit patient = target.GetComponent<Unit>();
        int xDiff = Math.Abs(GetComponent<Unit>().tileX - patient.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - patient.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range");
        return false;
    }

    public override void Init() {
        strength = 3;
        range = 8;
        areaOfEffect = 0;
        cost = 4;
        canTargetEnemy = false;
        canTargetFriendly = true;
        canTargetSelf = true;
        canTargetTile = false;
    }
    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        return "Heal";
    }

    public override string GetInfo() {
        return "Healing: " + strength + "\n"
                + "Range: " + range + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }
}


//Archer Abilitys
public class BowStirke : Ability {

    protected override void ApplyToTarget(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        enemy.ChangeHelth(-strength);
    }

    protected override bool PayCost() {
        if (GetComponent<Unit>() == null) { print("Unit Not Set"); } else { print("Unit Set"); }

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }

    protected override bool CheckRange(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        int xDiff = Math.Abs(GetComponent<Unit>().tileX - enemy.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - enemy.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range for the ability");
        return false;
    }


    public override void Init() {
        strength = 1;
        range = 1;
        areaOfEffect = 0;
        cost = 2;
        canTargetEnemy = true;
        canTargetFriendly = false;
        canTargetSelf = false;
        canTargetTile = false;
    }

    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        //Return the name of this Ability to be displayed in Ui
        return "Bow Strike";
    }

    public override string GetInfo() {
        return "Damage: " + strength + "\n"
                + "Range: Meilei" + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }

}

public class MagicBolt : Ability {
    protected override void ApplyToTarget(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        enemy.ChangeHelth(-strength);
    }

    protected override bool PayCost() {
        if (GetComponent<Unit>() == null) { print("Unit Not Set"); } else { print("Unit Set"); }

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }

    protected override bool CheckRange(GameObject target) {
        Unit patient = target.GetComponent<Unit>();
        int xDiff = Math.Abs(GetComponent<Unit>().tileX - patient.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - patient.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range");
        return false;
    }

    public override void Init() {
        strength = 2;
        range = 5;
        areaOfEffect = 0;
        cost = 3;
        canTargetEnemy = true;
        canTargetFriendly = false;
        canTargetSelf = false;
        canTargetTile = false;
    }
    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        return "Magic Bolt";
    }

    public override string GetInfo() {
        return "Damage: " + strength + "\n"
                + "Range: " + range + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }
}

public class QuickShot : Ability {
    protected override void ApplyToTarget(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        enemy.ChangeHelth(-strength);
    }

    protected override bool PayCost() {
        if (GetComponent<Unit>() == null) { print("Unit Not Set"); } else { print("Unit Set"); }

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }

    protected override bool CheckRange(GameObject target) {
        Unit patient = target.GetComponent<Unit>();
        int xDiff = Math.Abs(GetComponent<Unit>().tileX - patient.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - patient.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range");
        return false;
    }

    public override void Init() {
        strength = 2;
        range = 6;
        areaOfEffect = 0;
        cost = 3;
        canTargetEnemy = true;
        canTargetFriendly = false;
        canTargetSelf = false;
        canTargetTile = false;
    }
    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        return "Quick Shot";
    }

    public override string GetInfo() {
        return "Damage: " + strength + "\n"
                + "Range: " + range + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }
}

public class AimedShot : Ability {
    protected override void ApplyToTarget(GameObject target) {
        Unit enemy = target.GetComponent<Unit>();
        enemy.ChangeHelth(-strength);
    }

    protected override bool PayCost() {
        if (GetComponent<Unit>() == null) { print("Unit Not Set"); } else { print("Unit Set"); }

        return GetComponent<Unit>().ChangeMovementPoints(-cost);
    }

    protected override bool CheckRange(GameObject target) {
        Unit patient = target.GetComponent<Unit>();
        int xDiff = Math.Abs(GetComponent<Unit>().tileX - patient.tileX);
        int yDiff = Math.Abs(GetComponent<Unit>().tileY - patient.tileY);

        if (xDiff <= range && yDiff <= range) {
            return true;
        }
        //print("Unit out of range");
        return false;
    }

    public override void Init() {
        strength = 3;
        range = 9;
        areaOfEffect = 0;
        cost = 5;
        canTargetEnemy = true;
        canTargetFriendly = false;
        canTargetSelf = false;
        canTargetTile = false;
    }
    public override bool Target(GameObject target) {
        if (CheckRange(target)) {
            if (PayCost()) {
                ApplyToTarget(target);
                return true;
            }
        }
        return false;
    }

    public override string GetName() {
        return "Aimed Shot";
    }

    public override string GetInfo() {
        return "Damage: " + strength + "\n"
                + "Range: " + range + "\n"
                + "Cost: " + cost + "\n"
                + "Conditions not met";
    }
}