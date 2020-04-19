using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartupController : MonoBehaviour
{
    private bool rotatingToLeft;
    private int z;

    private bool bHasAnimationFadeoutBeenStarted = false;

    [SerializeField]
    public GameObject headphonesPicture = null;

    [SerializeField]
    public Animator anim;

    [SerializeField]
    public Image blackImage;

    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        //Initialise Google Firebase Crashlytics.
        InitCrashlytics();
    }

    void Update()
    {
        if (rotatingToLeft)
        {
            headphonesPicture.transform.Rotate(0, 0, 2, Space.Self);

            if (z++ >= 10)
                rotatingToLeft = false;
        }
        else
        {
            headphonesPicture.transform.Rotate(0, 0, -2, Space.Self);

            if (z-- <= -10)
                rotatingToLeft = true;
        }

        if (Time.time >= 3 && !bHasAnimationFadeoutBeenStarted)
        {
            //Fade out and load main menu
            StartCoroutine(FadeOut());

            bHasAnimationFadeoutBeenStarted = true;
        }
    }
    IEnumerator FadeOut()
    {
        anim.SetBool("FadeOut", true);
        yield return new WaitUntil(() => blackImage.color.a == 1);
        SceneManager.LoadScene("MainMenu");
    }

    private void InitCrashlytics()
    {
        //https://firebase.google.com/docs/crashlytics/get-started?platform=unity&authuser=0
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Crashlytics will use the DefaultInstance, as well;
                // this ensures that Crashlytics is initialized.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here for indicating that your project is ready to use Firebase.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
}
