using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A wrapper especially for CharacterController.Move (to handle walking animations)
public class MyCharacterController
{
    public CharacterController CharacterController = null;
    private Animator _animationController = null;

    public MyCharacterController(CharacterController controller, Animator animator) {
        CharacterController = controller;
        _animationController = animator;
    }

    public void SetAnimationController(Animator animator) {
        _animationController = animator;
    }

    public void SetCharacterController(CharacterController controller) {
        CharacterController = controller;
    }

    public CollisionFlags Move(Vector3 motion) {
        // If we're passed (approx) the zero vector, then the character is not walking
        if (Vector3.Distance(motion, Vector3.zero) <= 0.005) {
            if (_animationController != null) {
                _animationController.SetBool("IsWalking", false);
            }
        } else {
            if (_animationController != null) {
                _animationController.SetBool("IsWalking", true);
            }
        }

        return CharacterController.Move(motion);
    }

    public AnimatorStateInfo GetAnimatorStateInfo() {
        return _animationController.GetCurrentAnimatorStateInfo(0);
    }

    public virtual bool IsDoingBasicAttack() {
        return GetAnimatorStateInfo().IsName("Basic Attack");
    }

    public virtual void DoAttackAnimation() {
        if (_animationController != null) {
            _animationController.ResetTrigger("DoAttack");
            _animationController.SetTrigger("DoAttack");
        }
    }
}