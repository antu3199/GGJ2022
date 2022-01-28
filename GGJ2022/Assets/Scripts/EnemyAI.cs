using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateMachine {
    // NON AGGRO STATES
    LOOK,
    TURN,
    WALK,
    DAMAGED,
    // AGGRO STATES
    CHASE,
    ATTACK
}

// Possible State Transitions
//
//                -------------------|
//                |                  |
//                v                  |
// --> LOOK --> TURN --> WALK     DAMAGED
//      ^        |        |          ^
//      |        v        |          |
//      |----> CHASE <----|         ALL  
//               ^
//               |
//               v
//             ATTACK


public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected CharacterController characterController;
    protected FieldOfView fov;
    protected bool isGrounded;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected float jumpHeight;
    protected const float gravityValue = -9.81f;

    protected AiStateMachine currentState;
    protected Coroutine routine;

    // Coroutine Returned Results
    //
    // LOOK Routine
    protected Vector3 nextLookDirection;

    void Awake() 
    {
        characterController = GetComponent<CharacterController>();
        fov = GetComponent<FieldOfView>();        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StateMachineHandler());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StateMachineHandler() 
    {
        // Initialize the current routine
        this.currentState = AiStateMachine.LOOK;
        this.routine = StartCoroutine(Look(1));

        AiStateMachine previousState = AiStateMachine.LOOK;
        while(true)
        {   
            if(previousState == AiStateMachine.LOOK &&
                this.currentState == AiStateMachine.TURN) {
                StopCoroutine(routine);
                this.routine = StartCoroutine(Turn(this.nextLookDirection, 90));
            }
            else if(previousState == AiStateMachine.TURN &&
                this.currentState == AiStateMachine.LOOK) {
                StopCoroutine(routine);
                this.routine = StartCoroutine(Look(1));
            }
            //... TODO
            previousState = this.currentState;
            yield return new WaitForFixedUpdate();
        }
    }

    // LOOK State picks a random direction in the fov radius to walk to.
    // Also during this period, the AI may rest for a short duration
    IEnumerator Look(float restingPeriod) 
    {
        yield return new WaitForSeconds(restingPeriod);
        float randomAngle = Random.Range(-180f, 180f);
        this.nextLookDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * transform.forward;
        
        // Set the next State
        this.currentState = AiStateMachine.TURN;
        yield return null;
    }

    // TURN State turns to the facing direction
    IEnumerator Turn(Vector3 direction, float speedInDegPerSec)
    {   
        // Fuzzy Compare
        while(Vector3.SqrMagnitude(transform.forward - direction) > 0.001f)
        {
            float singleStep = (speedInDegPerSec * Mathf.Deg2Rad) * Time.fixedDeltaTime;
            transform.forward = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);
            yield return new WaitForFixedUpdate();
            //Debug.Log(Vector3.SqrMagnitude(transform.forward - direction));
        }

        // Set the next State
        this.currentState = AiStateMachine.LOOK;
        yield return null;
    }
}
