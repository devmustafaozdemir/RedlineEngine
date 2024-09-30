using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
	[Header("Buttons")]
	public Button playButton;
	public Button vibrateButton;
	public Button speedButton;

	[Header("Objects")]
	public Transform carTransform;

	[Header("Settings Values")]
	public int vibarteCount;
	public int speedCount;

	[Header("Text")]
	public TextMeshProUGUI vibrateInfoText;
	public TextMeshProUGUI speedInfoText;

	public void Start()
	{
		playButton.onClick.AddListener(Play);

		vibrateButton.onClick.AddListener(VibrateController);
		speedButton.onClick.AddListener(KmphController);

		if (PlayerPrefs.GetInt("Vibrate") == 0)
		{
			vibrateInfoText.text = "Açýk";
		}
		else
		{
			vibrateInfoText.text = "Kapalý";
		}

		if (PlayerPrefs.GetInt("speed") == 0)
		{
			speedInfoText.text = "KMPH";
		}
		else
		{
			speedInfoText.text = "MPH";
		}
	}

	public void VibrateController()
	{
		vibrateButton.transform.DOKill();
		vibrateButton.transform.DOScale(new Vector3(0.7f,0.7f,0.7f),0f);
		vibrateButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 1, 0.1f);


		if (vibarteCount == 0)
		{
			vibarteCount++;
			vibrateInfoText.text = "Kapalý";
			PlayerPrefs.GetInt("Vibrate", 1);			
		}
		else if (vibarteCount == 1)
		{
			vibarteCount--;
			vibrateInfoText.text = "Açýk";
			PlayerPrefs.GetInt("Vibrate", 0);		
		}
	}
	public void KmphController()
	{
		speedButton.transform.DOKill();
		speedButton.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0f);
		speedButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 1, 0.1f);

		if (speedCount == 0)
		{
			speedCount++;
			speedInfoText.text = "MPH";
			PlayerPrefs.SetInt("speed", 1);
		}
		else if (speedCount == 1)
		{
			speedCount--;
			speedInfoText.text = "KMPH";
			PlayerPrefs.SetInt("speed", 0);
		}
	}

	public async void Play()
	{
		carTransform.transform.DOMoveX(5, 4f);

		await Task.Delay(2000);
		SceneManager.LoadScene("Garage Scene");
	}
}
