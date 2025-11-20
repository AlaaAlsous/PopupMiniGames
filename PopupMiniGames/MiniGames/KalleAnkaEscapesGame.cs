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
    }
}
