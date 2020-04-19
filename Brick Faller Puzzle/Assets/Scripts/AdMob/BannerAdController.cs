using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAdController : MonoBehaviour
{
    //DEV
    private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";

    //PROD
    //private string bannerAdUnitId = "TODO : Copy From Google Admob website";


    private BannerView bannerView;


    private static BannerAdController instance;


    public static BannerAdController Get()
    {
        return instance;
    }


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(initStatus => { }); initialization is done in MainMenucontroller.

        LoadBannerAd();
    }

    #region Banner Ad
    public void LoadBannerAd()
    {
        //Create an adaptive banner (https://developers.google.com/admob/unity/banner#banner_sizes)
        bannerView = new BannerView(bannerAdUnitId, AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom);


        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    public void DestroyAd()
    {
        bannerView.Destroy();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);

        LoadBannerAd();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeavingApplication event received");
    }
    #endregion
}
