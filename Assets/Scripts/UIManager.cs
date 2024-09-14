using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Lists")]
    public List<Car> cars = new List<Car>();

    [Header("Objects")]
    public GameObject activeCar;

    [Header("Values")]
    public int activeCarID;

    [Header("Images")]
    public Image adImage;

    [Header("Buttons")]
    public Button nextButton;
    public Button backButton;
    public Button startButton;

    [Header("Texts")]
    public TextMeshProUGUI carNameTMP;

    private void Start()
    {
        GetCar(isStart: true);

        Initialize();
    }

    public void Initialize()
    {
        AdManager.LoadRewardedAd();

        nextButton.onClick.AddListener(() => GetCar(true,false));
        backButton.onClick.AddListener(() => GetCar(false,false));

        startButton.onClick.AddListener(GetScene);
    }

    public void GetCar(bool nextCar = true, bool isStart = false)
    {
        if (Equals(isStart,false))
        {
            if (nextCar == true && activeCarID != cars.Count-1)
                activeCarID++;
            else if (nextCar == false && activeCarID != 0)
                activeCarID--;
        }
        
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].carObj.gameObject.SetActive(false);
        }
       
        carNameTMP.text = cars[activeCarID].carName;       
        activeCar = cars[activeCarID].carObj;
        activeCar.gameObject.SetActive(true);

        if (activeCarID != 0)
        {          
            adImage.gameObject.SetActive(true);           
            return;
        }

        adImage.gameObject.SetActive(false);            
    }

    public void GetScene()
    {
        if(activeCarID == 0)
        {
            SceneManager.LoadScene("Game Scene");
            InGameInitialize();
        }       
        else
        {
            AdManager.ShowRewardedAd(() =>
            {
                SceneManager.LoadScene("Game Scene");
                InGameInitialize();
            });
        }        
    }

    public void InGameInitialize()
    {
        //Ýstediðin kendi sayfanda olacak initialize burada yapýlacak. (Yavuz)
    }
}

[System.Serializable]
public class Car
{
    public string carName;
    public GameObject carObj;
}
