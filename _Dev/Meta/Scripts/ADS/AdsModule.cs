using System;
using UnityEngine;

public class AdsModule : MonoBehaviour
{
    [NonSerialized] public static AdsModule instance;

    //[SerializeField] private StatModule statModule;
    [SerializeField] private AdsBanner banner;
    [SerializeField] private AdsReward reward;
    [SerializeField] private AdsInterstitial interstitial;
    
    [Space]
    [SerializeField] private float adsCooldown;
    [SerializeField] private bool isBannerOn;
    
    private const string SDKKey = "Fxp5PuXCMlWt2XhsZXBqyqOcWSf7DCb6S82fQ8qZsvCHvuOrl950wAP7A6OhJPKzSdzE8IopJlFpaLW584VJtz";
    private float _adsTimer;
    private bool _isAdsTime;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance == this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        InitializeAdsModule();
    }
    
    private void InitializeAdsModule()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
        };
        
        MaxSdk.SetSdkKey(SDKKey);
        MaxSdk.InitializeSdk();

        interstitial.InitializeInterstitialAds("LevelEnd");
        reward.InitializeRewardedAds();
        if (isBannerOn)
            banner.InitializeBannerAds();
    }

    private void Start()
    {
        if (isBannerOn)
            banner.ShowBanner();
    }
    
    private void Update()
    {
        IterateAdsTimer();
    }

    public bool IsInterstitialReady()
    {
        return interstitial.IsInterstitialsReady();
    }
    
    public void ShowInterstitial(string placement)
    {
        if (!_isAdsTime)
            return;

        interstitial.ShowInterstitials(placement);
        _isAdsTime = false;
        _adsTimer = 0f;
    }
    
    public bool IsInterstitialEnd()
    {
        return interstitial.IsInterstitialEnd;
    }
    
    public bool IsRewardReady()
    {
        return reward.IsRewardReady();
    }
    
    public void ShowReward(string placement)
    {
        reward.ShowReward(placement);
        _isAdsTime = false;
        _adsTimer = 0f;
    }

    public bool IsRewardEnd()
    {
        return reward.IsRewardEnd;
    }

    public bool IsRewardWatched()
    {
        return reward.IsRewardWatched;
    }
    
    private void IterateAdsTimer()
    {
        if(_isAdsTime)
            return;
        
        _adsTimer += Time.deltaTime;
        if (_adsTimer > adsCooldown)
            _isAdsTime = true;
    }
}

public enum AvailableResult
{
    DEFAULT_VALUE,
    SUCCES,
    NOT_AVAILABLE,
    WAITED
}

public enum StartedResult
{
    DEFAULT_VALUE,
    FAIL,
    START
}

public enum WatchResult
{
    DEFAULT_VALUE,
    WATCHED,
    CLICKED,
    CANCELED
}
