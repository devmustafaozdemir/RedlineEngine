using System;
using UnityEngine;



public abstract class AdPlatform
{
    public abstract void SetTestMode(bool testMode);
    public abstract void Initialize();
    public abstract void LoadBannerAd();
    public abstract void ShowBannerAd();
    public abstract void HideBannerAd();
    public abstract void LoadInterstitialAd();
    public abstract void ShowInterstitialAd();
    public abstract void LoadRewardedAd();
    public abstract void ShowRewardedAd(Action reward);

}