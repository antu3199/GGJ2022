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
        Vector3 motionNoY = motion;
        motionNoY.y = 0;
        // If we're passed (approx) the zero vector, then the character is not walking
        if (Vector3.Distance(motionNoY, Vector3.zero) <= 0.005) {
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

    public virtual bool IsIdleOrWalking()
    {
        AnimatorStateInfo animInfo = GetAnimatorStateInfo();
        return animInfo.IsName("Idle") || animInfo.IsName("Walk Start") || animInfo.IsName("Walk Loop");
    }

    public virtual void DoAttackAnimation() {
        if (_animationController != null) {
            _animationController.ResetTrigger("DoAttack");
            _animationController.SetTrigger("DoAttack");
        }
    }

    public virtual void Death() {
        DoDeathAnimation(); // Do the death animation
    }


    public virtual void ResetDeathAnimation() {
        if (_animationController != null) {
            _animationController.ResetTrigger("Die");
        }
    }

    public virtual void DoDeathAnimation() {
        if (_animationController != null) {
            _animationController.ResetTrigger("Die");
            _animationController.SetTrigger("Die");
        }
    }
}