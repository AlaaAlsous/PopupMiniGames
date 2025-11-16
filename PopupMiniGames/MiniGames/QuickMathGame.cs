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
        public QuickMathGame()
        {
            this.Text = "Quick Math Game";
            this.Width = 600;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Beige;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;

            labelQuestion = new Label() { Left = 170, Top = 370, Width = 300, Height = 100, ForeColor = Color.SaddleBrown, Font = new Font("Arial", 30) };
            labelTime = new Label() { Left = 60, Top = 50, Width = 200, Height = 60, ForeColor = Color.DarkOrange, Font = new Font("Arial", 18) };
            currentDifficultyLabel = new Label() { Left = 370, Top = 30, Width = 300, Height = 60, ForeColor = Color.OliveDrab, Font = new Font("Arial", 14), Text = $"Difficulty: {currentDifficulty}", };
            labelScore = new Label() { Left = 60, Top = 120, Width = 300, Height = 60, ForeColor = Color.DarkOliveGreen, Text = "Points: 0", Font = new Font("Arial", 18) };
            labelWrong = new Label() { Left = 60, Top = 190, Width = 300, Height = 60, ForeColor = Color.Firebrick, Text = "Wrong Answers: 0", Font = new Font("Arial", 18) };
            textBoxAnswer = new TextBox() { Left = 100, Top = 260, Width = 200, Height = 60, Font = new Font("Arial", 18) };
            buttonCheck = new Button() { Left = 320, Top = 257, Width = 200, Height = 50, Text = "Answer", Font = new Font("Arial", 18) };
            textBoxAnswer.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    buttonCheck.PerformClick();
                }
            };
            timer.Interval = 1000;
            this.Controls.Add(labelQuestion);
            this.Controls.Add(labelTime);
            this.Controls.Add(labelScore);
            this.Controls.Add(labelWrong);
            this.Controls.Add(textBoxAnswer);
            this.Controls.Add(buttonCheck);
            this.Controls.Add(currentDifficultyLabel);
            this.FormClosed += (s, e) => timer.Stop();
        }

        public void StartGame(Difficulty difficulty)
        {
            score = 0;
            wrongAnswer = 0;
            labelScore.Text = "Points: 0";
            labelWrong.Text = "Wrong Answers: 0";

            MessageBox.Show(
                $"Welcome to Quick Math Game!\n\n" +
                $"• Difficulty: {difficulty}\n" +
                $"• Time per question: {(difficulty == Difficulty.Easy ? 15 : difficulty == Difficulty.Medium ? 12 : 10)} seconds\n\n" +
                $"You will receive math questions about {(difficulty == Difficulty.Easy ? " + och -" : difficulty == Difficulty.Medium ? "+ , - och *" : "+ , - , * och /")}\n" +
                $"Your task is to solve as many as possible before the time runs out.\n\n" +
                $"Good luck!",
                "Game Instructions",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            this.ShowDialog();
        }

        public void Cleanup()
        {

        }
    }
}