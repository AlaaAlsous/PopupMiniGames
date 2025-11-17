using System;
using System.Drawing;
using System.Windows.Forms;
using MiniGames;

namespace MiniGames.MiniGames
{
    public class GuessNumberGame : Form, IMiniGame, IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private Random random = new Random();
        private int secretNumber;
        private int attemptsLeft;
        private Difficulty currentDifficulty;
        private Label labelQuestion, labelAttempts, currentDifficultyLabel;
        private TextBox textBoxGuess;
        private Button buttonGuess;
    }
}
