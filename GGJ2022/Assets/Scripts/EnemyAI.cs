using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateMachine {
    // NON AGGRO STATES
    LOOK,
    TURN,
    WALK,
    STUNNED,
    DEAD, 
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


public class EnemyAI : MonoBehaviour, IPausable
{
    [SerializeField] protected CharacterController characterController;
    protected MyCharacterController myCharacterController;
    [SerializeField] protected FieldOfView fov;

    [SerializeField] protected Material DefaultMaterial;
    [SerializeField] protected Material AttackedMaterial; // Enemy changes to red when it gets hit
    [SerializeField] protected SkinnedMeshRenderer MeshRenderer;

    [SerializeField] protected ParticleSystem DeathPFX;

    [SerializeField] HealthBar HealthBar;
    [SerializeField] Canvas HealthBarCanvas;
    [SerializeField] int TotalHealth;
    protected int CurrentHealth;

    public bool IsAttacking{get; set;}
    [SerializeField] float Damage;

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
    protected float slowPercentage = 1f; // % of time

    protected bool isDead = false;

    protected Vector3 EnemyVelocity;
    protected bool IsGrounded;

    void Awake() 
    {
        fov = GetComponent<FieldOfView>();
        myCharacterController = new MyCharacterController(characterController, GetComponentInChildren<Animator>());
        AttachPausable();
    }

    void Start()
    {
        CurrentHealth = TotalHealth;
        HealthBar.SetTotalHealth(TotalHealth);

        StartCoroutine(StateMachineHandler());
    }

    void Update()
    {
        // We have to make the healthbar's rotation the reverse of the enemy, so the healthbar stays facing forward
        if (HealthBarCanvas != null) {
            // Find the closest player, and look at their direction
            Vector3 attackerPos = GameManager.Instance.GetAttackerPlayer().transform.position;
            Vector3 defenderPos = GameManager.Instance.GetDefenderPlayer().transform.position;

            float attackerDist = Vector3.Distance(attackerPos, transform.position);
            float defenderDist = Vector3.Distance(defenderPos, transform.position);

            Vector3 locationToUse;
            if (attackerDist <= defenderDist)
            {
                locationToUse = GameManager.Instance.AttackerCamera.transform.position;
            }
            else
            {
                locationToUse = GameManager.Instance.DefenderCamera.transform.position;
            }

            HealthBarCanvas.transform.rotation = Quaternion.LookRotation(transform.position - locationToUse, Vector3.up);
        }

        IsGrounded = characterController.isGrounded;
        if ((IsGrounded && EnemyVelocity.y < 0) || isDead) {
            EnemyVelocity.y = 0f;
        }
        
        if (!IsGrounded && !isDead)
        {
            EnemyVelocity.y += -9.8f; // Add some gravity to prevent glitching
        }

    }


    void OnDestroy()
    {
        DetachPausable();
    }

    // Impl Interface
    public void AttachPausable()
    {
        PauseController pauser = (PauseController)PauseController.Instance;
        pauser?.Attach(this.gameObject, this);
    }

    public void DetachPausable()
    {
        PauseController pauser = (PauseController)PauseController.Instance;
        pauser?.Detach(this.gameObject, this);
    }

    public void Pause()
    {
        slowPercentage = 0;
    }

    public void Slow(float percentage)
    {
        slowPercentage = percentage;
    }

    public void Reset()
    {
        slowPercentage = 1;
    }

    IEnumerator StateMachineHandler() 
    {
        // Initialize the current routine
        this.currentState = AiStateMachine.LOOK;
        this.routine = StartCoroutine(Look(1));

        AiStateMachine previousState = AiStateMachine.LOOK;
        while(true)
        {   
            // If the enemy is dead, we don't want to do any more movements
            if (this.currentState != AiStateMachine.DEAD) {
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
            } else {
                // If we are dead, then stop whatever routine is running
                StopCoroutine(routine);
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
        this.myCharacterController.Move(EnemyVelocity + Vector3.zero);

        float rested = 0f;
        while(rested < restingPeriod) {
            yield return new WaitForSeconds(0.5f);
            rested += 0.5f * this.slowPercentage;
        }
        
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
            float singleStep = (speedInDegPerSec * Mathf.Deg2Rad) * Time.fixedDeltaTime * this.slowPercentage;
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
            if (!isDead) {
                this.myCharacterController.Move((EnemyVelocity + transform.forward * Time.fixedDeltaTime * speed) * this.slowPercentage);
            }
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
            float updateTime = Time.fixedDeltaTime * this.slowPercentage;
            Vector3 direction = (toChase.transform.position - transform.position).normalized;
            float singleStep = (turnSpeedInDegPerSec * Mathf.Deg2Rad) * updateTime;

            if (!isDead) {
                // Turn the character
                transform.forward = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);
                // Move the character
                this.myCharacterController.Move((EnemyVelocity + direction * speed) * updateTime);
            }
    
            yield return new WaitForFixedUpdate();
            accTime += updateTime;
        }

        // Ensure we at least spend one frame before updating current state
        yield return new WaitForFixedUpdate();
        this.aggroTarget = toChase;
        this.currentState = AiStateMachine.ATTACK;
        yield return null;
    }

    // ATTACK State, AI wait for enemy to complete it's attack
    IEnumerator Attack(GameObject target) {
        myCharacterController.DoAttackAnimation();
        while(myCharacterController.IsDoingBasicAttack()) {            
            yield return new WaitForSeconds(0.5f);
        }

        // Ensure we at least spend one frame before updating current state
        yield return new WaitForFixedUpdate();
        this.currentState = AiStateMachine.CHASE;
        yield return null;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Use events to determine whether or not to do damage
        if (IsAttacking) {
            // Check if the slime collided with a player
            if (hit.gameObject.tag == "Attacker" || hit.gameObject.tag == "Defender") {
                Player player = (Player)hit.gameObject.GetComponentInChildren<Player>();
                player.GetAttacked(player.CalculateDamageTaken(Damage));
            }
        }
    }

    public void GetAttacked(float damage) {
        // When the enemy's attacked, it'll turn red for 0.2s
        StartCoroutine(ApplyAttackedMaterial());

        // Decrement the enemy's health based on the damage
        CurrentHealth -= (int)damage;
        HealthBar.SetHealth(CurrentHealth < 0 ? 0 : CurrentHealth);

        if (CurrentHealth <= 0) {
            if (!isDead) {
                isDead = true;
                this.currentState = AiStateMachine.DEAD;

                // todo: Go into dead state, and do some death pfx and then destroy this object
                myCharacterController.Death();

                // Instantiate the death pfx and then destroy it after the particle system's lifetime
                Destroy(Instantiate(DeathPFX, transform.position, Quaternion.identity), DeathPFX.main.startLifetime.constant);

                StartCoroutine(SinkIntoGroundAndDestroy());
            }
        }
    }

    IEnumerator ApplyAttackedMaterial() {
        MeshRenderer.material = AttackedMaterial;

        yield return new WaitForSeconds(0.2f);

        MeshRenderer.material = DefaultMaterial;
    }

    // After the enemy dies, just have them sink into the ground I guess
    // We can destroy it after
    IEnumerator SinkIntoGroundAndDestroy() {
        float sinkDuration = 1.5f;
        float timePassed = 0f;

        // Only start sinking into the ground after the animation is done (25 frames)
        for (int i=0; i < 25; i++) {
            yield return null;
        }

        while (timePassed < sinkDuration) {
            this.transform.position += new Vector3(0, -0.05f, 0);
            timePassed += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
