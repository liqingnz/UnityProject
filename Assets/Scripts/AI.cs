using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    
    //MapGenerator map;
    public List<GameObject> teamOne = GameManager.teamOne;
    public List<GameObject> teamTwo = GameManager.teamTwo;

    // Added during unit instantiation
    public List<AIBehaviors> AIBehaviors;

    private Unit unitInfo;
    public Ability[] AIAbilities;

    // Could use unitHealthState instead
    public int retreatingHP = 2;

    // Use this for initialization
    void Start ()
    {

        // Reference to the script MapGenerator
        //map = GetComponent<MapGenerator>();

        // Reference to the Unit class
        unitInfo = GetComponent<Unit>();
        AIAbilities = GetComponents<Ability>();
    }

    public void CheckHPForRetreat(int retreatingHP)
    {
        // Work in progress
        if (unitInfo.remainingPlayerHealth <= retreatingHP)
        {
            print(">>>>>>>>>>>>>>>>>>health too low");
            gameObject.GetComponent<Retreat>().priority = 0;
        }
    }

    public void AIMoves()
    {
        // Clear the UI during AI moves
        GameManager.ClearAbilities();
        // If the unit hp fall below certain value, prioritise retreat
        CheckHPForRetreat(retreatingHP);
        // Sort the behaviors by their priority
        AIBehaviors.Sort((x, y) => x.priority.CompareTo(y.priority));
        //print("The name of the AI behavior is " + AIBehaviors[0].priority);

        // Move toward one unit(random) and attack
        //int targetNumber = Random.Range(0, teamOne.Count);

        // Does not actually follow the priority, need fix
        Ability castingAbility = AIAbilities[1];
        //if(Vector3.Distance(transform.position, teamOne[targetNumber].transform.position) > castingAbility.GetRange())

        print("AI moving<<<<<<<<<<<<");
        AIBehaviors[0].Move(teamOne[0], castingAbility.GetRange());
        print("<<<<<<<<<<AI moved>>>>>>>>>>>");
        AIBehaviors[0].Attack(teamOne[0], castingAbility);
        print("AI attacked>>>>>>>>>>>");
        //print("the target is: " + teamTwo[targetNumber]);


        //foreach (AIBehaviors moves in AIBehaviors)
        //{
        //    moves.Move(teamOne[targetNumber]);
        //    print("the target is: " + teamTwo[targetNumber]);
        //    moves.Attack(teamOne[targetNumber]);
        //}
    }
    
}
