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
    }
}
