using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerType
{
    ATTACKER,
    DEFENDER
};

/*
 * THe abstract player class, that the defender and attacker extend from
*/
public abstract class Player : MonoBehaviour
{
    public PlayerType PlayerType;
    [SerializeField] protected CharacterController CharacterController;
    protected MyCharacterController MyCharacterController;
    [SerializeField] protected Animator AnimationController;
    
    [SerializeField] HealthBar HealthBar;
    [SerializeField] Canvas HealthBarCanvas;
    [SerializeField] int TotalHealth;
    [SerializeField] float Armor;
    protected int CurrentHealth;

    protected Vector3 PlayerVelocity;
    protected bool IsGrounded;
    [SerializeField] protected float PlayerSpeed = 3.0f;
    [SerializeField] protected float JumpHeight = 1.0f;
    protected const float GravityValue = -9.81f;

    protected KeyCode ForwardKeyCode = KeyCode.UpArrow;
    protected KeyCode BackwardKeyCode = KeyCode.DownArrow;
    protected KeyCode LeftKeyCode = KeyCode.LeftArrow;
    protected KeyCode RightKeyCode = KeyCode.RightArrow;

    void Awake() {
        MyCharacterController = new MyCharacterController(CharacterController, AnimationController);

        CurrentHealth = TotalHealth;
        HealthBar.SetTotalHealth(TotalHealth);
    }

    protected void Update()
    {
        // Yoinked from https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

        if (CharacterController == null) return;

        IsGrounded = CharacterController.isGrounded;
        if (IsGrounded && PlayerVelocity.y < 0) {
            PlayerVelocity.y = 0f;
        }

        float vertical = 0;
        float horizontal = 0;

        if (Input.GetKey(ForwardKeyCode)) {
            vertical = 1;
        }

        if (Input.GetKey(BackwardKeyCode)) {
            vertical = -1;
        }

        if (Input.GetKey(RightKeyCode)) {
            horizontal = 1;
        }

         if (Input.GetKey(LeftKeyCode)) {
            horizontal = -1;
        }

        Vector3 moveVector = new Vector3(horizontal, 0, vertical);

        MyCharacterController.Move(moveVector * Time.deltaTime * PlayerSpeed);

        if (moveVector != Vector3.zero) {
            gameObject.transform.forward = moveVector;
        }

        // We have to make the healthbar's rotation the reverse of the player, so the healthbar stays facing forward
        if (HealthBarCanvas != null) {
            Vector3 cameraPosition;
            if (PlayerType == PlayerType.ATTACKER)
            {
                cameraPosition = GameManager.Instance.AttackerCamera.transform.position;
            }
            else if (PlayerType == PlayerType.DEFENDER)
            {
                cameraPosition = GameManager.Instance.DefenderCamera.transform.position;
            }
            else
            {
                cameraPosition = Camera.main.transform.position;
            }
            HealthBarCanvas.transform.rotation = Quaternion.LookRotation(transform.position - cameraPosition, Vector3.up);
        }
    }

    public AnimatorStateInfo GetAnimatorStateInfo() {
        return AnimationController.GetCurrentAnimatorStateInfo(0);
    }

    public bool IsDoingBasicAttack() {
        return MyCharacterController.IsDoingBasicAttack();
    }


    public abstract void DoAbility1();

    public abstract void DoAbility2();

    public abstract void DoAbility3();

    public abstract void DoUltimateAbility();
}
