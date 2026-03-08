using FSMs;
using Steerings;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "FSM_RoombaPatrol", menuName = "Finite State Machines/FSM_RoombaPatrol", order = 1)]
public class FSM_RoombaPatrol : FiniteStateMachine
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

        State GOTOTARGET = new State("GOTOTARGET",
            () => { goToTarget.target = LocationHelper.RandomPatrolPoint(); }, 
            () => { }, 
            () => { }
        );



        Transition ReachPoint = new Transition("ReachPoint",
            () => { return SensingUtils.DistanceToTarget(gameObject, goToTarget.target) <= blackboard.wanderPointReachDistance; },
            () => { goToTarget.target = LocationHelper.RandomPatrolPoint(); }  
        );


        AddState(GOTOTARGET);

        AddTransition(GOTOTARGET, ReachPoint, GOTOTARGET);


        initialState = GOTOTARGET; 

    }
}
