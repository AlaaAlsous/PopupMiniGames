namespace MiniGames
{
    public class GameData
    {
        public int Score { get; set; } = 0;

        public int Mistakes { get; set; } = 0;

        public int MaxScore { get; set; }

        public int MaxMistakes { get; set; }
        public Difficulty Difficulty { get; set; }
        public void SetDifficulty(Difficulty difficulty)
        {
            Difficulty = difficulty;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    MaxScore = 50;
                    MaxMistakes = 25;
                    break;
                case Difficulty.Medium:
                    MaxScore = 75;
                    MaxMistakes = 35;
                    break;
                case Difficulty.Hard:
                    MaxScore = 100;
                    MaxMistakes = 50;
                    break;
            }
        }
    }
}