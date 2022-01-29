using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateMachine {
    // NON AGGRO STATES
    LOOK,
    TURN,
    WALK,
    STUNNED,
    // AGGRO STATES
    CHASE,
    ATTACK
}

// Possible State Transitions
//
//                -------------------|
//                |                  |
//                v                  |
// --> LOOK --> TURN --> WALK     STUNNED
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
    protected MyCharacterController myCharacterController;
    [SerializeField] protected Animator animationController;
    [SerializeField] protected FieldOfView fov;
    protected bool isGrounded;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected float jumpHeight;
    [SerializeField] protected float turnSpeedInDegPerSec;
    [SerializeField] protected float chaseDuration;
    protected const float gravityValue = -9.81f;

    protected AiStateMachine currentState;
    protected Coroutine routine;

    // Coroutine Returned Results
    //
    // LOOK Routine
    protected Vector3 nextLookDirection;
    // TURN Routine
    // WALK Routine
    // CHASE Routine
    protected GameObject aggroTarget;

    void Awake() 
    {
        myCharacterController = new MyCharacterController(characterController, animationController);       
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
            // Aggro Check
            bool inAggro = isAggro();
            if(inAggro) {
                if(previousState == AiStateMachine.LOOK || 
                 previousState == AiStateMachine.TURN ||
                 previousState == AiStateMachine.WALK) {
                    this.currentState = AiStateMachine.CHASE;
                    StopCoroutine(routine);
                    this.routine = StartCoroutine(Chase(fov.Visible, playerSpeed, turnSpeedInDegPerSec, chaseDuration));
                }
                else if(previousState == AiStateMachine.CHASE &&
                    this.currentState == AiStateMachine.ATTACK) {
                    StopCoroutine(routine);
                    this.routine = StartCoroutine(Attack(aggroTarget));
                }
                else if(previousState == AiStateMachine.ATTACK &&
                    this.currentState == AiStateMachine.CHASE)
                {
                    StopCoroutine(routine);
                    this.routine = StartCoroutine(Chase(fov.Visible, playerSpeed, turnSpeedInDegPerSec, chaseDuration));
                }
            }
            else 
            {
                if(previousState == AiStateMachine.LOOK &&
                    this.currentState == AiStateMachine.TURN) {
                    StopCoroutine(routine);
                    this.routine = StartCoroutine(Turn(this.nextLookDirection, turnSpeedInDegPerSec));
                }
                else if(previousState == AiStateMachine.TURN &&
                    this.currentState == AiStateMachine.WALK) {
                    StopCoroutine(routine);
                    this.routine = StartCoroutine(Walk(playerSpeed));
                }
                else if(previousState == AiStateMachine.WALK &&
                    this.currentState == AiStateMachine.LOOK) {
                    StopCoroutine(routine);
                    this.routine = StartCoroutine(Look(1));
                }
                else if(previousState == AiStateMachine.CHASE)
                {
                    this.currentState = AiStateMachine.LOOK;
                    StopCoroutine(routine);
                    this.routine = StartCoroutine(Look(1));
                }
            }

            //... TODO
            previousState = this.currentState;
            yield return new WaitForFixedUpdate();
        }
    }

    bool isAggro()
    {
        return fov.Visible.Count > 0;
    }

    // LOOK State picks a random direction in the fov radius to walk to.
    // Also during this period, the AI may rest for a short duration
    IEnumerator Look(float restingPeriod) 
    {
        // While the enemy is turning/looking, we wanna set the enemy to the idle animation
        this.myCharacterController.Move(Vector3.zero);

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
        }

        // Set the next State
        this.currentState = AiStateMachine.WALK;
        yield return null;
    }

    // MOVE State Moves somewhere forward
    IEnumerator Walk(float speed)
    {
        // Walk towards to max of ([0, fov.viewRadius], distance to first collision)
        float distance = Random.Range(fov.viewRadius/4, fov.viewRadius);
        foreach(LayerMask mask in fov.obstacleMasks) 
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, transform.forward, out hitInfo, distance, mask))
            {
                // Hit something
                if(distance > hitInfo.distance)
                {
                    distance = hitInfo.distance;
                }
            }      
        }

        Vector3 toWalkTo = transform.position + transform.forward * distance;
        while(Vector3.SqrMagnitude(transform.position - toWalkTo) >= this.characterController.radius)
        {
            this.myCharacterController.Move(transform.forward * Time.fixedDeltaTime * speed);
            yield return new WaitForFixedUpdate();
        }

        // Ensure we at least spend one frame before updating current state
        yield return new WaitForFixedUpdate();
        // Set the next State
        this.currentState = AiStateMachine.LOOK;
        yield return null;
    }

    // CHASE State, picks the closer aggro to chase down
    IEnumerator Chase(List<GameObject> aggro, float speed, float turnSpeedInDegPerSec, float duration) {
        if(aggro.Count == 0) {
            yield break;
        }
        GameObject toChase = aggro[0];
        foreach(GameObject obj in aggro) {
            float distToCurrent = Vector3.SqrMagnitude(transform.position - toChase.transform.position);
            float distToNew = Vector3.SqrMagnitude(transform.position - obj.transform.position);
            if(distToNew < distToCurrent) {
                toChase = obj;
            }
        }

        float accTime = 0f;
        float chaseTime = Random.Range(duration/2, duration);
        while(true) {
            if(accTime > chaseTime) {
                break;
            }
            Vector3 direction = (toChase.transform.position - transform.position).normalized;
            float singleStep = (turnSpeedInDegPerSec * Mathf.Deg2Rad) * Time.fixedDeltaTime;
            // Turn the character
            transform.forward = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);
            // Move the character
            this.myCharacterController.Move(direction * Time.fixedDeltaTime * speed);
            yield return new WaitForFixedUpdate();
            accTime += Time.fixedDeltaTime;
        }

        // Ensure we at least spend one frame before updating current state
        yield return new WaitForFixedUpdate();
        this.aggroTarget = toChase;
        this.currentState = AiStateMachine.ATTACK;
        yield return null;
    }

    // ATTACK State, AI wait for enemy to complete it's attack
    IEnumerator Attack(GameObject target) {
        //TODO: Call the Attack Method for this unit
        DoAttackAnimation();
        while(true) {
            //TODO: ... wait for attack to complete
            yield return new WaitForSeconds(3f);
            break;
        }

        // Ensure we at least spend one frame before updating current state
        yield return new WaitForFixedUpdate();
        this.currentState = AiStateMachine.CHASE;
        yield return null;
    }

    protected virtual void DoAttackAnimation() {
        if (animationController != null) {
            animationController.ResetTrigger("DoAttack");
            animationController.SetTrigger("DoAttack");
        }
    }
}
