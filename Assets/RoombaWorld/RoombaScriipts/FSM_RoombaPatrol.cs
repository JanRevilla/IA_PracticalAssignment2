using FSMs;
using Steerings;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "FSM_RoombaPatrol", menuName = "Finite State Machines/FSM_RoombaPatrol", order = 1)]
public class FSM_RoombaPatrol : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private ROOMBA_Blackboard blackboard;
    private GoToTarget goToTarget;

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

        State GoToTarget = new State("GoToTarget",
            () => { goToTarget.target = LocationHelper.RandomPatrolPoint(); }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );



        Transition ReachPoint = new Transition("ReachPoint",
            () => { return SensingUtils.DistanceToTarget(gameObject, goToTarget.target) <= blackboard.wanderPointReachDistance; }, // write the condition checkeing code in {}
            () => { goToTarget.target = LocationHelper.RandomPatrolPoint(); }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        AddState(GoToTarget);

        AddTransition(GoToTarget, ReachPoint, GoToTarget);


        initialState = GoToTarget; 

    }
}
