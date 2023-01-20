//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyInTime : MonoBehaviour
{
    [SerializeField]
    private float _timeToDestoy;

    void Start() => Destroy(gameObject, _timeToDestoy); 
}