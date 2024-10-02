using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public static CarController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsCarStarted;

    [Header("Variables")]

    [SerializeField] bool IsGasOn;
    [SerializeField] bool IsBreaking;
    public float CurrentSpeed;
    public float MaxSpeed;
    [SerializeField] float DefaultBreakMult;

    [Header("")]
    [SerializeField] int CurrentGear;
    public List<Gears> Gears_;

    [Header("Indicators")]
    [SerializeField] float CurrentAngle;
    public Vector2 MinMaxIndicatorAngle;
    [SerializeField] float LerpSpeed;

    [Header("Indicator Selection")]

    public Text CurrentSpeedIndýcator;
    public Transform CurrentTorkIndicator;
    [SerializeField] float IndicatorMult;

    [Header("Transforms")]
    [SerializeField] Transform GearKnob;

    [Header("Lights")]

    [SerializeField] GameObject Orange;
    [SerializeField] GameObject Orange_Black;
    [SerializeField] GameObject Red;
    [SerializeField] GameObject Red_Black;

    private void Update()
    {
        if (!IsCarStarted)
            return;

        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0f, Gears_[CurrentGear].MinMaxGearSpeeds.y);
        CurrentSpeedIndýcator.text = (int)CurrentSpeed + "";

        if (CurrentSpeed <= Gears_[CurrentGear - 1].IdealSpeed && CurrentGear > 1 && !IsGasOn)
            SelectGear(CurrentGear - 1);

        if (IsBreaking)
        {
            CurrentSpeed -= Time.deltaTime * Gears_[CurrentGear].BreakMult;
        }

        else if (IsGasOn)
        {
            CurrentSpeed += Time.deltaTime * Gears_[CurrentGear].SpeedMult;
        }

        else
        {
            CurrentSpeed -= Time.deltaTime * DefaultBreakMult;
        }

        // Tork Indicator

        float DÝff = (MinMaxIndicatorAngle.y - MinMaxIndicatorAngle.x) / (Gears_[CurrentGear].MinMaxGearSpeeds.y - (Gears_[CurrentGear - 1].MinMaxGearSpeeds.x));
        CurrentAngle = ((CurrentSpeed) - (Gears_[CurrentGear - 1].MinMaxGearSpeeds.x)) * DÝff;
        CurrentTorkIndicator.localRotation =  Quaternion.Lerp(CurrentTorkIndicator.localRotation, Quaternion.Euler(0f, 0f, CurrentAngle * IndicatorMult), LerpSpeed * Time.deltaTime);

        // Lights

        if(CurrentAngle >= MinMaxIndicatorAngle.y - 80f)
        {
            Orange.SetActive(true);
            Orange_Black.SetActive(false);
        }

        else
        {
            Orange.SetActive(false);
            Orange_Black.SetActive(true);
        }

        if (CurrentAngle >= MinMaxIndicatorAngle.y - 10f)
        {
            Red.SetActive(true);
            Red_Black.SetActive(false);
        }

        else
        {
            Red.SetActive(false);
            Red_Black.SetActive(true);
        }

    }

    public void Gas()
    {
        IsGasOn = !IsGasOn;
    }

    public void Break()
    {
        IsBreaking = !IsBreaking;
    }

    public void SelectGear(int GearNUmber)
    {
        CurrentGear = GearNUmber;
        GearKnob.position = Gears_[CurrentGear].GearKnobPos.position;
        SoundManager.Instance.ChangeGearSound(5f);
    }

    public void StartStopCar()
    {
        IsCarStarted = !IsCarStarted;

        StartCoroutine(SoundManager.Instance.StartTheEngine());
    }
}

[System.Serializable]
public class Gears
{
    public int GearNumber;
    public Vector2 MinMaxGearSpeeds;
    public float IdealSpeed;
    public float SpeedMult;
    public float BreakMult;

    [Header("")]

    public Transform GearKnobPos;
}
