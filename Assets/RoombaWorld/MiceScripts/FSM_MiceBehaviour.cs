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
    private SteeringContext steeringContext;


    public override void OnEnter()
    {
        blackboard = GetComponent<MOUSE_Blackboard>();
        goToTarget = GetComponent<GoToTarget>();
        steeringContext = GetComponent<SteeringContext>();
        blackboard.initSpeed = steeringContext.maxSpeed;
        blackboard.initAcc = steeringContext.maxAcceleration;

        base.OnEnter(); 
    }

    public override void OnExit()
    {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        State GOTORANDOMWALKABLELOCATION = new State("GOTORANDOMWALKABLELOCATION",
            () => { goToTarget.target = LocationHelper.RandomPatrolPoint(); },
            () => { }, 
            () => { }  
        );


        State DOESAPOO = new State ("DOESAPOO",
            () => {
                GameObject instance = Instantiate(blackboard.pooPrefab);
                instance.transform.position = gameObject.transform.position;
            },
            () => { }, 
            () => { }  
        );

        State GOTORANDOMEXITLOCATION = new State("GOTORANDOMEXITLOCATION",
            () => { goToTarget.target = LocationHelper.RandomEntryExitPoint();
            }, 
            () => { }, 
            () => { }   
        );

        State RUNFROMROOMBA = new State("RUNFROMROOMBA",
            () => { 
                goToTarget.target = LocationHelper.NearestExitPoint(gameObject);
                GetComponent<SpriteRenderer>().color = Color.green;
             
            }, 
            () => {
                steeringContext.maxSpeed = blackboard.initSpeed * 2;
                steeringContext.maxAcceleration = blackboard.initAcc * 4;
            }, 
            () => { steeringContext.maxSpeed = blackboard.initSpeed; steeringContext.maxAcceleration = blackboard.initAcc; }  
        );

        Transition RandomWalkableLocationNear = new Transition("RandomWalkableLocationNear",
            () => { return SensingUtils.DistanceToTarget(gameObject, goToTarget.target) <= 5f; },
            () => { }
        );

        Transition PooDone = new Transition("PooDone",
            () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "POO", 5f); },
            () => { }
        );

        Transition ExitLocationNear = new Transition("ExitLocationNear",
             () => { return SensingUtils.DistanceToTarget(gameObject, goToTarget.target) <= 5f; },
             () => { Destroy(gameObject); }
        );

        Transition CloseToRoomba = new Transition("CloseToRoomba",
             () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "ROOMBA", blackboard.roombaDetectionRadius); },
             () => { }
        );

        AddStates(GOTORANDOMWALKABLELOCATION,DOESAPOO,GOTORANDOMEXITLOCATION, RUNFROMROOMBA);

        AddTransition(GOTORANDOMWALKABLELOCATION,RandomWalkableLocationNear,DOESAPOO);
        AddTransition(DOESAPOO, PooDone, GOTORANDOMEXITLOCATION);
        AddTransition(GOTORANDOMEXITLOCATION, ExitLocationNear, GOTORANDOMEXITLOCATION);
        AddTransition(RUNFROMROOMBA, ExitLocationNear, GOTORANDOMEXITLOCATION);

        AddTransition(GOTORANDOMEXITLOCATION, CloseToRoomba, RUNFROMROOMBA);
        AddTransition(DOESAPOO, CloseToRoomba, RUNFROMROOMBA);
        AddTransition(GOTORANDOMWALKABLELOCATION, CloseToRoomba, RUNFROMROOMBA);

        initialState = GOTORANDOMWALKABLELOCATION;

    }
}
