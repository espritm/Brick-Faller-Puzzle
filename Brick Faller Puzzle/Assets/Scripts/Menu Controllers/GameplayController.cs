using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    private Guid currentGameGuid;//used to identify current party in score records

    private bool bHasScoreAndLevelLoosepanelImageBeenMooved = false;
    // private bool bHasWatchedRewardedAdToContinue;

    //Permet de faire le lien avec un GameObjet présent dans la scène !
    private bool bHasEventOnRewardedAdWatchedBeenAdded;

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    public GameObject loosePanel;

    [SerializeField]
    public Text scoreText;

    [SerializeField]
    public Text levelText;

    [SerializeField]
    public Image scoreAndLevelLoosepanelImage;

    [SerializeField]
    public Text scoreAndLevelLoosepanelText;

    [SerializeField]
    public Image continueLoosepanelButton;

    [SerializeField]
    public GameObject newHighscorePanelLoosepanel;

    [SerializeField]
    public Animator anim;

    private int score = 0;

    private int iCurrentLevel = 0;//in game, we have lvl1,2,3,4,5,6,7,8,9,10.....69. In code, we have 0,1,2,3,4,5,6,7,8,9......68 in order to make code more clear.


    void Awake()
    {
        currentGameGuid = Guid.NewGuid();
        pausePanel.SetActive(false);
        loosePanel.SetActive(false);
    }

    void Start()
    {
        UpdateScore(0);
        levelText.text = Translation.Get("level") + " : " + GetCurrentLevelDisplay();
    }
   
    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();
    }

    public void ResumeGame()
    {
        UpdateTimeScale();
        pausePanel.SetActive(false);
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();
    }

    public void DisplayLoosePanel()
    {
        GamePlayBackgroundSoundController.PauseBackgroundMusic();
        FindObjectOfType<SoundsEffectsController>().PlayGameOver();

        scoreAndLevelLoosepanelText.text = Translation.Get("level") + " : " + GetCurrentLevelDisplay() + "\n" + Translation.Get("score") + " : " + score;

        //Add user's high score if needed (in PlayerPerfs)
        bool bIsANewHighScore = FindObjectOfType<ScoreController>().NewScore(score, GetCurrentLevelDisplay(), currentGameGuid);

        if (bIsANewHighScore)
        {
            newHighscorePanelLoosepanel.SetActive(true);

            if (bHasScoreAndLevelLoosepanelImageBeenMooved)
            {
                scoreAndLevelLoosepanelImage.transform.position += new Vector3(0, -5, 0);
                continueLoosepanelButton.transform.position += new Vector3(0, -3, 0);
                bHasScoreAndLevelLoosepanelImageBeenMooved = false;
            }

        }
        else
        {
            newHighscorePanelLoosepanel.SetActive(false);

            if (!bHasScoreAndLevelLoosepanelImageBeenMooved)
            {
                scoreAndLevelLoosepanelImage.transform.position += new Vector3(0, 5, 0);
                continueLoosepanelButton.transform.position += new Vector3(0, 3, 0);
                bHasScoreAndLevelLoosepanelImageBeenMooved = true;
            }
        }

        /*if (bHasWatchedRewardedAdToContinue)
        {
            //We allow user to watch an ad and continue only one time per game
            continueLoosepanelButton.gameObject.SetActive(false);
        }*/

        //Add score to Google Play Services
        GooglePlayGameController.PostToScoreLeaderboard(score);
        GooglePlayGameController.PostToLevelLeaderboard(GetCurrentLevelDisplay());

        loosePanel.SetActive(true);
    }

    public void ContinueGame()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        if (!bHasEventOnRewardedAdWatchedBeenAdded)
        {
            VideoAdController.Get().OnRewardedAdWatched += OnRewardedAdWatched;
            bHasEventOnRewardedAdWatchedBeenAdded = true;
        }

        //Display rewarded ad
        VideoAdController.Get().StartRewardedAd();
    }

    private void OnRewardedAdWatched(object sender, EventArgs args)
    {
        UpdateBackgroundMusic();

        MatrixGrid.ClearAll();
        loosePanel.SetActive(false);
        FindObjectOfType<Spawner>().SpawnRandom();
        //bHasWatchedRewardedAdToContinue = true;
    }

    public void UpdateScore(int iNbRowDeleted)
    {
        if (iNbRowDeleted > 0)
            FindObjectOfType<SoundsEffectsController>().PlayRowDeleted();

        int iNbPointsBase = 0;
        if (iNbRowDeleted == 0)
            iNbPointsBase += 0;
        else if (iNbRowDeleted == 1)        //100pts a line
            iNbPointsBase += 100;
        else if (iNbRowDeleted == 2)        // 150pts a line
            iNbPointsBase += 300;
        else
            iNbPointsBase += iNbRowDeleted * 200;   // 200pts a line

        int scoreDone = iNbPointsBase + (iNbPointsBase * iCurrentLevel / 10);  //lvl1 : 100%, lvl2 : 110%, lvl3 : 120%, lvl4 : 130%, lvl5 : 140%, ..., lvl 9 : 180%, ... etc

        score += scoreDone;

        scoreText.text = " " + Translation.Get("score") + " :  " + score;

        UpdateLevel();

        GooglePlayGameController.ManageAchievement(score, GetCurrentLevelDisplay(), iNbRowDeleted, scoreDone);
    }

    private void UpdateLevel()
    {
        int previousLevel = iCurrentLevel;

        if (score >= 255900)
            iCurrentLevel = 69;
        else if (score >= 248800)
            iCurrentLevel = 68;
        else if (score >= 241800)
            iCurrentLevel = 67;
        else if (score >= 234900)
            iCurrentLevel = 66;
        else if (score >= 228100)
            iCurrentLevel = 65;
        else if (score >= 221400)
            iCurrentLevel = 64;
        else if (score >= 214800)
            iCurrentLevel = 63;
        else if (score >= 208300)
            iCurrentLevel = 62;
        else if (score >= 201900)
            iCurrentLevel = 61;
        else if (score >= 195600)
            iCurrentLevel = 60;
        else if (score >= 189400)
            iCurrentLevel = 59;
        else if (score >= 183300)
            iCurrentLevel = 58;
        else if (score >= 177300)
            iCurrentLevel = 57;
        else if (score >= 171400)
            iCurrentLevel = 56;
        else if (score >= 165600)
            iCurrentLevel = 55;
        else if (score >= 159900)
            iCurrentLevel = 54;
        else if (score >= 154300)
            iCurrentLevel = 53;
        else if (score >= 148800)
            iCurrentLevel = 52;
        else if (score >= 143400)
            iCurrentLevel = 51;
        else if (score >= 138100)
            iCurrentLevel = 50;
        else if (score >= 132900)
            iCurrentLevel = 49;
        else if (score >= 127800)
            iCurrentLevel = 48;
        else if (score >= 122800)
            iCurrentLevel = 47;
        else if (score >= 117900)
            iCurrentLevel = 46;
        else if (score >= 113100)
            iCurrentLevel = 45;
        else if (score >= 108400)
            iCurrentLevel = 44;
        else if (score >= 103800)
            iCurrentLevel = 43;
        else if (score >= 99300)
            iCurrentLevel = 42;
        else if (score >= 94900)
            iCurrentLevel = 41;
        else if (score >= 90600)
            iCurrentLevel = 40; 
        else if (score >= 86400)
            iCurrentLevel = 39;
        else if (score >= 82300)
            iCurrentLevel = 38;
        else if (score >= 78300)
            iCurrentLevel = 37;
        else if (score >= 74400)
            iCurrentLevel = 36;
        else if (score >= 70600)
            iCurrentLevel = 35;
        else if (score >= 66900)
            iCurrentLevel = 34;
        else if (score >= 63300)
            iCurrentLevel = 33;
        else if (score >= 59800)
            iCurrentLevel = 32;
        else if (score >= 56400)
            iCurrentLevel = 31;
        else if (score >= 53100)
            iCurrentLevel = 30;
        else if (score >= 49900)
            iCurrentLevel = 29;
        else if (score >= 46800)
            iCurrentLevel = 28;
        else if (score >= 43800)
            iCurrentLevel = 27;
        else if (score >= 40900)
            iCurrentLevel = 26;
        else if (score >= 38100)
            iCurrentLevel = 25;
        else if (score >= 35400)
            iCurrentLevel = 24;
        else if (score >= 32800)
            iCurrentLevel = 23;
        else if (score >= 30300)
            iCurrentLevel = 22;
        else if (score >= 27900)
            iCurrentLevel = 21;
        else if (score >= 25600)
            iCurrentLevel = 20;
        else if (score >= 23400)
            iCurrentLevel = 19;
        else if (score >= 21300)
            iCurrentLevel = 18;
        else if (score >= 19300)
            iCurrentLevel = 17;
        else if (score >= 17400)
            iCurrentLevel = 16;
        else if (score >= 15600)
            iCurrentLevel = 15;
        else if (score >= 13900)
            iCurrentLevel = 14;
        else if (score >= 12300)
            iCurrentLevel = 13;
        else if (score >= 10800)
            iCurrentLevel = 12;
        else if (score >= 9400)
            iCurrentLevel = 11;
        else if (score >= 8100)
            iCurrentLevel = 10;
        else if (score >= 6900)
            iCurrentLevel = 9;
        else if (score >= 5800)
            iCurrentLevel = 8;
        else if (score >= 4800)
            iCurrentLevel = 7;
        else if (score >= 3900)
            iCurrentLevel = 6;
        else if (score >= 3000)
            iCurrentLevel = 5;
        else if (score >= 2200)
            iCurrentLevel = 4;
        else if (score >= 1500)
            iCurrentLevel = 3;
        else if (score >= 900)
            iCurrentLevel = 2;
        else if (score >= 400)
            iCurrentLevel = 1;
        else
            iCurrentLevel = 0;

        //Sound effect level up
        if (previousLevel != iCurrentLevel)
            FindObjectOfType<SoundsEffectsController>().PlayLevelUp();

        //Background music
        UpdateBackgroundMusic();

        //Text Level displayed
        levelText.text = Translation.Get("level") + " : " + GetCurrentLevelDisplay();

        UpdateTimeScale();
    }

    private void UpdateBackgroundMusic()
    {
        if (iCurrentLevel == 0)
            GamePlayBackgroundSoundController.SetSlowerBackgroundMusic();
        else if (iCurrentLevel >= 1)
            GamePlayBackgroundSoundController.SetNormalBackgroundMusic();
        else if (iCurrentLevel >= 5)
            GamePlayBackgroundSoundController.SetFasterBackgroundMusic();
    }

    private int GetCurrentLevelDisplay()
    {
        return iCurrentLevel + 1;
    }

    private void UpdateTimeScale()
    {
        Time.timeScale = iCurrentLevel / 2 + 1;
    }
}
