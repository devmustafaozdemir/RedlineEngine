using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Threading.Tasks;

public class CameraController : MonoBehaviour
{

	public CinemachineVirtualCamera virtualCamera1;
	public CinemachineVirtualCamera virtualCamera2;
	public CinemachineVirtualCamera virtualCamera3;

	public UIManager managerUI;

	private bool isWorkedFirst = false;

	public bool isGarage;
	public bool isMenu;

	public GameObject menuVirtiual2;

	[SerializeField] private CinemachineBrain cinemachineBrain;
	[SerializeField] private List<VirtualCameraTransationItem> virtualCameraTransationItems = new List<VirtualCameraTransationItem>();

	private void Start()
	{
		if (isMenu == true)
		{
			PlayAnimation();
		}
	}

	public async void PlayAnimation()
	{
		if (isGarage == false)
		{
			foreach (var item in virtualCameraTransationItems)
			{
				await Task.Delay(item.delayMillisecond);

				CinemachineBlendDefinition customBlend = new CinemachineBlendDefinition();
				customBlend.m_Time = item.transationTime;
				customBlend.m_Style = item.style;
				cinemachineBrain.m_DefaultBlend = customBlend;

				foreach (var item2 in virtualCameraTransationItems)
					item2.camera.Priority = 0;

				item.camera.Priority = 2;

				await Task.Delay((int)(item.transationTime * 1000));
			}
		}
		else
		{
			if (isWorkedFirst == false)
			{
				virtualCamera1.Priority = 1;
				virtualCamera2.Priority = 0;
				isWorkedFirst = true;
			}
			else
			{
				virtualCamera1.Priority = 0;
				virtualCamera2.Priority = 1;
			}

			if (isGarage == true)
			{
				if (managerUI.activeCarID != 0)
				{
					transform.DORotate(new Vector3(0, 360), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(() =>
					{
						virtualCamera1.Priority = 0;
						virtualCamera2.Priority = 1;
					});
				}		
			}

		}
	}


}
[System.Serializable]
public class VirtualCameraTransationItem
{
	public CinemachineVirtualCamera camera;
	public CinemachineBlendDefinition.Style style;
	public float transationTime;
	public int delayMillisecond;

}



