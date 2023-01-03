//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  Entity : MonoBehaviour
{
    [SerializeField]
    protected float life;

    [SerializeField]
    protected float maxLife;

    protected float realSpeed;

    protected virtual void Awake() => life = maxLife;
    
    protected abstract void CheckLife();
}
