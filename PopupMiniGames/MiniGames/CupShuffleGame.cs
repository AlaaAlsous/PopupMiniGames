using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MiniGames.MiniGames
{
    public class CupGuessGame : IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;

        private PictureBox background;
        private Random rand = new Random();
        private List<PictureBox> cups = new List<PictureBox>();
        private int correctIndex = 0;
        private Control parentContainer = null!;
        private Label instructionLabel = null!;
        private int correctGuesses = 0;
        private int wrongGuesses = 0;
        private Difficulty currentDifficulty;
        private PictureBox titleImage = null!;
        private string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\GameAssets");

        public void StartGame(Difficulty difficulty)
        {
            currentDifficulty = difficulty;
            if (parentContainer == null)
                parentContainer = Application.OpenForms[0];

            SetupGameUI(difficulty);
        }
        private void SetupGameUI(Difficulty difficulty)
        {
            int cupCount = difficulty switch
            {
                Difficulty.Easy => 3,
                Difficulty.Medium => 4,
                Difficulty.Hard => 5,
                _ => 3
            };

            if (instructionLabel == null)
            {
                instructionLabel = new Label
                {
                    Text = "Guess the correct cup!",
                    AutoSize = false,
                    Size = new Size(400, 80),
                    Location = new Point((parentContainer.Width - 400) / 2, 10),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent,
                    Font = new Font("Arial", 16, FontStyle.Bold)
                };

                string titlePath = Path.Combine(assetsPath, "Title.png");
                if (File.Exists(titlePath))
                    titleImage.Image = Image.FromFile(titlePath);

                parentContainer.Controls.Add(instructionLabel);
                instructionLabel.BringToFront();
            }

            correctIndex = rand.Next(cupCount);

            foreach (var c in cups)
                parentContainer.Controls.Remove(c);
            cups.Clear();

            int startX = 50;
            int spacing = 200;
            int cupWidth = 180;
            int cupHeight = 220;

            correctIndex = rand.Next(cupCount);

            for (int i = 0; i < cupCount; i++)
            {
                PictureBox cup = new PictureBox
                {
                    Width = cupWidth,
                    Height = cupHeight,
                    Location = new Point(startX + i * spacing, 200),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Cursor = Cursors.Hand,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.Transparent
                };

                string cupImage = Path.Combine(assetsPath, "BOX_CLOSE.png");
                if (File.Exists(cupImage))
                    cup.Image = Image.FromFile(cupImage);

                int index = i;
                cup.Click += (s, e) => OnCupSelected(index);

                cups.Add(cup);
                parentContainer.Controls.Add(cup);
                cup.BringToFront();
            }
        }

    }
}
