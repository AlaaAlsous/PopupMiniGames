using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MiniGames.MiniGames
{
    public class CatchTapGame : IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private List<System.Windows.Forms.Timer> activeTimers = new List<System.Windows.Forms.Timer>();

        private Control parentContainer = null!;
        private List<PictureBox> objects = new List<PictureBox>();
        private Random rand = new Random();
        private Difficulty difficulty;

        private int score = 0;
        private int mistakes = 0;
        private int totalPopups = 0;

        private const int maxPopups = 14;
        private const int targetScore = 10;
        private const int maxMistakes = 4;

        private Label? scoreLabel;

        private string assetsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\GameAssets"
        );

        private bool gameOver = false;
        public void StartGame(Difficulty difficulty)
        {
            if (parentContainer == null)
            {
                if (Application.OpenForms.Count == 0)
                    throw new InvalidOperationException("No open forms found.");
                parentContainer = Application.OpenForms[0]!;
            }
            MessageBox.Show("CatchTapGame starting! Be ready to tap the popups!", "Get Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.difficulty = difficulty;

            SetupScoreLabel();
            ShowNextObject();
        }

        private void SetupScoreLabel()
        {
            if (scoreLabel == null)
            {
                scoreLabel = new Label
                {
                    Text = $"Score: {score}",
                    AutoSize = false,
                    Size = new Size(200, 50),
                    Location = new Point(10, 10),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    BackColor = Color.Transparent
                };
                parentContainer.Controls.Add(scoreLabel);
                scoreLabel.BringToFront();
            }
        }
    }
}