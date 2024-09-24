using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Lists")]
    public List<Car> cars = new List<Car>();

    [Header("Objects")]
    public GameObject activeCar;
    private GameObject nextActiveCar;

    [Header("Values")]
    public int activeCarID;
    public float rotationSpeed = 10f;
    private bool isRotating = false;

    [Header("Images")]
    public Image adImage;

    [Header("Buttons")]
    public Button nextButton;
    public Button backButton;
    public Button startButton;
    public Button rotateButton;

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
        rotateButton.onClick.AddListener(RotateCar);

        startButton.onClick.AddListener(GetScene);
    }

	private void Update()
	{
        if(isRotating == true)
		{
            activeCar.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }        
    }

	public void RotateCar()
	{
        isRotating = true;        
	}

    public void GetCar(bool nextCar = true, bool isStart = false)
    {
        isRotating = false;

        if(activeCar != null)
        activeCar.transform.DOKill();

		for (int i = 0; i < cars.Count; i++)
		{
            cars[i].carObj.transform.DORotate(Vector3.zero, 0f);
		}

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
