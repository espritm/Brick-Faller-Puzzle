using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEffectsController : MonoBehaviour
{
    public AudioSource rotate;
    public AudioSource down;
    public AudioSource rowDeleted;
    public AudioSource levelUp;
    public AudioSource gameover;
    public AudioSource buttonCliked;
    public AudioSource backgroundMusic;

    private void Start()
    {
        if (backgroundMusic != null)
            backgroundMusic.volume = 0;
    }

    private void Update()
    {
        if (backgroundMusic != null && backgroundMusic.volume <= 0.7)
            backgroundMusic.volume += 0.005f;
    }

    public void PlayRotate()
    {
        rotate?.Play();
    }

    public void PlayDown()
    {
        down?.Play();
    }

    public void PlayRowDeleted()
    {
        rowDeleted?.Play();
    }

    public void PlayLevelUp()
    {
        levelUp?.Play();
    }

    public void PlaybuttonCliked()
    {
        buttonCliked?.Play();
    }

    public void PlayGameOver()
    {
        gameover?.Play();
    }
}
