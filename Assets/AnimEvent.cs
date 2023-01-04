using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField]
    private PlayerModel _player;

    private void LauchGranade()
    {
        _player.ThoughGranade();

        StartCoroutine(BackToMovement());
    }

    IEnumerator BackToMovement()
    {
        yield return new WaitForSeconds(1f);

        _player.a();
    }
}
