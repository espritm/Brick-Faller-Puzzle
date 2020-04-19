using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoAdController : MonoBehaviour
{
    //DEV
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";

    //PROD
    //private string rewardedAdUnitId = "TODO : Copy From Google Admob website";

    private RewardedAd rewardedAd;

    public event EventHandler OnRewardedAdWatched;

    private static VideoAdController instance;


    public static VideoAdController Get()
    {
        return instance;
    }


    void Start()
    {
        instance = this;

        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(initStatus => { }); initialization is done in MainMenucontroller.
        LoadRewardedAd();
    }

    #region Rewarded Ad
    private void LoadRewardedAd()
    {
        this.rewardedAd = new RewardedAd(rewardedAdUnitId);


        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void StartRewardedAd()
    {
        if (this.rewardedAd.IsLoaded())
            this.rewardedAd.Show();

#if UNITY_EDITOR
        if (OnRewardedAdWatched != null)
            OnRewardedAdWatched(this, new EventArgs());
#endif
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToLoad event received with message: " + args.Message);
        LoadRewardedAd();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.Message);
        LoadRewardedAd();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdClosed event received");

        //Load the next rewarded ad
        LoadRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

        //Load the next rewarded ad
        LoadRewardedAd(); //à voir si nécessaire

        OnRewardedAdWatched(this, args);
    }
    #endregion
}
