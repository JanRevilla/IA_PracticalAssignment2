using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Charge", menuName = "Finite State Machines/FSM_Charge", order = 1)]
public class FSM_Charge : FiniteStateMachine
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
        FiniteStateMachine FSM_Clean = ScriptableObject.CreateInstance<FMS_Clean>();

        State GOTOCHARGE = new State("GOTOCHARGE",
            () => { goToTarget.target = GameObject.FindGameObjectWithTag("ENERGY"); }, 
            () => {  }, 
            () => {  }  
        );

        State CHARGING = new State("CHARGING",
            () => { blackboard.startRecharging(); }, 
            () => { }, 
            () => { blackboard.stopRecharging(); } 
        );
     
        Transition StationReached = new Transition("StationReached",
            () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "ENERGY", blackboard.chargingStationReachedRadius); },
            () => { }
        );

        Transition EnergyLow = new Transition("EnergyLow",
             () => { return blackboard.EnergyIsLow(); },
             () => { }
         );

        Transition ChargeFull = new Transition("ChargeFull",
            () => { return blackboard.EnergyIsFull(); },
            () => { }
        );
   
        AddStates(FSM_Clean, GOTOCHARGE,CHARGING);

        AddTransition(FSM_Clean, EnergyLow, GOTOCHARGE);
        AddTransition(GOTOCHARGE, StationReached, CHARGING);
        AddTransition(CHARGING  , ChargeFull,FSM_Clean);
       
        initialState = FSM_Clean;

    }
}
