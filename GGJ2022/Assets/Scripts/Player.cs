using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * THe abstract player class, that the defender and attacker extend from
*/
public class Player : MonoBehaviour
{
    protected CharacterController characterController;
    protected Vector3 playerVelocity;
    protected bool isGrounded;
    [SerializeField] protected float playerSpeed = 1.0f;
    [SerializeField] protected float jumpHeight = 1.0f;
    protected const float gravityValue = -9.81f;

    protected KeyCode forwardKeyCode = KeyCode.UpArrow;
    protected KeyCode backwardKeyCode = KeyCode.DownArrow;
    protected KeyCode leftKeyCode = KeyCode.LeftArrow;
    protected KeyCode rightKeyCode = KeyCode.RightArrow;

    void Start()
    {
        
    }

    protected void Update()
    {
        // Yoinked from https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
        isGrounded = characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        float vertical = 0;
        float horizontal = 0;

        if (Input.GetKey(forwardKeyCode)) {
            vertical = 1;
        }

        if (Input.GetKey(backwardKeyCode)) {
            vertical = -1;
        }

        if (Input.GetKey(leftKeyCode)) {
            horizontal = 1;
        }

         if (Input.GetKey(rightKeyCode)) {
            horizontal = -1;
        }

        Vector3 moveVector = new Vector3(horizontal, 0, vertical);

        characterController.Move(moveVector * Time.deltaTime * playerSpeed);

        if (moveVector != Vector3.zero)
        {
            gameObject.transform.forward = moveVector;
        }

        // Changes the height position of the player
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
