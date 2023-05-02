using GameAnalyticsSDK;
using UnityEngine;

public class AdsBanner : MonoBehaviour
{
    //[SerializeField] private StatModule statModule;

    private const string bannerAdUnitId = "5b03dc57fbb693df";

    public void InitializeBannerAds()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerLoadFailedEvent;
        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);
    }

    private void OnBannerLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Banner, "AppLovin", "GameScreen",
            TranslateErrorCode(errorInfo.Code));
    }

    public void ShowBanner()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerAdUnitId);
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        var res = "watched";
        // statModule.AdsWatch(
        //     "banner", 
        //     "footer_banner", 
        //     res);
        GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.Banner, "AppLovin", "GameScreen");
    }

    private GAAdError TranslateErrorCode(MaxSdkBase.ErrorCode code)
    {
        return code switch
        {
            MaxSdkBase.ErrorCode.Unspecified => GAAdError.Undefined,
            MaxSdkBase.ErrorCode.NoFill => GAAdError.NoFill,
            MaxSdkBase.ErrorCode.AdLoadFailed => GAAdError.UnableToPrecache,
            MaxSdkBase.ErrorCode.NetworkError => GAAdError.InvalidRequest,
            MaxSdkBase.ErrorCode.NetworkTimeout => GAAdError.InvalidRequest,
            MaxSdkBase.ErrorCode.NoNetwork => GAAdError.Offline,
            MaxSdkBase.ErrorCode.FullscreenAdAlreadyShowing => GAAdError.InternalError,
            MaxSdkBase.ErrorCode.FullscreenAdNotReady => GAAdError.InternalError,
            MaxSdkBase.ErrorCode.NoActivity => GAAdError.InternalError,
            MaxSdkBase.ErrorCode.DontKeepActivitiesEnabled => GAAdError.InternalError,
            _ => GAAdError.Unknown
        };
    }
}