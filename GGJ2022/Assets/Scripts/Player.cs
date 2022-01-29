using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * THe abstract player class, that the defender and attacker extend from
*/
public abstract class Player : MonoBehaviour
{
    [SerializeField] protected CharacterController CharacterController;
    [SerializeField] protected Animator AnimationController;

    protected Vector3 PlayerVelocity;
    protected bool IsGrounded;
    [SerializeField] protected float PlayerSpeed = 3.0f;
    [SerializeField] protected float JumpHeight = 1.0f;
    protected const float GravityValue = -9.81f;

    protected KeyCode ForwardKeyCode = KeyCode.UpArrow;
    protected KeyCode BackwardKeyCode = KeyCode.DownArrow;
    protected KeyCode LeftKeyCode = KeyCode.LeftArrow;
    protected KeyCode RightKeyCode = KeyCode.RightArrow;

    void Start()
    {
        
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
            SetWalking(true);
        }

        if (Input.GetKey(BackwardKeyCode)) {
            vertical = -1;
            SetWalking(true);
        }

        if (Input.GetKey(RightKeyCode)) {
            horizontal = 1;
            SetWalking(true);
        }

         if (Input.GetKey(LeftKeyCode)) {
            horizontal = -1;
            SetWalking(true);
        }

        if (horizontal == 0 && vertical == 0) {
            SetWalking(false);
        }

        Vector3 moveVector = new Vector3(horizontal, 0, vertical);

        CharacterController.Move(moveVector * Time.deltaTime * PlayerSpeed);

        if (moveVector != Vector3.zero) {
            gameObject.transform.forward = moveVector;
        }
    }

    public AnimatorStateInfo GetAnimatorStateInfo() {
        return AnimationController.GetCurrentAnimatorStateInfo(0);
    }

    protected abstract void SetWalking(bool isWalking);

    public abstract void DoAbility1();

    public abstract void DoAbility2();

    public abstract void DoAbility3();

    public abstract void DoUltimateAbility();
}
