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
    [SerializeField] bool AudioSetting;

    private void Update()
    {
        if (AudioSetting)
        {
            AudioS.pitch = (((Pitch.y - Pitch.x) / CarController.Instance.MaxSpeed) * CarController.Instance.CurrentSpeed) + Pitch.x;
            AudioS.volume = Mathf.Lerp(AudioS.volume, MaxVolume, VolumeLerpSpeed * Time.deltaTime);
        }
    }

    public void ChangeGearSound(float Lerp)
    {
        AudioS.volume = 0f;
        VolumeLerpSpeed = Lerp;
    }

    public IEnumerator StartTheEngine()
    {
        if (CarController.Instance.IsCarStarted)
        {
            AudioS.loop = false;
            AudioS.clip = EngineStartSound;
            AudioS.Play();

            yield return new WaitForSeconds(EngineStartSound.length);

            AudioSetting = true;
            AudioS.clip = GearSound;
            AudioS.Play();

            yield return new WaitForSeconds(GearSound.length);

            AudioS.loop = true;
            AudioS.clip = EngineSound;
            AudioS.Play();
        }

        else
        {
            AudioS.Stop();
            AudioSetting = false;
            AudioS.pitch = 1f;
        }
    }
}
