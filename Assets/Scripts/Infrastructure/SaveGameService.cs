using UnityEngine;

namespace Infrastructure.Services
{
    public class SaveGameService : ISaveGameService
    {
        private const string LevelKey = "CurrentLevelIndex";
        private const string ScoreKey = "CurrentScore";

        public void SaveCurrentLevel(int levelIndex)
        {
            PlayerPrefs.SetInt(LevelKey, levelIndex);
            PlayerPrefs.Save();
        }

        public int LoadCurrentLevel(int defaultIndex = 0)
        {
            return PlayerPrefs.GetInt(LevelKey, defaultIndex);
        }

        public void SaveScore(int score)
        {
            PlayerPrefs.SetInt(ScoreKey, score);
            PlayerPrefs.Save();
        }

        public int LoadScore(int defaultScore = 0)
        {
            return PlayerPrefs.GetInt(ScoreKey, defaultScore);
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteKey(LevelKey);
            PlayerPrefs.DeleteKey(ScoreKey);
            PlayerPrefs.Save();
        }
    }
}
