using System;
using System.Windows.Forms;
using System.Drawing;
using MiniGames;

namespace MiniGames.MiniGames
{
    public class ClickTheButtonGame : Form, IMiniGame, IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private Button clickButton;
        private Label labelTime, winClicksLebel, currentDifficultyLabel;
        private Difficulty currentDifficulty;
        private System.Windows.Forms.Timer timer;
        private int timeLeft, clicks, winClicks = 0;
        private Random place = new Random();
    }
}
