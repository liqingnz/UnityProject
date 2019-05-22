using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AIBehaviors : MonoBehaviour {

    public int priority;
    public Ability[] AIAbilities;
    protected AIBehaviors()
    {
        priority = 0;
    }
    protected AIBehaviors(int i)
    {
        priority = i;
    }

    protected void Start()
    {
        AIAbilities = GetComponents<Ability>();
    }
    // Evaluate current condition
    protected int Evaluation()
    {

        return 0;
    }

    abstract public void ResetPriority(int newPriority);

    // AI basic moves
    abstract public void Move(int distance = 1);
    abstract public void Move(GameObject go, int distance = 1);
    abstract public void Attack(GameObject target);
    abstract public void Attack(GameObject target, Ability castAbility);
}

public class Retreat : AIBehaviors
{

    protected Retreat()
    {
        priority = 5;
    }
    protected Retreat(int i)
    {
        priority = i;
    }
    
    public override void ResetPriority(int newPriority)
    {
        priority = newPriority;
    }

    public override void Move(int distance = 5)
    {
        if(GameManager.defendingTeam)
            gameObject.GetComponent<Unit>().GeneratePathTo(11, GameManager.boardHeight - 1);
        else
            gameObject.GetComponent<Unit>().GeneratePathTo(11, 0);
    }
    public override void Move(GameObject target, int distance = 5)
    {
        Move();
    }

    public override void Attack(GameObject target)
    {
        if (target != null)
            this.gameObject.GetComponent<Unit>().atack.Target(target);
    }
    public override void Attack(GameObject target, Ability castAbility)
    {
        if (target != null)
            castAbility.Target(target);
    }
}

public class AIMelee : AIBehaviors
{

    protected AIMelee()
    {
        priority = 2;
    }
    protected AIMelee(int i)
    {
        priority = i;
    }

    public override void ResetPriority(int newPriority)
    {
        priority = newPriority;
    }

    public override void Move(int distance = 1)
    {
        List<GameObject> teamOne = GameManager.teamOne;
        int randomNumber = Random.Range(0, teamOne.Count);
        GameObject target = teamOne[randomNumber];
        if (AIAbilities[1].GetRange() > Vector3.Distance(transform.position, target.transform.position))
            return;
        print(target);
        List<Node> calculatedPositions = AIAlgorithms.AvailableTiles(target, distance);
        randomNumber = Random.Range(0, calculatedPositions.Count);
        AIAlgorithms.occupiedTiles.Add(calculatedPositions[randomNumber]);
        this.gameObject.GetComponent<Unit>().GeneratePathTo(calculatedPositions[randomNumber].x, calculatedPositions[randomNumber].y);
    }
    public override void Move(GameObject target, int distance = 1)
    {
        if (AIAbilities[1].GetRange() > Vector3.Distance(transform.position, target.transform.position) || target == null)
            return;
        List<Node> calculatedPositions = AIAlgorithms.AvailableTiles(target, distance);
        int randomNumber = Random.Range(0, calculatedPositions.Count);
        AIAlgorithms.occupiedTiles.Add(calculatedPositions[randomNumber]);
        this.gameObject.GetComponent<Unit>().GeneratePathTo(calculatedPositions[randomNumber].x, calculatedPositions[randomNumber].y);
        print(this.gameObject + " is moving towards " + target);
    }

    public override void Attack(GameObject target)
    {
        if (target == null)
            return;
        this.gameObject.GetComponent<Unit>().atack.Target(target);
        print(this.gameObject + " is attacking " + target);
    }
    public override void Attack(GameObject target, Ability castAbility)
    {
        if (target == null)
            return;
        this.gameObject.GetComponent<Unit>().atack.Target(target);
    }
}

public class AIRange : AIBehaviors
{
    protected AIRange()
    {
        priority = 2;
    }
    protected AIRange(int i)
    {
        priority = i;
    }

    public override void ResetPriority(int newPriority)
    {
        priority = newPriority;
    }

