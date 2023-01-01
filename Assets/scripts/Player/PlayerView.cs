//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerView
{
    private bool _sprint;

    private Animator _animator;

    private MonoBehaviour _mono;

    public PlayerView(Animator playerAnimator, MonoBehaviour playerMonoBehaviour)
    {
        _animator = playerAnimator;
    
        _mono = playerMonoBehaviour;
    }

    public void Run(float x, float z, bool onSprint, bool aim)
    {
        _sprint = onSprint;

        if(aim) 
        {
            _animator.SetBool("Sprint", false);
        }
        else if(_sprint && z !> 0)
        {
            _animator.SetBool("Sprint", true);
        }
        else
        {
            _animator.SetFloat("Horizontal", x);
            _animator.SetFloat("Vertical", z);

            _animator.SetBool("Sprint", false);
        }
    }

    public void SetCrouchAnim(bool crouching) => _animator.SetBool("Crouch", crouching);

    public void SetAimAnim(bool _isAiming) => _animator.SetBool("Aim", _isAiming);

    public void SetShootAnim(bool shooting)
    {
        string parameter = "Shoot";
        _animator.SetBool("Shoot", shooting);
    }

    public void Jump()
    {
        string parameter = "Jump";
        _animator.SetBool(parameter, true);

        _mono.StartCoroutine(ChangeBoolParameter(parameter));
    }

    public void Falling(bool falling) => _animator.SetBool("Falling", falling);

    IEnumerator ChangeBoolParameter(string parameterModificated)
    {
        yield return new WaitForSeconds(0.6f);
        _animator.SetBool(parameterModificated, false);
    }
}