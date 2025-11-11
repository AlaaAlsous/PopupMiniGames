using System;

namespace MiniGames
{
    public interface IMiniGame
    {
        event EventHandler<GameResult> GameEnded;
        void StartGame();
    }

    public class GameResult : EventArgs
    {
        public bool Won { get; set; }
        public int Points { get; set; }
        public int Mistakes { get; set; }
    }
}