using System;
using System.Windows.Forms;
using System.Drawing;
using MiniGames;
namespace MiniGames.MiniGames
{
    public class QuickMathGame : Form, IMiniGame, IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private Random rnd = new Random();
        private int num1, num2, answer, timeLeft, score, wrongAnswer;
        private Difficulty currentDifficulty;
        private Label labelQuestion, labelTime, labelScore, labelWrong, currentDifficultyLabel;
        private TextBox textBoxAnswer;
        private Button buttonCheck;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
    }
}