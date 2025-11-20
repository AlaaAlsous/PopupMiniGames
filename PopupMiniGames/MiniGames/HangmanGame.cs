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
        private GameData gameData;
        private GameResult result = new GameResult();
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
        public HangmanGame(GameData data)
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
            };
            guessButton.Click += GuessButtonClick;
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
            this.gameData = data;
        }

        public void StartGame(Difficulty difficulty)
        {
            currentDifficulty = difficulty;
            ConfigureDifficulty(difficulty);
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
            secretWord = PickWord(difficulty);
            guessedLetters.Clear();
            mistakes = 0;
            labelWord.Text = GetHiddenWord();
            labelInfo.Text = $"Mistakes: {mistakes}/{maxMistakes}";
            inputBox.Text = "";
            inputBox.Focus();
            this.ShowDialog();
        }

        private void ConfigureDifficulty(Difficulty difficulty)
        {
            maxMistakes = difficulty switch
            {
                Difficulty.Easy => 10,
                Difficulty.Medium => 9,
                Difficulty.Hard => 8,
                _ => 9
            };
        }

        private string PickWord(Difficulty difficulty)
        {
            var random = new Random();
            return difficulty switch
            {
                Difficulty.Easy => easyWords[random.Next(easyWords.Count)],
                Difficulty.Medium => mediumWords[random.Next(mediumWords.Count)],
                Difficulty.Hard => hardWords[random.Next(hardWords.Count)],
                _ => mediumWords[random.Next(mediumWords.Count)]
            };
        }

        private void GuessButtonClick(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputBox.Text)) return;
            char guess = char.ToUpper(inputBox.Text[0]);
            inputBox.Clear();
            if (!char.IsLetter(guess))
            {
                MessageBox.Show("Please enter a letter!");
                return;
            }
            if (guessedLetters.Contains(guess))
            {
                MessageBox.Show("You already guessed that letter!");
                return;
            }
            guessedLetters.Add(guess);

            if (secretWord.Contains(guess))
            {
                labelWord.Text = GetHiddenWord();

                if (!labelWord.Text.Contains("_"))
                {
                    GameOver(true);
                }
            }
            else
            {
                mistakes++;
                MessageBox.Show("Wrong! Try agian!");
                labelInfo.Text = $"Mistakes: {mistakes}/{maxMistakes}";
                if (mistakes >= maxMistakes)
                {
                    MessageBox.Show($"The word was ({secretWord})");
                    GameOver(false);
                }
            }
            inputBox.Focus();
        }

        private string GetHiddenWord()
        {
            return string.Join(" ", secretWord.Select(c => guessedLetters.Contains(c) ? c : '_'));
        }

        private void GameOver(bool won)
        {
            result = new GameResult
            {
                Points = won ? 10 : 0,
                Mistakes = won ? 0 : 5,
                Won = won,
                GameName = "Hangman Game"
            };
            GameEnded?.Invoke(this, result);
            this.Close();
        }

        public void Cleanup()
        {
            guessButton.Click -= GuessButtonClick;
            foreach (Control c in this.Controls)
                c.Dispose();
            this.Controls.Clear();
            this.Hide();
            this.Close();
            this.Dispose();
        }
    }
}
