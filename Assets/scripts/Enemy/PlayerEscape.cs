//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscape : MonoBehaviour
{
    [SerializeField]
    private EnemyModel _enemyModel;

    private void OnTriggerExit(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerModel>();

        if (player != null)
            _enemyModel.DesactivateMovement();
    }
}
