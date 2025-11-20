using MiniGames;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MiniGames.MiniGames
{
    public class KalleAnkaEscapesGame : Form, IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private PictureBox kalle;
        private List<PictureBox> obstacles = new List<PictureBox>();
        private Random random = new Random();
        private System.Windows.Forms.Timer timer;
        private Label livesLabel, scoreLabel, currentDifficultyLabel;
        private int lives, speed, score, maxScore;
        private Difficulty currentDifficulty;
        private string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\GameAssets");
        private GameData gameData;
        private GameResult result = new GameResult();
        public KalleAnkaEscapesGame(GameData data)
        {
            this.Text = "Kalle Anka Escapes Game";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.LightSkyBlue;
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true;
            string kalleImage = Path.Combine(assetsPath, "Kalle.png");
            string kajsaImage = Path.Combine(assetsPath, "Kajsa.png");
            kalle = new PictureBox()
            {
                Size = new Size(75, 75),
                Location = new Point(320, 420),
                BackColor = Color.Transparent,
                Image = Image.FromFile(kalleImage),
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            livesLabel = new Label()
            {
                AutoSize = true,
                Location = new Point(520, 30),
                Font = new Font("Consolas", 14, FontStyle.Bold),
            };
            scoreLabel = new Label()
            {
                AutoSize = true,
                Location = new Point(520, 60),
                Font = new Font("Consolas", 14, FontStyle.Bold),
            };
            currentDifficultyLabel = new Label()
            {
                Location = new Point(480, 90),
                Font = new Font("Consolas", 12),
                AutoSize = true,
                ForeColor = Color.OliveDrab,
            };
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 30;
            this.Controls.Add(currentDifficultyLabel);
            this.Controls.Add(scoreLabel);
            this.Controls.Add(livesLabel);
            this.Controls.Add(kalle);
            for (int i = 0; i < 3; i++)
            {
                PictureBox obstacle = new PictureBox()
                {
                    Size = new Size(100, 100),
                    BackColor = Color.Transparent,
                    Image = Image.FromFile(kajsaImage),
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
                obstacles.Add(obstacle);
                this.Controls.Add(obstacle);
            }
            this.FormClosed += (s, e) => timer.Stop();
            this.gameData = data;
        }

        public void StartGame(Difficulty difficulty)
        {
            currentDifficulty = difficulty;
            currentDifficultyLabel.Text = $"Difficulty: {difficulty}";
            switch (difficulty)
            {
                case Difficulty.Easy:
                    speed = 5; lives = 6; maxScore = 20; break;
                case Difficulty.Medium:
                    speed = 8; lives = 5; maxScore = 25; break;
                case Difficulty.Hard:
                    speed = 10; lives = 4; maxScore = 30; break;
            }

            MessageBox.Show(
                $"Help Donald run for his life with the arrow keys!\nWatch out!! Daisy is chasing him!\nLives: {lives}, Speed: {speed}\nGood luck, duck hero!",
                "Instructions",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            timer.Start();
            this.ShowDialog();
        }
        private bool gameEnded = false;
        private void TimerTick(object? sender, EventArgs e)
        {
            if (gameEnded) return;
            foreach (var o in obstacles)
            {
                o.Top += speed;
                if (kalle.Bounds.IntersectsWith(o.Bounds))
                {
                    lives--;
                }
                if (o.Top >= this.ClientSize.Height)
                {
                    score++;
                }
            }
            livesLabel.Text = $"Lives: {lives}";
            scoreLabel.Text = $"Score: {score}/{maxScore}";

            if (lives <= 0)
            {
                gameEnded = true;
                timer.Stop();
                MessageBox.Show("Donald got caught! Better luck next time!");
            }
            if (score >= maxScore)
            {
                gameEnded = true;
                timer.Stop();
                MessageBox.Show("Donald escaped!");
            }
        }
    }
}
