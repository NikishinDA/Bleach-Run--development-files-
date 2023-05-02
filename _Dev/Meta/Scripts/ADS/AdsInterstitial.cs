using System;
using UnityEngine;
using GameAnalyticsSDK;
public class AdsInterstitial : MonoBehaviour
{
    //[SerializeField] private StatModule statModule;
    
    private const string ADUnitId = "37155fc1c10190a2";
    private int _retryAttempt;
    
    private const string AdType = "interstitial";
    private string _placement;
    
    private bool _isInterstitialEnd;
    
    private AvailableResult _availableResult;
    private StartedResult _startedResult;
    private WatchResult _watchResult;
    
    public bool IsInterstitialsReady()
    {
        return MaxSdk.IsInterstitialReady(ADUnitId);
    }

    public bool IsInterstitialEnd => _isInterstitialEnd;
    
    public void ShowInterstitials(string placement)
    {
        _placement = placement;
        _isInterstitialEnd = false;
        _availableResult = AvailableResult.DEFAULT_VALUE;
        _startedResult = StartedResult.DEFAULT_VALUE;
        _watchResult = WatchResult.DEFAULT_VALUE;

        if (IsInterstitialsReady())
            _availableResult = AvailableResult.SUCCES;
        else
            _availableResult = AvailableResult.NOT_AVAILABLE;
        
        AvailableStatLogEvent();
        MaxSdk.ShowInterstitial(ADUnitId, placement);
    }
    
    public void InitializeInterstitialAds(string initPlacement)
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;

        _placement = initPlacement;
        // Load the first interstitial
        LoadInterstitial();
    }


    private void AvailableStatLogEvent()
    {
        //GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.Interstitial, "AppLovin", adInfo.Placement);
    }
    
    private void StartedStatLogEvent()
    {
        // statModule.AdsStart(AdType, _placement, _startedResult.ToString().ToLower());
    }
    
    private void WatchStatLogEvent()
    {
        // statModule.AdsWatch(AdType, _placement, _watchResult.ToString().ToLower());
    }
    
    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(ADUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(adUnitId) will now return 'true'

        // Reset retry attempt
        _retryAttempt = 0;
        GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.Interstitial, "AppLovin", _placement);
    }

    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)

        _retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, _retryAttempt));
    
        Invoke("LoadInterstitial", (float) retryDelay);
        GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial, "AppLovin", _placement,
            TranslateErrorCode(errorInfo.Code));

    }

    private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        if (_availableResult == AvailableResult.SUCCES)
        {
            _startedResult = StartedResult.FAIL;
            StartedStatLogEvent();
        }
        
        _isInterstitialEnd = true;
        
        // Interstitial ad failed to display. We recommend loading the next ad
        LoadInterstitial();
        
        GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial, "AppLovin", _placement,
            TranslateErrorCode(errorInfo.Code));
    }

    private void OnInterstitialDisplayedEvent(string obj, MaxSdkBase.AdInfo adInfo)
    {
        if (_availableResult == AvailableResult.NOT_AVAILABLE)
        {
            _availableResult = AvailableResult.WAITED;
            //AvailableStatLogEvent();
        }

        _startedResult = StartedResult.START;
        StartedStatLogEvent();
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, "AppLovin", _placement);
    }
    
    private void OnInterstitialClickedEvent(string obj, MaxSdkBase.AdInfo adInfo)
    {
        if (_startedResult == StartedResult.START &&
            _watchResult != WatchResult.CLICKED)
        {
            _watchResult = WatchResult.CLICKED;
            WatchStatLogEvent();
        }
        GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Interstitial, "AppLovin", _placement);

    }
    
    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (_startedResult == StartedResult.START && _watchResult != WatchResult.CLICKED)
        {
            _watchResult = WatchResult.WATCHED;
            WatchStatLogEvent();
        }
        
        _isInterstitialEnd = true;
        
        // Interstitial ad is hidden. Pre-load the next ad
        LoadInterstitial();

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
