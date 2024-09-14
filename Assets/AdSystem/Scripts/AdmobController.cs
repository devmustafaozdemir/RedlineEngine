using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



public class AdmobController : AdPlatform
{
    private bool testMode = false;

    public override void Initialize()
    {
        // When true all events raised by GoogleMobileAds will be raised
        // on the Unity main thread. The default value is false.

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {

            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();

            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        });
    }

    public override void SetTestMode(bool testMode)
    {
        this.testMode = testMode;
    }

    #region BannerAd

    private BannerView bannerView;

    private string androidTestBannerAdID = "ca-app-pub-3940256099942544/6300978111";
    private string iosTestBannerAdID = "ca-app-pub-3940256099942544/2934735716";

    private string androidBannerAdID = "ca-app-pub-4048498709664067/2397149071";
    private string iosAdBannerID = "ca-app-pub-3940256099942544/2934735716";

    public enum BannerSide
    {
        Top,
        Bottom
    }

    public override void LoadBannerAd()
    {
        BannerSide bannerSide = BannerSide.Bottom;

        string adUnitId = "";

        if (testMode)
        {
#if UNITY_EDITOR
            adUnitId = "unused";
#elif UNITY_ANDROID
            adUnitId = androidTestBannerAdID;
#elif UNITY_IPHONE
            adUnitId = iosTestBannerAdID;
#else
            adUnitId = "unexpected_platform";
#endif
        }
        else
        {
#if UNITY_EDITOR
            adUnitId = "unused";
#elif UNITY_ANDROID
            adUnitId = androidBannerAdID;
#elif UNITY_IPHONE
            adUnitId = iosAdBannerID;
#else
            adUnitId = "unexpected_platform";
#endif
        }

        // Clean up banner ad before creating a new one.
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        int coordinate = 0;

        if (Equals(bannerSide, BannerSide.Bottom))
        {
            coordinate = -1390 + (int)GetBottomSafeAreaHeight();
        }
        else
        {
            coordinate = 1390 - (int)GetTopSafeAreaHeight();
        }

        bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

        bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
        bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;
        bannerView.OnAdClicked += OnBannerAdClicked;
        bannerView.OnAdPaid += OnBannerAdPaid;
        bannerView.OnAdImpressionRecorded += OnBannerAdImpressionRecorded;
        bannerView.OnAdFullScreenContentClosed += OnBannerAdFullScreenContentClosed;
        bannerView.OnAdFullScreenContentOpened += OnBannerAdFullScreenContentOpened;

        AdRequest adRequest = new AdRequest();

        bannerView.LoadAd(adRequest);
    }

    public float GetBottomSafeAreaHeight()
    {
        float bottomUnits;

        Vector2 referenceResolution = new Vector2(2590, 1440);

        float bottomPixels = Screen.safeArea.y;
        float bottomRatio = bottomPixels / Screen.currentResolution.height;

        bottomUnits = referenceResolution.y * bottomRatio;

        return bottomUnits;

    }
    public float GetTopSafeAreaHeight()
    {
        float topUnits;

        Vector2 referenceResolution = new Vector2(2590, 1440);

        float topPixel = Screen.currentResolution.height - (Screen.safeArea.y + Screen.safeArea.height);
        float topRatio = topPixel / Screen.currentResolution.height;

        topUnits = referenceResolution.y * topRatio;

        return topUnits;
    }

    public override void ShowBannerAd()
    {
        if (bannerView == null)
            LoadBannerAd();

        bannerView.Show();
    }

    public override void HideBannerAd()
    {
        bannerView.Hide();
    }




    /// <summary>
    /// Raised when an ad is loaded into the banner view.
    /// </summary>
    public void OnBannerAdLoaded()
    {
        Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
    }

    /// <summary>
    /// Raised when an ad fails to load into the banner view.
    /// </summary>
    public void OnBannerAdLoadFailed(LoadAdError error)
    {
        Debug.LogError("Banner view failed to load an ad with error : "
              + error);
    }

    /// <summary>
    /// Raised when the ad is estimated to have earned money.
    /// </summary>
    /// <param name="adValue"></param>
    public void OnBannerAdPaid(AdValue adValue)
    {
        Debug.Log(String.Format("Banner view paid {0} {1}.",
         adValue.Value,
         adValue.CurrencyCode));
    }

    /// <summary>
    /// Raised when an impression is recorded for an ad.
    /// </summary>
    public void OnBannerAdImpressionRecorded()
    {
        Debug.Log("Banner view recorded an impression.");
    }

    /// <summary>
    /// Raised when a click is recorded for an ad.
    /// </summary>
    public void OnBannerAdClicked()
    {
        Debug.Log("Banner view was clicked.");
    }

    /// <summary>
    /// Raised when an ad opened full screen content.
    /// </summary>
    public void OnBannerAdFullScreenContentOpened()
    {
        Debug.Log("Banner view full screen content opened.");
    }

    /// <summary>
    /// Raised when the ad closed full screen content.
    /// </summary>
    public void OnBannerAdFullScreenContentClosed()
    {
        Debug.Log("Banner view full screen content closed.");
    }

    #endregion

    #region InterstitialAd

    private string androidTestInterstitialAdID = "ca-app-pub-3940256099942544/1033173712";
    private string iosTestInterstitialAdID = "ca-app-pub-3940256099942544/4411468910";

    private string androidInterstitialAdID = "ca-app-pub-4048498709664067/4002493337";
    private string iosInterstitialAdID = "ca-app-pub-8240985822528868/6786559766";

    private InterstitialAd interstitialAd;

    public override void LoadInterstitialAd()
    {

        string adUnitId = "";

        if (testMode)
        {
#if UNITY_ANDROID
                adUnitId = androidTestInterstitialAdID;
#elif UNITY_IPHONE
                adUnitId = iosTestInterstitialAdID;
#else
            adUnitId = "unused";
#endif
        }
        else
        {
#if UNITY_ANDROID
                adUnitId = androidInterstitialAdID;
#elif UNITY_IPHONE
                adUnitId = iosInterstitialAdID;
#else
            adUnitId = "unused";
#endif
        }


        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(adUnitId, adRequest,
            async (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    //TryLoadInterstitialAdAgain();
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }

    public override void ShowInterstitialAd()
    {
        if (interstitialAd == null)
        {
            Debug.LogWarning("Interstitial ad is not ready yet.");
            LoadInterstitialAd();
        }

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }

    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdClicked += OnInterstitialAdClicked;
        ad.OnAdImpressionRecorded += OnInterstitialAdImpressionRecorded;
        ad.OnAdPaid += OnInterstitialAdPaid;
        ad.OnAdFullScreenContentOpened += OnInterstitialAdFullScreenContentOpened;
        ad.OnAdFullScreenContentClosed += OnInterstitialAdFullScreenContentClosed;
        ad.OnAdFullScreenContentFailed += OnInterstitialAdFullScreenContentFailed;
    }

    /// <summary>
    /// Raised when the ad is estimated to have earned money.
    /// </summary>
    /// <param name="adValue"></param>
    private void OnInterstitialAdPaid(AdValue adValue)
    {
        Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
              adValue.Value,
              adValue.CurrencyCode));
    }

    /// <summary>
    /// Raised when an impression is recorded for an ad.
    /// </summary>
    private void OnInterstitialAdImpressionRecorded()
    {
        Debug.Log("Interstitial ad recorded an impression.");
    }

    /// <summary>
    /// Raised when a click is recorded for an ad.
    /// </summary>
    private void OnInterstitialAdClicked()
    {
        Debug.Log("Interstitial ad was clicked.");
    }

    /// <summary>
    /// Raised when an ad opened full screen content.
    /// </summary>
    private void OnInterstitialAdFullScreenContentOpened()
    {
        Debug.Log("Interstitial ad full screen content opened.");
    }

    /// <summary>
    /// Raised when the ad closed full screen content.
    /// </summary>
    private void OnInterstitialAdFullScreenContentClosed()
    {
        Debug.Log("Interstitial ad full screen content closed.");
        LoadInterstitialAd();
    }

    /// <summary>
    /// Raised when the ad failed to open full screen content.
    /// </summary>
    /// <param name="error"></param>
    private void OnInterstitialAdFullScreenContentFailed(AdError error)
    {
        Debug.LogError("Interstitial ad failed to open full screen content " +
                         "with error : " + error);
    }

    private async void TryLoadInterstitialAdAgain()
    {
        await Task.Delay(3000);
        LoadInterstitialAd();
    }



    #endregion

    #region RewardedAd

    private string androidTestRewardedAdID = "ca-app-pub-3940256099942544/5224354917";
    private string iosTestRewardedAdID = "ca-app-pub-3940256099942544/1712485313";

    private string androidRewardedAdID = "ca-app-pub-4048498709664067/9892495710";
    private string iosRewardedAdID = "ca-app-pub-3940256099942544/1712485313";

    private RewardedAd rewardedAd;

    public override void LoadRewardedAd()
    {
        string adUnitId = "";

        if (testMode)
        {
#if UNITY_ANDROID
                adUnitId = androidTestRewardedAdID;
#elif UNITY_IPHONE
                adUnitId = iosTestRewardedAdID;
#else
            adUnitId = "unused";
#endif
        }
        else
        {
#if UNITY_ANDROID
                adUnitId = androidRewardedAdID;
#elif UNITY_IPHONE
                adUnitId = iosRewardedAdID;
#else
            adUnitId = "unused";
#endif
        }

        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest();

        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    //TryLoadInterstitialAdAgain();
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);
            });
    }

    public override void ShowRewardedAd(Action rewardAction)
    {
        if (rewardedAd == null)
        {
            Debug.LogWarning("Rewarded Ad not loaded yet.");
            LoadRewardedAd();
        }

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                rewardAction?.Invoke();
            });
        }

    }


    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdClicked += OnRewardedAdClicked;
        ad.OnAdImpressionRecorded += OnRewardedAdImpressionRecorded;
        ad.OnAdPaid += OnRewardedAdPaid;
        ad.OnAdFullScreenContentClosed += OnRewardedAdFullScreenContentClosed;
        ad.OnAdFullScreenContentOpened += OnRewardedAdFullScreenContentOpened;
        ad.OnAdFullScreenContentFailed += OnRewardedAdFullScreenContentFailed;
    }


    /// <summary>
    /// Raised when the ad is estimated to have earned money.
    /// </summary>
    /// <param name="adValue"></param>
    private void OnRewardedAdPaid(AdValue adValue)
    {
        Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
              adValue.Value,
              adValue.CurrencyCode));
    }

    /// <summary>
    /// Raised when an impression is recorded for an ad.
    /// </summary>
    private void OnRewardedAdImpressionRecorded()
    {
        Debug.Log("Rewarded ad recorded an impression.");
    }

    /// <summary>
    /// Raised when a click is recorded for an ad.
    /// </summary>
    private void OnRewardedAdClicked()
    {
        Debug.Log("Rewarded ad was clicked.");
    }

    /// <summary>
    /// Raised when an ad opened full screen content.
    /// </summary>
    private void OnRewardedAdFullScreenContentOpened()
    {
        Debug.Log("Rewarded ad full screen content opened.");
    }

    /// <summary>
    /// Raised when the ad closed full screen content.
    /// </summary>
    private void OnRewardedAdFullScreenContentClosed()
    {
        Debug.Log("Rewarded ad full screen content closed.");
        LoadRewardedAd();
    }

    /// <summary>
    /// Raised when the ad failed to open full screen content.
    /// </summary>
    /// <param name="error"></param>
    private void OnRewardedAdFullScreenContentFailed(AdError error)
    {
        Debug.LogError("Rewarded ad failed to open full screen content " +
                         "with error : " + error);
    }

    private IEnumerator TryLoadRewardedAdAgain()
    {
        yield return new WaitForSeconds(3);
        LoadRewardedAd();

    }

    #endregion

}