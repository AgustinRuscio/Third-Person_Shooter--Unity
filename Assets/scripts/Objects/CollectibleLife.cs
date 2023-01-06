using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLife : MonoBehaviour, IItem
{
    [SerializeField]
    private float _lifeToSum;

    [SerializeField]
    private SoundData _sound;

    public void OnGrab()
    {
        EventManager.Trigger(ManagerKeys.LifeAdded, _lifeToSum);

        AudioManager.instance.AudioPlay(_sound, transform.position);
        
        Destroy(gameObject);
    }
}
