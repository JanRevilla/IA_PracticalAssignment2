using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Charge", menuName = "Finite State Machines/FSM_Charge", order = 1)]
public class FSM_Charge : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
      * states and transitions and/or set in OnEnter or used in OnExit 
      * For instance: steering behaviours, blackboard, ...*/

    private ROOMBA_Blackboard blackboard;
    private GoToTarget goToTarget;
    private GameObject chargingStation;


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<ROOMBA_Blackboard>();
        goToTarget = GetComponent<GoToTarget>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

        FiniteStateMachine FSM_Clean = ScriptableObject.CreateInstance<FMS_Clean>();


        State GOTOCHARGE = new State("GOTOCHARGE",
            () => { goToTarget.target = GameObject.FindGameObjectWithTag("ENERGY"); }, // write on enter logic inside {}
            () => {  }, // write in state logic inside {}
            () => {  }  // write on exit logic inisde {}  
        );

        State CHARGING = new State("CHARGING",
            () => { blackboard.startRecharging(); }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { blackboard.stopRecharging(); }  // write on exit logic inisde {}  
        );
        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition StationReached = new Transition("StationReached",
            () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "ENERGY", blackboard.chargingStationReachedRadius); },// write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition EnergyLow = new Transition("EnergyLow",
             () => { return blackboard.EnergyIsLow(); },
             () => { }
         );

        Transition ChargeFull = new Transition("ChargeFull",
            () => { return blackboard.EnergyIsFull(); }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(FSM_Clean, GOTOCHARGE,CHARGING);

        AddTransition(FSM_Clean, EnergyLow, GOTOCHARGE);
        AddTransition(GOTOCHARGE, StationReached, CHARGING);
        AddTransition(CHARGING  , ChargeFull,FSM_Clean);
        /* STAGE 4: set the initial state
         
        initialState = ... 

         */



        initialState = FSM_Clean;

    }
}
