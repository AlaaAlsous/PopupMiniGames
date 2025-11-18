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
    }
}