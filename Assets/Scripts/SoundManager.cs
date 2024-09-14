using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource AudioS;

    public AudioClip EngineStartSound;
    public AudioClip EngineSound;
    [SerializeField] AudioClip GearSound;

    [Header("Variables")]

    [SerializeField] Vector2 Pitch;

    private void Update()
    {
        AudioS.pitch = (((Pitch.y - Pitch.x) / CarController.Instance.MaxSpeed) * CarController.Instance.CurrentSpeed) + Pitch.x;
    }
}