    public override void Move(int distance = 2)
    {
        List<GameObject> teamOne = GameManager.teamOne;
        int randomNumber = Random.Range(0, teamOne.Count);
        GameObject target = teamOne[randomNumber];
        if (AIAbilities[1].GetRange() > Vector3.Distance(transform.position, target.transform.position))
            return;
        List<Node> calculatedPositions = AIAlgorithms.AvailableTiles(target, distance);
        randomNumber = Random.Range(0, calculatedPositions.Count);
        AIAlgorithms.occupiedTiles.Add(calculatedPositions[randomNumber]);
        this.gameObject.GetComponent<Unit>().GeneratePathTo(calculatedPositions[randomNumber].x, calculatedPositions[randomNumber].y);
    }
    public override void Move(GameObject target, int distance = 2)
    {
        if (AIAbilities[1].GetRange() > Vector3.Distance(transform.position, target.transform.position) || target == null)
            return;
        List<Node> calculatedPositions = AIAlgorithms.AvailableTiles(target, distance);
        int randomNumber = Random.Range(0, calculatedPositions.Count);
        AIAlgorithms.occupiedTiles.Add(calculatedPositions[randomNumber]);
        this.gameObject.GetComponent<Unit>().GeneratePathTo(calculatedPositions[randomNumber].x, calculatedPositions[randomNumber].y);
        print(this.gameObject + " is moving towards " + target);         
    }

    public override void Attack(GameObject target)
    {
        if (target != null)
            this.gameObject.GetComponent<Unit>().atack.Target(target);
        print(this.gameObject + " is attacking " + target);
    }
    public override void Attack(GameObject target, Ability castAbility)
    {
        if (target != null)
            castAbility.Target(target);
    }
}

public class AIHeal : AIBehaviors
{
    GameObject healTarget;

    protected AIHeal()
    {
        priority = 2;
    }
    protected AIHeal(int i)
    {
        priority = i;
    }

    public override void ResetPriority(int newPriority)
    {
        priority = newPriority;
    }

    public override void Move(int distance = 2)
    {
        List<Node> calculatedPositions;
        int randomNumber;

        List<GameObject> teamOne = GameManager.teamTwo;
        foreach(GameObject target in teamOne)
        {
            if (target.GetComponent<Unit>().UnitHealthState == Unit.HealthState.low)
            {
                healTarget = target;
                if (GetComponent<Heal>().GetRange() > Vector3.Distance(transform.position, target.transform.position))
                    return;
                calculatedPositions = AIAlgorithms.AvailableTiles(target, distance);
                randomNumber = Random.Range(0, calculatedPositions.Count);
                AIAlgorithms.occupiedTiles.Add(calculatedPositions[randomNumber]);
                this.gameObject.GetComponent<Unit>().GeneratePathTo(calculatedPositions[randomNumber].x, calculatedPositions[randomNumber].y);
                return;
            }
            if (target.GetComponent<Unit>().UnitHealthState == Unit.HealthState.damaged)
            {
                healTarget = target;
                if (GetComponent<Heal>().GetRange() > Vector3.Distance(transform.position, target.transform.position))
                    return;
                calculatedPositions = AIAlgorithms.AvailableTiles(target, distance);
                randomNumber = Random.Range(0, calculatedPositions.Count);
                AIAlgorithms.occupiedTiles.Add(calculatedPositions[randomNumber]);
                this.gameObject.GetComponent<Unit>().GeneratePathTo(calculatedPositions[randomNumber].x, calculatedPositions[randomNumber].y);
                return;
            }
        }

        teamOne = GameManager.teamOne;
        randomNumber = Random.Range(0, teamOne.Count);
        GameObject followingTarget = teamOne[randomNumber];
        if (AIAbilities[1].GetRange() > Vector3.Distance(transform.position, followingTarget.transform.position))
            return;
        calculatedPositions = AIAlgorithms.AvailableTiles(followingTarget, distance);
        randomNumber = Random.Range(0, calculatedPositions.Count);
        AIAlgorithms.occupiedTiles.Add(calculatedPositions[randomNumber]);
        this.gameObject.GetComponent<Unit>().GeneratePathTo(calculatedPositions[randomNumber].x, calculatedPositions[randomNumber].y);
    }
    public override void Move(GameObject go, int distance = 2)
    {
        Move(distance);
    }

    public override void Attack(GameObject go = null)
    {
        if(healTarget != null)
        {
            print(this.gameObject + " is healing " + healTarget);
            gameObject.GetComponent<Heal>().Target(healTarget);

        }
    }
    public override void Attack(GameObject target, Ability castAbility)
    {
        if (healTarget != null)
        {
            print(this.gameObject + " is healing " + healTarget);
            gameObject.GetComponent<Heal>().Target(healTarget);
        }
        //castAbility.Target(target);
    }
}
