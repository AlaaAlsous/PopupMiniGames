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
        private System.Windows.Forms.Timer? roundTimer;
        private Control parentContainer = new Panel();
        private Random rng = new Random();
        private PictureBox? targetPicture;
        private List<PictureBox> optionBoxes = new();
        private int mistakes = 0;
        private int maxMistakes = 3;
        private int roundTimeMs = 5000;
        private int totalOptions = 14; // antal små bilder
        private bool gameOver = false;
        private int currentRound = 0;
        private int maxRounds = 5;
        private GameData? gameData;
        private GameResult result = new GameResult();
        private string findMatchPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\GameAssets\FindMatchImages"
        );
       

        public FindMatchGame(GameData data)
        {
            parentContainer = Application.OpenForms[0]!;
            this.gameData = data;
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
                Form? mainForm = parentContainer as Form;
                if (mainForm != null)
                    mainForm.TopMost = true;
            }

            if (imagePool.Count < 2)
            {
                GameOver(false);
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
                GameOver(true); 
                return;
            }

            currentRound++;

            // Kontroll: finns det bilder?
            if (imagePool.Count == 0)
            {
                MessageBox.Show("Inga bilder finns att spela med!");
                GameOver(false);
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
        private void RoundTimeElapsed(object? sender, EventArgs e)
        {
            roundTimer?.Stop();
            roundTimer?.Dispose();
            roundTimer = null;

            mistakes++;

            if (mistakes >= maxMistakes)
            {
                GameOver(false);
                return;
            }
            ShowNextRound();
        }
        private void GenerateOptions((Image? img, string name) target)
        {
            int startX = 40;
            int startY = 200;
            int spacing = 110;
            int imagesPerRow = 7; // max 7 annars förs

            List<(Image img, string name)> wrongImages = imagePool.Where(x => x.name != target.name).ToList();

            if (wrongImages.Count < totalOptions - 1)
            {
                MessageBox.Show("Inte tillräckligt med unika bilder för alternativen!");
                return;
            }

            wrongImages = wrongImages.OrderBy(x => rng.Next()).ToList();

            List<(Image img, string name)> options = new List<(Image img, string name)>();
            options.AddRange(wrongImages.Take(totalOptions - 1));
            options.Add((target.img!, target.name));
            options = options.OrderBy(x => rng.Next()).ToList();

            for (int i = 0; i < totalOptions; i++)
            {
                var opt = options[i];

                PictureBox box = new PictureBox
                {
                    Image = opt.img,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Size = new Size(90, 90),
                    Location = new Point(startX + (i % imagesPerRow) * spacing,
                                        startY + (i / imagesPerRow) * spacing),
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = (opt.name == target.name) // true = korrekt
                };

                box.Click += OptionClick;
                parentContainer?.Controls.Add(box);
                optionBoxes.Add(box);
            }
        }
        private bool ImagesAreEqual(Image a, Image b)
        {
            if (a == null || b == null) return false;
            return a == b; 
        }

        private void OptionClick(object? sender, EventArgs e)
        {
            if (gameOver) return;
            if (sender is not PictureBox clicked) return;
            if (clicked.Tag is not bool isCorrect) return;

            roundTimer?.Stop();
            roundTimer?.Dispose();
            roundTimer = null;

            if (isCorrect)
            {
                ShowNextRound();
            }
            else
            {
                mistakes++;
                clicked.BorderStyle = BorderStyle.Fixed3D;

                if (mistakes >= maxMistakes)
                {
                    GameOver(false);
                }
                else
                {
                    ShowNextRound();
                }
            }
        }
                private void GameOver(bool won)
        {
            if (gameOver) return;
            gameOver = true;

            MessageBox.Show(
                won ? "Correct! You win!" : $"Wrong! Game Over.\nMistakes: {mistakes}/{maxMistakes}",
                "Find Match Result",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            Cleanup();

            GameEnded?.Invoke(this, new GameResult
            {
                Won = won,
                Points = won ? 10 : 0,
                Mistakes = won ? 0 : 5,
                GameName = "FindMatchGame"
            });
        }
                public void Cleanup()
        {
            if (parentContainer is Form mainForm)
                mainForm.TopMost = false;
            if (targetPicture != null)
            {
                parentContainer.Controls.Remove(targetPicture);
                targetPicture.Dispose();
                targetPicture = null;
            }

            foreach (var b in optionBoxes)
            {
                try
                {
                    parentContainer.Controls.Remove(b);
                    b.Dispose();
                }
                catch { }
            }
            optionBoxes.Clear();
        }
    }
}
        