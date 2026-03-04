using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FMS_Clean", menuName = "Finite State Machines/FMS_Clean", order = 1)]
public class FMS_Clean : FiniteStateMachine
{
    private ROOMBA_Blackboard blackboard;
    private GoToTarget goToTarget;
    GameObject target;
    private float elapsedTime;

    public override void OnEnter()
    {
        blackboard = GetComponent<ROOMBA_Blackboard>(); 
        goToTarget = GetComponent<GoToTarget>();

        base.OnEnter(); 
    }

    public override void OnExit()
    {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        FiniteStateMachine ROOMBAPATROL = ScriptableObject.CreateInstance<FSM_RoombaPatrol>();

        State CLEANPOO = new State("Clean Poo",
            () => { }, 
            () => {
                target = SensingUtils.FindInstanceWithinRadius(gameObject, "POO", blackboard.pooDetectionRadius);
                goToTarget.target = target;                          
            }, 
            () => {
                Destroy(target); }
        );

        State SPINNING = new State("Spinnig",
            () => { blackboard.StartSpinning(); elapsedTime = 0; },
            () => { elapsedTime += Time.deltaTime; },
            () => { blackboard.StopSpinning(); }
        );

        State CLEANDUST = new State("Clean Dust",
            () => { },
            () => { goToTarget.target = SensingUtils.FindInstanceWithinRadius(gameObject, "DUST", blackboard.dustDetectionRadius); },
            () => { Destroy(goToTarget.target); }
        );

        Transition goToPoo = new Transition("Go To Poo",
            () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "POO", blackboard.pooDetectionRadius);                 
            }, 
            () => { } 
        );

        Transition goToDust = new Transition("Go To Dust",
            () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "DUST", blackboard.dustDetectionRadius); },
            () => { }
        );

        Transition stopCleaning = new Transition("Stop Cleaning",
            () => { return goToTarget.target == null; },
            () => { }
        );

        Transition stopSpinning = new Transition("Stop Spinnig",
            () => { return (elapsedTime > blackboard.pooCleaningTime); },
            () => { }
        );

        AddStates(ROOMBAPATROL,CLEANPOO,CLEANDUST, SPINNING);

        AddTransition(ROOMBAPATROL,goToPoo,CLEANPOO);
        AddTransition(ROOMBAPATROL, goToDust, CLEANDUST);
        AddTransition(CLEANDUST, stopCleaning, ROOMBAPATROL);
        AddTransition(CLEANPOO, stopCleaning, SPINNING);
        AddTransition(CLEANDUST, goToPoo, CLEANPOO);
        AddTransition(SPINNING, stopSpinning, ROOMBAPATROL);

        initialState = ROOMBAPATROL;

    }
}
