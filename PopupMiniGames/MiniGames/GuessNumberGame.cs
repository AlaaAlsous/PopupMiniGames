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
        public GuessNumberGame()
        {
            this.Text = "Guess The Number Game";
            this.Width = 700;
            this.Height = 350;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightYellow;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            labelQuestion = new Label()
            {
                Left = 90,
                Top = 120,
                Width = 600,
                Height = 60,
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.SaddleBrown,
                Text = "Guess a number between 1 and 100"
            };
            labelAttempts = new Label()
            {
                Left = 40,
                Top = 40,
                Width = 300,
                Height = 40,
                Font = new Font("Arial", 15),
                ForeColor = Color.Brown,
            };
            currentDifficultyLabel = new Label()
            {
                Left = 470,
                Top = 40,
                Width = 400,
                Height = 40,
                Font = new Font("Arial", 15),
                ForeColor = Color.OliveDrab,
            };
            textBoxGuess = new TextBox()
            {
                Left = 140,
                Top = 200,
                Width = 200,
                Font = new Font("Arial", 20)
            };
            buttonGuess = new Button()
            {
                Left = 400,
                Top = 198,
                Width = 150,
                Height = 50,
                Text = "Guess",
                Font = new Font("Arial", 18)
            };
            textBoxGuess.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    buttonGuess.PerformClick();
            };
            this.Controls.Add(labelQuestion);
            this.Controls.Add(labelAttempts);
            this.Controls.Add(textBoxGuess);
            this.Controls.Add(buttonGuess);
            this.Controls.Add(currentDifficultyLabel);
        }
    }
}
