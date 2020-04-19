using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    public GameObject HighscorePanel;

    [SerializeField]
    public GameObject OptionsPanel;

    [SerializeField]
    public Dropdown LanguageDropdown;

    [SerializeField]
    public Toggle ReplayTutoToggle;

    [SerializeField]
    public Text Highscoretext1;
    [SerializeField]
    public Text Highscoretext2;
    [SerializeField]
    public Text Highscoretext3;

    [SerializeField]
    public GameObject MainMenuPanel;

    public void PlayGame()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        LevelLoaderController.Get().LoadScene("Gameplay");
        BannerAdController.Get().DestroyAd();
    }

    private void Start()
    {
        MainMenuPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        HighscorePanel.SetActive(false);

        InitSettings();
    }

    public void ShowHighScore()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        HighscorePanel.SetActive(true);
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(false);

        List<ScoreController.HighScoreData> lsHighScore = FindObjectOfType<ScoreController>().GetHighScores();

        if (lsHighScore.Count >= 1)
            Highscoretext1.text = Translation.Get("score") + " : " + lsHighScore[0].score + "\n" + Translation.Get("level") + " : " + lsHighScore[0].level;
        else
            Highscoretext1.text = "";

        if (lsHighScore.Count >= 2)
            Highscoretext2.text = Translation.Get("score") + " : " + lsHighScore[1].score + "\n" + Translation.Get("level") + " : " + lsHighScore[1].level;
        else
            Highscoretext2.text = "";

        if (lsHighScore.Count >= 3)
            Highscoretext3.text = Translation.Get("score") + " : " + lsHighScore[2].score + "\n" + Translation.Get("level") + " : " + lsHighScore[2].level;
        else
            Highscoretext3.text = "";
    }

    public void DismissHighScore()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        HighscorePanel.SetActive(false);
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void ShowOptions()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        HighscorePanel.SetActive(false);
        OptionsPanel.SetActive(true);
        MainMenuPanel.SetActive(false);

        if (PlayerPrefs.HasKey("language"))
        {
            if (PlayerPrefs.GetString("language") == "EN")
                LanguageDropdown.value = 1;
            else if (PlayerPrefs.GetString("language") == "FR")
                LanguageDropdown.value = 2;
            else if (PlayerPrefs.GetString("language") == "N")
                LanguageDropdown.value = 3;
        }
    }

    public void OnLanguageSelected(int iIndex)
    {
        string sPrevious = PlayerPrefs.GetString("language");

        if (LanguageDropdown.value == 1)
            PlayerPrefs.SetString("language", "EN");
        else if (LanguageDropdown.value == 2)
            PlayerPrefs.SetString("language", "FR");
        else if (LanguageDropdown.value == 3)
            PlayerPrefs.SetString("language", "N");
        else
            PlayerPrefs.DeleteKey("language");

        if (sPrevious != PlayerPrefs.GetString("language"))
        {
            Translation.SwitchLanguage();
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public void DismissOptions()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        HighscorePanel.SetActive(false);
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void ShowLeaderboardUI()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        GooglePlayGameController.ShowLeaderboardUI();
    }

    public void ShowAchievementUI()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        GooglePlayGameController.ShowAchievementUI();
    }

    public void ShowCredits()
    {
        FindObjectOfType<SoundsEffectsController>().PlaybuttonCliked();

        LevelLoaderController.Get().LoadScene("Credits");
        BannerAdController.Get().DestroyAd();
    }

    private void InitSettings()
    {
        //If user has not done tuto, tuto will be prompted on next new game
        if (PlayerPrefs.GetInt("HasDoneTuto") == 0)
            ReplayTutoToggle.isOn = true;
        else
            ReplayTutoToggle.isOn = false;
    }

    public void OnReplayTutoToggleValueChanged(bool value)
    {
        if (ReplayTutoToggle.isOn)
            PlayerPrefs.SetInt("HasDoneTuto", 0);
        else
            PlayerPrefs.SetInt("HasDoneTuto", 1);
    }
}
