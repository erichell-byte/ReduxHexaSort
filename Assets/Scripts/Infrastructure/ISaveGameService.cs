namespace Infrastructure.Services
{
    public interface ISaveGameService
    {
        void SaveCurrentLevel(int levelIndex);
        int LoadCurrentLevel(int defaultIndex = 0);
        
        void SaveScore(int score);
        int LoadScore(int defaultScore = 0);
        void ResetProgress();
    }
}
