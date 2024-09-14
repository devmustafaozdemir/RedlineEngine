using System;



public static class AdManager
{

    public static AdPlatform adPlatform = new AdmobController();
    public static bool testMode = false;

    private static bool initialized;

    public static void Initialize()
    {
        adPlatform.SetTestMode(testMode);
        adPlatform.Initialize();

        initialized = true;
    }

    public static void LoadBannerAd()
    {
        if (GameSettings.removeAds) return;

        if (!initialized)
            Initialize();

        adPlatform.LoadBannerAd();
    }

    public static void ShowBannerAd()
    {
        if (GameSettings.removeAds) return;

        if (!initialized)
            Initialize();

        adPlatform.ShowBannerAd();
    }

    public static void HideBannerAd()
    {
        if (GameSettings.removeAds) return;

        if (!initialized)
            Initialize();

        adPlatform.HideBannerAd();
    }

    public static void LoadInterstitialAd()
    {
        if (GameSettings.removeAds) return;

        if (!initialized)
            Initialize();

        adPlatform.LoadInterstitialAd();
    }

    public static void ShowInterstitialAd()
    {
        if (GameSettings.removeAds) return;

        if (!initialized)
            Initialize();

        adPlatform.ShowInterstitialAd();
    }

    public static void LoadRewardedAd()
    {
        if (!initialized)
            Initialize();

        adPlatform.LoadRewardedAd();
    }

    public static void ShowRewardedAd(Action reward)
    {
        if (!initialized)
            Initialize();

        adPlatform.ShowRewardedAd(reward);
    }


}