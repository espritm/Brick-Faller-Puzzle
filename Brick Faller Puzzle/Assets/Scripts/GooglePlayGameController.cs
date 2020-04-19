using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GooglePlayGameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
            AuthenticateUser();
    }

    // Update is called once per frame
    void AuthenticateUser()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Logged in to Google Play Games Services.");

                //Load next scene here if needed.
            }
            else
            {
                Debug.LogError("Unable to sign in to Google Play Games Services.");

                //Display an error message if needed.
            }
        });
    }

    public static void PostToScoreLeaderboard(int newScore)
    {
        Social.ReportScore(newScore, GPGSIds.leaderboard_high_score, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Posted new score to score leaderboard");

                //Display a validation/congrat message
            }
            else
            {
                Debug.LogError("Unable to post new score to score leaderboard");

                //Display an error message if needed
            }
        });
    }

    public static void PostToLevelLeaderboard(int newLevel)
    {
        Social.ReportScore(newLevel, GPGSIds.leaderboard_high_level, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Posted new score to level leaderboard");

                //Display a validation/congrat message
            }
            else
            {
                Debug.LogError("Unable to post new score to level leaderboard");

                //Display an error message if needed
            }
        });
    }

    public static void ShowLeaderboardUI()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }

    public static void ShowAchievementUI()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }


    public static void ManageAchievement(int newScore, int newLevel, int iNbRowDeleted, int iLastHitScore)
    {
        if (newScore > 0 && newLevel <= 2)
        {
            Social.ReportProgress(GPGSIds.achievement_first_line_falled, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_first_line_falled to achievements");
                else
                    Debug.LogError("Unable to post achievement_first_line_falled to achievements");
            });
        }


        if (newLevel >= 2)
        {
            Social.ReportProgress(GPGSIds.achievement_wrecker, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_wrecker to achievements");
                else
                    Debug.LogError("Unable to post achievement_wrecker to achievements");
            });
        }


        if (newScore >= 1000)
        {
            Social.ReportProgress(GPGSIds.achievement_beginner, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_beginner to achievements");
                else
                    Debug.LogError("Unable to post achievement_beginner to achievements");
            });
        }


        if (newLevel >= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_destroyer, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_destroyer to achievements");
                else
                    Debug.LogError("Unable to post achievement_destroyer to achievements");
            });
        }


        if (newScore >= 3000)
        {
            Social.ReportProgress(GPGSIds.achievement_intermediate, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_intermediate to achievements");
                else
                    Debug.LogError("Unable to post achievement_intermediate to achievements");
            });
        }


        if (iNbRowDeleted >= 4)
        {
            Social.ReportProgress(GPGSIds.achievement_optimization, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_optimization to achievements");
                else
                    Debug.LogError("Unable to post achievement_optimization to achievements");
            });
        }


        if (newLevel >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_pulverizator, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_pulverizator to achievements");
                else
                    Debug.LogError("Unable to post achievement_pulverizator to achievements");
            });
        }


        if (newScore >= 10000)
        {
            Social.ReportProgress(GPGSIds.achievement_pro, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_pro to achievements");
                else
                    Debug.LogError("Unable to post achievement_pro to achievements");
            });
        }


        if (iLastHitScore >= 1400)
        {
            Social.ReportProgress(GPGSIds.achievement_secret_weapon, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_secret_weapon to achievements");
                else
                    Debug.LogError("Unable to post achievement_secret_weapon to achievements");
            });
        }

        if (newLevel >= 15)
        {
            Social.ReportProgress(GPGSIds.achievement_ripper, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_ripper to achievements");
                else
                    Debug.LogError("Unable to post achievement_ripper to achievements");
            });
        }


        if (newLevel >= 20)
        {
            Social.ReportProgress(GPGSIds.achievement_atomizer, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_atomizer to achievements");
                else
                    Debug.LogError("Unable to post achievement_atomizer to achievements");
            });
        }


        if (newLevel >= 69)
        {
            Social.ReportProgress(GPGSIds.achievement_god_of_block_faller, 100, (bool success) =>
            {
                if (success)
                    Debug.Log("Posted achievement_god_of_block_faller to achievements");
                else
                    Debug.LogError("Unable to post achievement_god_of_block_faller to achievements");
            });
        }


        //ManageGodOfBlockFallerAchievement(newLevel); //unity bug : save an increment dismiss previous achievement earned
    }


    static void ManageGodOfBlockFallerAchievement(int newLevel)
    {
        List<LevelUnlocked> lsLevels;

        //Initialize (get existing list of unlocked levels or create it)
        if (PlayerPrefs.HasKey("LevelsUnlocked"))
        {
            string sjson = PlayerPrefs.GetString("LevelsUnlocked");
            lsLevels = JsonUtility.FromJson<LevelUnlockedManager>(sjson).LevelsUnlocked;
        }
        else
        {
            lsLevels = new List<LevelUnlocked>();

            for(int i = 1; i < 70; i++)
                lsLevels.Add(new LevelUnlocked { Level = i, Unlocked = false });
        }

        //Update unlocked levels
        for(int i = 1; i <= newLevel; i++)
        {
            LevelUnlocked tmp = lsLevels[i];

            if (tmp.Unlocked == false)
            {
                //New level unlocked !
                tmp.Unlocked = true;
                Social.ReportProgress(GPGSIds.achievement_god_of_block_faller, 1, (bool success) =>
                {
                    if (success)
                        Debug.Log("Posted achievement_god_of_block_faller to achievements");
                    else
                        Debug.LogError("Unable to post achievement_god_of_block_faller to achievements");
                });
            }
        }

        //Save
        string sJson = JsonUtility.ToJson(new LevelUnlockedManager { LevelsUnlocked = lsLevels });
        PlayerPrefs.SetString("LevelsUnlocked", sJson);

        /*
         Problème : on enregistre les niveau en local. Et à chaque niveau, on incrémente l'achievement God Of Block.
         De ce fait, sur un unique téléphone, l'utilisateur devra atteindre les 69 niveaux pour incrémenter 69 fois l'achievement.
         Mais si l'utilisateur utilise plusieurs device, la liste des niveaux unlocked ne sera pas partagée sur tous les devices !
         L'utilisateur pourra alors incrémenter 69 fois l'achievement sans jamais dépasser le niveau 10 par exemple.
         Même problème si l'utilisateur désinstalle et réinstalle l'appli : les PlayerPrefs seront réinitialiser et l'utilisateur pourra
         incrémenter les niveaux unlocked à l'infini sans jamais dépasser le niveau 10 par exemple.

        Pour le moment : acceptable.
        TODO si ça devient problématique : enregistrer la liste des niveaux unlocked sur un serveur à distance en les rattachant à l'ID utilisateur de GooglePlayGame.
         */
    }

    [Serializable]
    public class LevelUnlockedManager
    {
        public List<LevelUnlocked> LevelsUnlocked = new List<LevelUnlocked>();
    }

    [Serializable]
    public class LevelUnlocked
    {
        public int Level;
        public bool Unlocked;
    }
}
