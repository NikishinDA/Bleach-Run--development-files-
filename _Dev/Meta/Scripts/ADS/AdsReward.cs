using System;
using UnityEngine;

public class AdsReward : MonoBehaviour
{
    //[SerializeField] private StatModule statModule;
    
    private const string ADUnitId = "d766753392913af0";
    private int _retryAttempt;
    
    private const string AdType = "reward";
    private string _placement;

    private bool _isRewardEnd;
    private bool _isRewardWatched;

    private AvailableResult _availableResult;
    private StartedResult _startedResult;
    private WatchResult _watchResult;

    public bool IsRewardReady()
    {
        return MaxSdk.IsRewardedAdReady(ADUnitId);
    }

    public bool IsRewardEnd => _isRewardEnd;

    public bool IsRewardWatched => _isRewardWatched;

    public void ShowReward(string placement)
    {
        _placement = placement;
        _isRewardWatched = false;
        _isRewardEnd = false;
        _availableResult = AvailableResult.DEFAULT_VALUE;
        _startedResult = StartedResult.DEFAULT_VALUE;
        _watchResult = WatchResult.DEFAULT_VALUE;

        if (IsRewardReady())
            _availableResult = AvailableResult.SUCCES;
        else
            _availableResult = AvailableResult.NOT_AVAILABLE;
        
        AvailableStatLogEvent();
        MaxSdk.ShowRewardedAd(ADUnitId);
    }
    
    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void AvailableStatLogEvent()
    {
        // statModule.AdsAvailable(AdType, _placement, _availableResult.ToString().ToLower());
    }
    
    private void StartedStatLogEvent()
    {
        // statModule.AdsStart(AdType, _placement, _startedResult.ToString().ToLower());
    }
    
    private void WatchStatLogEvent()
    {
        // statModule.AdsWatch(AdType, _placement, _watchResult.ToString().ToLower());
    }
    
    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(ADUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(adUnitId) will now return 'true'

        // Reset retry attempt
        _retryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
        // Rewarded ad failed to load 
        // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)

        _retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, _retryAttempt));
    
        Invoke("LoadRewardedAd", (float) retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        if (_availableResult == AvailableResult.SUCCES)
        {
            _startedResult = StartedResult.FAIL;
            StartedStatLogEvent();
        }
        
        _isRewardEnd = true;
        
        // Rewarded ad failed to display. We recommend loading the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId)
    {
        if (_availableResult == AvailableResult.NOT_AVAILABLE)
        {
            _availableResult = AvailableResult.WAITED;
            AvailableStatLogEvent();
        }

        _startedResult = StartedResult.START;
        StartedStatLogEvent();
    }

    private void OnRewardedAdClickedEvent(string adUnitId)
    {
        if (_startedResult == StartedResult.START && 
            _watchResult != WatchResult.WATCHED && 
            _watchResult != WatchResult.CLICKED)
        {
            _watchResult = WatchResult.CLICKED;
            WatchStatLogEvent();
        }
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {
        if (_startedResult == StartedResult.START && _watchResult != WatchResult.CLICKED)
        {
            _watchResult = WatchResult.WATCHED;
            WatchStatLogEvent();
        }
        
        // Rewarded ad was displayed and user should receive the reward
        
        _isRewardWatched = true;
        _isRewardEnd = true;
    }
    
    private void OnRewardedAdDismissedEvent(string adUnitId)
    {
        if (_startedResult == StartedResult.START && _watchResult != WatchResult.CLICKED && _watchResult != WatchResult.WATCHED)
        {
            _watchResult = WatchResult.CANCELED;
            WatchStatLogEvent();
        }
        
        _isRewardEnd = true;
        
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }
}
