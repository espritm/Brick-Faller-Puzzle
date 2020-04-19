using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayBackgroundSoundController : MonoBehaviour
{
    public AudioSource slower;
    public AudioSource normal;
    public AudioSource faster;

    private static GamePlayBackgroundSoundController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        SetSlowerBackgroundMusic();
    }

    public static GamePlayBackgroundSoundController Get()
    {
        return instance;
    }

    public static void SetSlowerBackgroundMusic()
    {
        GamePlayBackgroundSoundController.Get().slower.gameObject.SetActive(true);
        GamePlayBackgroundSoundController.Get().normal.gameObject.SetActive(false);
        GamePlayBackgroundSoundController.Get().faster.gameObject.SetActive(false);
    }

    public static void SetNormalBackgroundMusic()
    {
        GamePlayBackgroundSoundController.Get().slower.gameObject.SetActive(false);
        GamePlayBackgroundSoundController.Get().normal.gameObject.SetActive(true);
        GamePlayBackgroundSoundController.Get().faster.gameObject.SetActive(false);
    }

    public static void SetFasterBackgroundMusic()
    {
        GamePlayBackgroundSoundController.Get().slower.gameObject.SetActive(false);
        GamePlayBackgroundSoundController.Get().normal.gameObject.SetActive(false);
        GamePlayBackgroundSoundController.Get().faster.gameObject.SetActive(true);
    }

    public static void PauseBackgroundMusic()
    {
        GamePlayBackgroundSoundController.Get().slower.gameObject.SetActive(false);
        GamePlayBackgroundSoundController.Get().normal.gameObject.SetActive(false);
        GamePlayBackgroundSoundController.Get().faster.gameObject.SetActive(false);
    }
}
