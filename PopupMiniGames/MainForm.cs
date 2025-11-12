using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MiniGames.MiniGames;
using PopupMiniGames.UI;

namespace MiniGames
{
    public partial class MainForm : Form
    {
        private List<Type> miniGames = new List<Type>();
        private Random rand = new Random();
        private GameData gameData = new GameData();

        private UIManager uiManager; 

        public MainForm()
        {
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UIAssets")
                            + System.IO.Path.DirectorySeparatorChar;
            uiManager = new UIManager(this, basePath);
            uiManager.CreateButton("PLAY.png", "PLAYPRESS.png", new Point(300, 100), StartNextGame);
            uiManager.CreateButton("OPTIONS.png", "OPTIONSPRESS.png", new Point(300, 200), ShowOptions); // <-- WIP
            uiManager.CreateButton("EXIT.png", "EXITPRESS.png", new Point(300, 300), () => Application.Exit());
            LoadMiniGames();
        }
        private void ShowOptions()
        {
            MessageBox.Show("Options: W.I.P");
        }
        private void StartNextGame()
        {
            if (gameData.Score >= gameData.MaxScore)
            {
                MessageBox.Show("You Win!");
                gameData.Score = 0;
                gameData.Mistakes = 0;
                return;
            }

            if (gameData.Mistakes >= gameData.MaxMistakes)
            {
                MessageBox.Show("Game Over!");
                gameData.Score = 0;
                gameData.Mistakes = 0;
                return;
            }

            if (miniGames.Count == 0)
            {
                MessageBox.Show("No minigames added!");
                return;
            }

            int index = rand.Next(miniGames.Count);
            var instance = Activator.CreateInstance(miniGames[index]) as IMiniGame;
            if (instance != null)
            {
                instance.GameEnded += OnMiniGameEnded!;
                instance.StartGame();
            }
            else
            {
                MessageBox.Show($"Could not create instance of {miniGames[index].Name}");
            }
        }
        private void LoadMiniGames()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && typeof(IMiniGame).IsAssignableFrom(type) && type.Namespace == "MiniGames.MiniGames")
                {
                    miniGames.Add(type);
                }
            }
        }
        private void OnMiniGameEnded(object sender, GameResult e)
        {
            gameData.Score += e.Points;
            gameData.Mistakes += e.Mistakes;
            StartNextGame();
        }
    }
}