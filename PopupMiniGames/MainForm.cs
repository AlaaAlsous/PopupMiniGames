using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MiniGames.MiniGames;
using MiniGames.UI;

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
            gameData.SetDifficulty(Difficulty.Medium);

            string basePath = Path.Combine(Application.StartupPath, @"..\..\..\UIAssets");
            //MessageBox.Show(Path.GetFullPath(basePath));
            uiManager = new UIManager(this, basePath);
            uiManager.CreateButton("PLAY.png", "PLAYPRESS.png", new Point(310, 100), StartNextGame);
            uiManager.CreateButton("OPTIONS.png", "OPTIONSPRESS.png", new Point(310, 200), ShowOptions);
            uiManager.CreateButton("EXIT.png", "EXITPRESS.png", new Point(310, 300), () => Application.Exit());
            LoadMiniGames();
        }
        private void ShowOptions()
        {
            Form optionsForm = new Form();
            optionsForm.Width = 800;
            optionsForm.Height = 600;
            optionsForm.Text = "Options";
            optionsForm.FormBorderStyle = FormBorderStyle.None;
            optionsForm.StartPosition = FormStartPosition.CenterParent;
            optionsForm.BackColor = Color.Black;
            optionsForm.TransparencyKey = Color.Black;

            string basePath = Path.Combine(Application.StartupPath, @"..\..\..\UIAssets");
            UIManager uiOption = new UIManager(optionsForm, basePath);
            uiOption.CreateButton("EASY.png", "EASYPRESS.png", new Point(310, 100), () =>
            {
                gameData.SetDifficulty(Difficulty.Easy);
                optionsForm.Close();
            });

            uiOption.CreateButton("MEDIUM.png", "MEDIUMPRESS.png", new Point(310, 200), () =>
            {
                gameData.SetDifficulty(Difficulty.Medium);
                optionsForm.Close();
            });

            uiOption.CreateButton("HARD.png", "HARDPRESS.png", new Point(310, 300), () =>
            {
                gameData.SetDifficulty(Difficulty.Hard);
                optionsForm.Close();
            });

            optionsForm.ShowDialog();
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
                MessageBox.Show("No mini-games have been added yet!");
                return;
            }

            int index = rand.Next(miniGames.Count);
            var instance = Activator.CreateInstance(miniGames[index]) as IMiniGame;
            if (instance != null)
            {
                instance.GameEnded += OnMiniGameEnded!;
                instance.StartGame(gameData.Difficulty);
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