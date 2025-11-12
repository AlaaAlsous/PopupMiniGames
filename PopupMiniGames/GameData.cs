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
                    MaxScore = 15;
                    MaxMistakes = 10;
                    break;
                case Difficulty.Medium:
                    MaxScore = 25;
                    MaxMistakes = 7;
                    break;
                case Difficulty.Hard:
                    MaxScore = 35;
                    MaxMistakes = 5;
                    break;
            }
        }
    }
}