//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTimer
{
    private float timer;

    private float coolDown;

    public GenericTimer(float _coolDown) => coolDown = _coolDown;
    
    public void RunTimer() => timer = timer + 1 * Time.deltaTime;

    public bool CheckCoolDown()
    {
        if(timer > coolDown)
            return true;   
        else 
            return false;
    }

    public void ResetTimer() => timer = 0;
}