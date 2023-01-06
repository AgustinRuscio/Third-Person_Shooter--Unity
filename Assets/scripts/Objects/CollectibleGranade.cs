using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGranade : MonoBehaviour, IItem
{
    [SerializeField]
    private int _granadeAdded;

    [SerializeField]
    private SoundData _sound;

    public void OnGrab(bool canGrab)
    {
        if (canGrab)
        {
            EventManager.Trigger(ManagerKeys.GranadeAdded, _granadeAdded);

            AudioManager.instance.AudioPlay(_sound, transform.position);

            Destroy(gameObject);
        }
    }
}
