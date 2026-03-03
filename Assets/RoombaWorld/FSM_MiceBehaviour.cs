using FSMs;
using UnityEngine;
using Steerings;


[CreateAssetMenu(fileName = "FSM_MiceBehaviour", menuName = "Finite State Machines/FSM_MiceBehaviour", order = 1)]
public class FSM_MiceBehaviour : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private MOUSE_Blackboard blackboard;
    private GoToTarget goToTarget;


  



    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<MOUSE_Blackboard>();
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
        State GoToRandomWalkableLocation = new State("GoToRandomWalkableLocation",
            () => { goToTarget.target = LocationHelper.RandomPatrolPoint(); }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );


        State DoesAPoo = new State ("DoesAPoo",
            () => {
                Debug.Log("Poo");
                //instance = Instantiate(blackboard.pooPrefab, gameObject.transform); ;
                GameObject instance = Instantiate(blackboard.pooPrefab);
                instance.transform.position = gameObject.transform.position;
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

        State GoToRandomExitLocation = new State("GoToRandomExitLocation",
            () => { goToTarget.target = LocationHelper.RandomEntryExitPoint();
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );




        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition RandomWalkableLocationNear = new Transition("RandomWalkableLocationNear",
            () => { return SensingUtils.DistanceToTarget(gameObject, goToTarget.target) <= 5f; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition PooDone = new Transition("PooDone",
            () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "POO", 5f); },
            () => { }
        );

        Transition ExitLocationNear = new Transition("ExitLocationNear",
             () => { return SensingUtils.DistanceToTarget(gameObject, goToTarget.target) <= 5f; },
             () => { Destroy(gameObject); }
        );



        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(GoToRandomWalkableLocation,DoesAPoo,GoToRandomExitLocation);

        AddTransition(GoToRandomWalkableLocation,RandomWalkableLocationNear,DoesAPoo);
        AddTransition(DoesAPoo, PooDone, GoToRandomExitLocation);
        AddTransition(GoToRandomExitLocation, ExitLocationNear, GoToRandomExitLocation);
        /* STAGE 4: set the initial state
         
        initialState = ... 

        

         */

        initialState = GoToRandomWalkableLocation;

    }
}
