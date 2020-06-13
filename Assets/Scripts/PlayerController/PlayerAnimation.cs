using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    public E_PlayerAnimationType PlayerAnimationType { get; private set; }

    public Animator PlayerAnimator => this._animator;

    private void Awake()
    {
        InitAnimation();
    }

    private void InitAnimation()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Play(E_PlayerAnimationType.Damaged);
        }
    }

    public void Play(E_PlayerAnimationType animationType)
    {
        _animator.SetBool("IsStanding", false);
        switch (animationType)
        {
            case E_PlayerAnimationType.Standing:
                _animator.SetBool("IsRunning", false);
                _animator.SetBool("Salute", false);
                _animator.SetBool("IsFalling", false);
                _animator.SetBool("TopOfJump", false);
                _animator.SetBool("IsStanding", true);
                _animator.SetBool("IsLanding", true);
                break;
            case E_PlayerAnimationType.Running:
                _animator.SetBool("IsRunning", true);
                _animator.SetBool("IsFalling", false);
                _animator.SetBool("TopOfJump", false);
                break;
            case E_PlayerAnimationType.JumpToTop:
                Debug.Log("jump");
                _animator.SetBool("IsLanding", false);
                _animator.SetTrigger("JumpToTop");
                _animator.SetBool("IsFalling", false);
                break;
            case E_PlayerAnimationType.TopOfJump:
                _animator.SetBool("TopOfJump", true);
                _animator.SetBool("IsFalling", false);
                break;
            case E_PlayerAnimationType.TopToGround:
                _animator.SetBool("IsFalling", true);
                break;
            case E_PlayerAnimationType.StickingWall:
                _animator.SetBool("StickingWall", true);
                break;
            case E_PlayerAnimationType.Damaged:
                _animator.SetTrigger("Damaged");
                break;
            case E_PlayerAnimationType.Salute:
                _animator.SetBool("Salute", true);
                break;
            case E_PlayerAnimationType.KnellDown:
                _animator.SetTrigger("KnellDown");
                break;
            case E_PlayerAnimationType.KnellDownToUp:
                _animator.SetTrigger("KnellDownToUp");
                break;
        }
    }

    public enum E_PlayerAnimationType
    {
        Standing,
        Running,
        JumpToTop,
        TopOfJump,
        TopToGround,
        StickingWall,
        Damaged,
        Salute,
        KnellDown,
        KnellDownToUp,
    }
}