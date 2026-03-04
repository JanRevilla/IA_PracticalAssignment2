using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FMS_Clean", menuName = "Finite State Machines/FMS_Clean", order = 1)]
public class FMS_Clean : FiniteStateMachine
{
    private ROOMBA_Blackboard blackboard;
    private GoToTarget goToTarget;

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
            () => { goToTarget.target = SensingUtils.FindInstanceWithinRadius(gameObject, "POO", blackboard.pooDetectionRadius); }, 
            () => { blackboard.StartSpinning();
                    Destroy(goToTarget.target); 
            }  
        );

        State CLEANDUST = new State("Clean Dust",
            () => { },
            () => { },
            () => { }
        );

        Transition goToPoo = new Transition("Go To Poo",
            () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "POO", blackboard.pooDetectionRadius);                 
            }, 
            () => { } 
        );

        /*Transition goToDust = new Transition("Go To Dust",
            () => { },
            () => { }
        );*/

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(ROOMBAPATROL,CLEANPOO,CLEANDUST);

        AddTransition(ROOMBAPATROL,goToPoo,CLEANPOO);

        initialState = ROOMBAPATROL;

    }
}
