using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int iNbHighscore = 3;//We keep only 3 best score

    //For debuging use
    public void ClearAllHighScore()
    {
        PlayerPrefs.SetString("highscores", "");
    }

    public bool NewScore(int newScore, int iLevel, Guid gameGuid)
    {
        //Get the 3 best score
        List<HighScoreData> lsHighScores = GetHighScores();

        //Verify if we have a score corresponding to the same gameGuid (this means user made a highscore, watched a rewarded video, and reached a higher score)
        HighScoreData currentGameHighScore = lsHighScores.Find(s => s.partyID == gameGuid.ToString());

        if (currentGameHighScore != null)
        {
            //Update corresponding highscore if needed...typically it will always be needed
            if (currentGameHighScore.score < newScore)
            {
                currentGameHighScore.score = newScore;
                currentGameHighScore.level = iLevel;

                lsHighScores.RemoveAll(s => s.partyID == gameGuid.ToString());
                lsHighScores.Add(currentGameHighScore);
            }
        }
        else
        {
            bool bIsNewScore = false;

            //Si on a moins de 3 records, le nouveau score est forcément un record.
            //Sinon si l'index du nouveau score est 0, 1, ou 2, c'est un record.
            if (lsHighScores.Count < iNbHighscore)
                bIsNewScore = true;
            else if (lsHighScores.FindIndex(s => s.score < newScore) >= 0 && lsHighScores.FindIndex(s => s.score < newScore) < iNbHighscore) //Verify if we have a new best score
                bIsNewScore = true;

            if (!bIsNewScore)
                return false;

            //Add the new best score
            lsHighScores.Add(new HighScoreData { score = newScore, level = iLevel, partyID = gameGuid.ToString() });
        }

        //Sort the score from the highest to the lowest
        lsHighScores.Sort((a, b) => b.score.CompareTo(a.score));

        //We keep only 3 best score
        while (lsHighScores.Count > iNbHighscore)
            lsHighScores.RemoveAt(iNbHighscore);

        //Save the list of best score into playerperfs in a json string format.
        SaveHighScores(lsHighScores);

        return true;
    }

    public List<HighScoreData> GetHighScores()
    {
        string sJsonHighScores = PlayerPrefs.GetString("highscores");

        if (sJsonHighScores == "")
            return new List<HighScoreData>();

        HighscoreDataHelper highScores = JsonUtility.FromJson<HighscoreDataHelper>(sJsonHighScores);
        
        return highScores.HighScores ?? new List<HighScoreData>();
    }

    private void SaveHighScores(List<HighScoreData> lsHighScores)
    {
        HighscoreDataHelper helper = new HighscoreDataHelper { HighScores = lsHighScores };

        string sJsonHighScores = JsonUtility.ToJson(helper);
        
        PlayerPrefs.SetString("highscores", sJsonHighScores);
    }

    /// <summary>
    /// Data helper to serialize JSON data. JSONUtility doesn't like the root object to be a List<T>.
    /// </summary>
    [Serializable]
    public struct HighscoreDataHelper
    {
        public List<HighScoreData> HighScores;
    }

    [Serializable]
    public class HighScoreData
    {
        public int score;
        public int level;
        public string pseudo;
        public string partyID;
    }
}

