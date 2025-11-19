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
