using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioSource AudioS;

    public AudioClip EngineStartSound;
    public AudioClip EngineSound;
    [SerializeField] AudioClip GearSound;

    [Header("Variables")]

    [SerializeField] Vector2 Pitch;
    [SerializeField] float MaxVolume;
    [SerializeField] float VolumeLerpSpeed;

    private void Update()
    {
        AudioS.pitch = (((Pitch.y - Pitch.x) / CarController.Instance.MaxSpeed) * CarController.Instance.CurrentSpeed) + Pitch.x;
        AudioS.volume = Mathf.Lerp(AudioS.volume, MaxVolume, VolumeLerpSpeed * Time.deltaTime);
    }

    public void ChangeGearSound(float Lerp)
    {
        AudioS.volume = 0f;
        VolumeLerpSpeed = Lerp;
    }
}
