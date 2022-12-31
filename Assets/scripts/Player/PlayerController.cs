//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private PlayerModel _player;

    public float x;
    public float z;

    private Action ArtificialUpdateLogics;
    private Action ArtificialFixedUpdateLogics;

    public PlayerController(PlayerModel player)
    {
        _player = player;

        SetMovementsDelegates();
    }

    private void SetMovementsDelegates()
    {
        ArtificialFixedUpdateLogics = CheckMovement;
        ArtificialFixedUpdateLogics += CheckCamera;
        ArtificialFixedUpdateLogics += CheckAim;

        ArtificialUpdateLogics = CheckJump;
        ArtificialUpdateLogics += CheckShoot;
        ArtificialUpdateLogics += CheckCrouch;
    }

    public void ArtificialUpdate()
    {
        ArtificialUpdateLogics();
    }

    public void ArtificialFixedUpdate()
    {
        ArtificialFixedUpdateLogics();
    }

    private void CheckMovement()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        bool sprint = Input.GetButton("Sprint");

        if (x != 0 || z != 0)
        {
            _player.MovePlayer(new Vector3(x, 0, z), sprint);
        }
    }

    private void CheckCamera()
    {
        float horizontalCam = Input.GetAxis("Mouse X");

        if (horizontalCam != 0)
        {
            _player.RotateWithMouse(horizontalCam);
        }
    }

    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _player.Jump();
        }
    }

    private void CheckAim()
    {
        _player.Aim(Input.GetButton("Aim"));

//         if (Input.GetButton("Aim"))
//         {
//             _player.Aim(true);
//         }
//         else
//         {
//             _player.Aim(false);   
//         }
    }

    private void CheckShoot()
    {
        _player.Shoot(Input.GetButton("Fire1"));

//         if (Input.GetButton("Fire1"))
//         {
//             _player.Shoot(true);
//         }
//         else
//         {
//             _player.Shoot(false);
//         }
    }

    private void CheckCrouch()
    {
        _player.Crouch(Input.GetButton("Crouch"));
    }
}
