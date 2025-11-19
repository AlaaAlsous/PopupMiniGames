using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MiniGames.MiniGames
{
    public class FindMatchGame : IMiniGameWithCleanup
    {
        public FindMatchGame() { }
        public event EventHandler<GameResult>? GameEnded;
        private List<(Image img, string name)> imagePool = new List<(Image img, string name)>();
        private Difficulty difficulty;
        private System.Windows.Forms.Timer roundTimer;
        private Control parentContainer;
        private Random rng = new Random();
        private PictureBox targetPicture;
        private List<PictureBox> optionBoxes = new();
        private int mistakes = 0;
        private int maxMistakes = 3;
        private int roundTimeMs = 5000;
        private int totalOptions = 20; // antal små bilder
        private bool gameOver = false;
        private int currentRound = 0;
        private int maxRounds = 5;
        private string findMatchPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\GameAssets\FindMatchImages"
        );
        

        public FindMatchGame(Control parent)
        {
            parentContainer = parent;
        }
                private void LoadImages()
        {
            imagePool.Clear();

            if (!Directory.Exists(findMatchPath))
            {
                MessageBox.Show($"Bildmappen hittades inte:\n{Path.GetFullPath(findMatchPath)}");
                return;
            }

            string[] files = Directory.GetFiles(findMatchPath, "*.*")
                                    .Where(f => f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                    .ToArray();

            foreach (string file in files)
            {
                try
                {
                    Image img = Image.FromFile(file);
                    string name = Path.GetFileName(file); // filnamnet sparas
                    imagePool.Add((img, name));
                }
                catch { }
            }

            if (imagePool.Count < 2)
                MessageBox.Show("Behöver minst 2 bilder för spelet!");
        }
                public void StartGame(Difficulty difficulty)
        {
            this.difficulty = difficulty;
            roundTimeMs = difficulty switch
            {
                Difficulty.Easy => 6000,
                Difficulty.Medium => 4000,
                Difficulty.Hard => 2500,
                _ => 4000
            };
            LoadImages();
            gameOver = false;
            mistakes = 0;
            currentRound = 0;

            if (parentContainer == null)
            {
                if (Application.OpenForms.Count == 0)
                    throw new InvalidOperationException("No open forms found.");
                parentContainer = Application.OpenForms[0]!;
                Form mainForm = parentContainer as Form;
                if (mainForm != null)
                    mainForm.TopMost = true;
            }

            if (imagePool.Count < 2)
            {
                EndGame(false);
                return;
            }
            MessageBox.Show(
                "Welcome to Find Match!\n\n" +
                "Your goal: Click the picture at the top among the options below.\n" +
                $"You have {maxMistakes} mistakes allowed and {roundTimeMs / 1000} seconds per round.\n\n" +
                "Good luck!",
                "Find Match Instructions",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            ShowNextRound();
        }
        private void ShowNextRound()
        {
            Cleanup();

            if (currentRound >= maxRounds)
            {
                EndGame(true); 
                return;
            }

            currentRound++;

            // Kontroll: finns det bilder?
            if (imagePool.Count == 0)
            {
                MessageBox.Show("Inga bilder finns att spela med!");
                EndGame(false);
                return;
            }

            var target = imagePool[rng.Next(imagePool.Count)];

            targetPicture = new PictureBox
            {
                Image = target.img,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(150, 150),
                Location = new Point((parentContainer.Width - 150) / 2, 20),
                BorderStyle = BorderStyle.FixedSingle
            };
            parentContainer.Controls.Add(targetPicture);

            roundTimer?.Stop();
            roundTimer?.Dispose();
            roundTimer = new System.Windows.Forms.Timer();
            roundTimer.Interval = roundTimeMs;
            roundTimer.Tick += RoundTimeElapsed;
            roundTimer.Start();

            GenerateOptions(target);
        }
