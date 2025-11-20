using System;
using System.Windows.Forms;
using System.Drawing;
using MiniGames;

namespace MiniGames.MiniGames
{
    public class ClickTheButtonGame : Form, IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private Button clickButton;
        private Label labelTime, winClicksLebel, currentDifficultyLabel;
        private Difficulty currentDifficulty;
        private System.Windows.Forms.Timer timer;
        private int timeLeft, clicks, winClicks = 0;
        private Random place = new Random();
        private GameData gameData;
        private GameResult result = new GameResult();
        public ClickTheButtonGame(GameData data)
        {
            this.Text = "Click The Button Game";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.BurlyWood;
            currentDifficultyLabel = new Label()
            {
                Left = 220,
                Top = 20,
                Width = 300,
                Height = 60,
                ForeColor = Color.OliveDrab,
                Font = new Font("Arial", 12),
                Text = $"Difficulty: {currentDifficulty}",
            };
            labelTime = new Label()
            {
                Text = $"Time left: {timeLeft}",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(140, 320),
            };
            clickButton = new Button()
            {
                Text = "Click Me!",
                Size = new Size(120, 50),
                Location = new Point(130, 120),
                BackColor = Color.SandyBrown,
                Cursor = Cursors.Hand,
            };
            this.AcceptButton = null;
            clickButton.TabStop = false;
            clickButton.Click += (s, e) =>
            {
                clicks++;
                int maxX = this.ClientSize.Width - clickButton.Width;
                int maxY = this.ClientSize.Height - clickButton.Height - 130;
                clickButton.Location = new Point(place.Next(maxX), place.Next(maxY));
            };
            winClicksLebel = new Label
            {
                ForeColor = Color.Red,
                Font = new Font("Arial", 8, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(95, 360),
            };
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += TimerTick;
            this.Controls.Add(labelTime);
            this.Controls.Add(clickButton);
            this.FormClosed += (s, e) => timer.Stop();
            this.Controls.Add(winClicksLebel);
            this.Controls.Add(currentDifficultyLabel);
            this.gameData = data;
        }

        public void StartGame(Difficulty difficulty)
        {
            currentDifficulty = difficulty;
            gameData.SetDifficulty(difficulty);
            clicks = 0;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    timeLeft = 10;
                    winClicks = 12;
                    break;
                case Difficulty.Medium:
                    timeLeft = 12;
                    winClicks = 20;
                    break;
                case Difficulty.Hard:
                    timeLeft = 15;
                    winClicks = 25;
                    break;
            }
            currentDifficultyLabel.Text = $"Difficulty: {currentDifficulty}";
            winClicksLebel.Text = $"You must click ({winClicks}) times to win";
            labelTime.Text = $"Time left: {timeLeft}";
            MessageBox.Show(
            $"Click the button as many times as you can before time runs out\n" +
            $"Time is: {timeLeft} seconds\n\n" +
            $"You must click at least {winClicks} times to win.",
            "Instructions",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
            );
            timer.Start();
            this.ShowDialog();
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            timeLeft--;
            labelTime.Text = $"Time left: {timeLeft}";
            if (timeLeft <= 0)
            {
                timer.Stop();

                if (clicks >= winClicks)
                {
                    MessageBox.Show($"Time's up! You clicked {clicks} times. You won!");
                    GameOver(true);
                }
                else
                {
                    MessageBox.Show($"Time's up! You clicked {clicks} times. You lost!");
                    GameOver(false);
                }
            }
        }

        private void GameOver(bool won)
        {
            timer.Stop();
            result = new GameResult
            {
                Points = won ? 10 : 0,
                Mistakes = won ? 0 : 5,
                Won = won,
                GameName = "Click The Button Game",
            };
            GameEnded?.Invoke(this, result);
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.KeyCode) == Keys.Enter || (keyData & Keys.KeyCode) == Keys.Space)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void Cleanup()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= TimerTick;
            }
            foreach (Control c in this.Controls)
            {
                c.Dispose();
            }
            this.Controls.Clear();
            this.Hide();
            this.Close();
            this.Dispose();
        }
    }
}
