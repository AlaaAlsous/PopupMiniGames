using PopupMiniGames.UI;
using System;
using System.Collections;
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
        private PictureBox difficultyIcon = new PictureBox();
        private UIManager uiManager;

        private Menu mainMenu;
        public MainForm()
        {
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            gameData.SetDifficulty(Difficulty.Medium);

            string basePath = Path.Combine(Application.StartupPath, @"..\..\..\UIAssets");
            //MessageBox.Show(Path.GetFullPath(basePath));
            uiManager = new UIManager(this, basePath);
            mainMenu = new Menu(this);
            mainMenu.AddMenuButton(new MenuButton("PLAY.png", "PLAYPRESS.png", new Point(310, 100), StartNextGame, basePath));
            mainMenu.AddMenuButton(new MenuButton("OPTIONS.png", "OPTIONSPRESS.png", new Point(310, 200), ShowOptions, basePath));
            mainMenu.AddMenuButton(new MenuButton("EXIT.png", "EXITPRESS.png", new Point(310, 300), () => Application.Exit(), basePath));
            this.KeyDown += mainMenu.OnKeyDown!;
            mainMenu.onBack += Application.Exit;

            currentDifficultyIcon();
            LoadMiniGames();
        }
        private void currentDifficultyIcon()
        {
            difficultyIcon = new PictureBox
            {
                Location = new Point(235, 400),
                Size = new Size(350, 150),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            string img = gameData.Difficulty switch
            {
                Difficulty.Easy => "SL_EASY.png",
                Difficulty.Medium => "SL_MEDIUM.png",
                Difficulty.Hard => "SL_HARD.png",
                _ => "SL_EASY.png"
            };
            string path = Path.Combine(Application.StartupPath, @"..\..\..\UIAssets", img);

            difficultyIcon.Image = Image.FromFile(path);

            this.Controls.Add(difficultyIcon);
            difficultyIcon.BringToFront();
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
                currentDifficultyIcon();
                optionsForm.Close();
            });

            uiOption.CreateButton("MEDIUM.png", "MEDIUMPRESS.png", new Point(310, 200), () =>
            {
                gameData.SetDifficulty(Difficulty.Medium);
                currentDifficultyIcon();
                optionsForm.Close();
            });

            uiOption.CreateButton("HARD.png", "HARDPRESS.png", new Point(310, 300), () =>
            {
                gameData.SetDifficulty(Difficulty.Hard);
                currentDifficultyIcon();
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