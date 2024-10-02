using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVariables : MonoBehaviour
{

    public static ChangeVariables Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] string TestId;
    public string SelectedId;
    public ID[] CarIDs;

    private void Start()
    {
        SelectedId = PlayerPrefs.GetString("CarID");
        LoadData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SetData();
    }

    public void SetData()
    {
        SelectedId = TestId;
        LoadData();
    }

    public void LoadData()
    {
        for (int i = 0; i < CarIDs.Length; i++)
        {
            if (CarIDs[i].ýd == SelectedId)
            {
                CarIDs[i].gameObject.SetActive(true);

                CarController.Instance.MaxSpeed = CarIDs[i].transform.GetComponent<Variables>().MaxSpeed;
                CarController.Instance.Gears_ = CarIDs[i].transform.GetComponent<Variables>()._Gears;
                CarController.Instance.MinMaxIndicatorAngle = CarIDs[i].transform.GetComponent<Variables>().MinMaxIndicatorAngle;
                CarController.Instance.CurrentTorkIndicator = CarIDs[i].transform.GetComponent<Variables>().CurrentTorkIndicator;
                CarController.Instance.CurrentSpeedIndýcator = CarIDs[i].transform.GetComponent<Variables>().SpeedUI;


            }

            else
                CarIDs[i].gameObject.SetActive(false);
        }
    }
}
