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
    }
}
