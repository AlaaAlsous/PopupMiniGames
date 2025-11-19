using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MiniGames;

namespace MiniGames.MiniGames
{
    public class HangmanGame : Form, IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private string secretWord = "";
        private HashSet<char> guessedLetters = new HashSet<char>();
        private Label labelWord, labelInfo, currentDifficultyLabel;
        private TextBox inputBox;
        private Button guessButton;
        private int mistakes = 0, maxMistakes = 0;
        private Difficulty currentDifficulty;
        private readonly List<string> easyWords = new List<string>()
        {
            "CAT", "DOG", "CAR", "SUN", "MAP","SEE","CUP",
            "HAT", "PEN", "BED", "BUS", "KEY","BOY","COW"
        };
        private readonly List<string> mediumWords = new List<string>()
        {
            "HOME", "CODE", "CHAT", "CITY", "FISH","DUCK",
            "WALL", "BOOK", "GAME", "LION", "WIND",
        };
        private readonly List<string> hardWords = new List<string>()
        {
            "APPLE", "HOUSE", "BRAIN", "LIGHT", "SOUND","HUMAN",
            "WATER", "STONE", "SMILE", "PLANT", "HEART",
        };
        public HangmanGame()
        {
            this.Text = "Hangman Game";
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Bisque;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            labelWord = new Label()
            {
                Font = new Font("Consolas", 22, FontStyle.Bold),
                Location = new Point(160, 75),
                AutoSize = true
            };
            labelInfo = new Label()
            {
                Location = new Point(175, 160),
                Font = new Font("Arial", 14),
                AutoSize = true
            };
            inputBox = new TextBox()
            {
                Location = new Point(110, 220),
                Size = new Size(110, 70),
                Font = new Font("Arial", 15),
            };
            guessButton = new Button()
            {
                Text = "Guess",
                Location = new Point(260, 220),
                Size = new Size(130, 39)
            };
            currentDifficultyLabel = new Label()
            {
                Location = new Point(325, 20),
                Font = new Font("Arial", 12),
                AutoSize = true,
                ForeColor = Color.OliveDrab,
                Text = "Test"
            };
            inputBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    guessButton.PerformClick();
                }
            };
            this.Controls.Add(currentDifficultyLabel);
            this.Controls.Add(labelWord);
            this.Controls.Add(labelInfo);
            this.Controls.Add(inputBox);
            this.Controls.Add(guessButton);
        }
        public void StartGame(Difficulty difficulty)
        {
            currentDifficulty = difficulty;
            currentDifficultyLabel.Text = $"Difficulty: {difficulty}";
            MessageBox.Show(
                $"Welcome to Hangman Game!\n\n" +
                $"Your goal is to guess the hidden word before you run out of attempts.\n" +
                $"• Difficulty: {difficulty}\n" +
                $"• Attempts: {(difficulty == Difficulty.Easy ? 10 : difficulty == Difficulty.Medium ? 9 : 8)}\n" +
                $"• Each wrong guess reduces your remaining attempts.\n\n" +
                $"Good luck!",
                "Game Instructions",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            guessedLetters.Clear();
            mistakes = 0;
            labelInfo.Text = $"Mistakes: {mistakes}/{maxMistakes}";
            inputBox.Text = "";
            inputBox.Focus();
            this.ShowDialog();
        }
    }
}
